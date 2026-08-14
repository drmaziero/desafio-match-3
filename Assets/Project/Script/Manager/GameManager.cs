using System;
using System.Collections.Generic;
using Controllers;
using GameLogic.Services;
using Models;
using ScriptableObjects.Level;
using ScriptableObjects.Score;
using UnityEngine;

namespace Project.Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField]
        private HudController _hudController;

        [SerializeField] private GameController _gameController;
        
        [Header("Score Settings")] 
        [SerializeField] private ScoreSettings _scoreSettings;

        [Header("Level Config")] 
        [SerializeField] private LevelListSettings _levelListSettings;
        
        private GameService _gameService;
        private ScoreService _scoreService;
        private LevelService _levelService;


        private void Awake()
        {
            _gameService = new GameService();
            _scoreService = new ScoreService(_scoreSettings.CreateConfig());
            _levelService = new LevelService(_levelListSettings.GetLevelConfigs());
            
            _gameService.ComputeScore += _scoreService.ComputeScore;
            _scoreService.ScoreChanged += OnScoreChanges;
            _levelService.MovementCountChanged += _hudController.OnMovementChanged;
            _gameController.SwapRequested += OnSwapRequested;
            _gameController.TurnCompleted += OnTurnCompleted;
        }

        private void OnDestroy()
        {
            _gameService.ComputeScore -= _scoreService.ComputeScore;
            _scoreService.ScoreChanged -= OnScoreChanges;
            _levelService.MovementCountChanged -= _hudController.OnMovementChanged;
            _gameController.SwapRequested -= OnSwapRequested;
            _gameController.TurnCompleted -= OnTurnCompleted;
        }

        public void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            _scoreService.Init();
            _levelService.Init();
            
            LevelConfig currentLevel = _levelService.GetCurrentLevel();
            _hudController.Init(currentLevel.Target);
            List<List<Tile>> board = _gameService.StartGame(currentLevel.BoardSize.x, currentLevel.BoardSize.y, currentLevel.Types);
            
            _gameController.Init(board);
        }

       

        private void OnScoreChanges(int score)
        {
            _hudController.OnScoreChanged(score);
            _levelService.UpdateScore(score);
        }
        
        private void OnSwapRequested(int fromX, int fromY, int toX, int toY)
        {
            bool isValid =
                _gameService.IsValidMovement(
                    fromX,
                    fromY,
                    toX,
                    toY);

            if (!isValid)
            {
                _gameController.AnimateInvalidSwap(
                    new Vector2Int(fromX, fromY),
                    new Vector2Int(toX, toY));
                return;
            }
            
            _levelService.DecreaseMovementCount();

            List<BoardSequence> result =
                _gameService.SwapTile(
                    fromX,
                    fromY,
                    toX,
                    toY);

            _gameController.AnimateValidSwap(
                new Vector2Int(fromX, fromY),
                new Vector2Int(toX, toY),
                result);
        }
        
        private void OnTurnCompleted()
        {
            if (_levelService.IsEndGame())
            {
                Debug.LogWarning("Game Finished");
                Debug.LogWarning($"Is Win: {_levelService.IsCompleteAllTargets()}");
            }
        }
    }
}