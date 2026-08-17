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
        [SerializeField] private UiController _uiController;

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
            _gameService.ComputeMatches += _levelService.ComputeMatches;
            _scoreService.ScoreChanged += OnScoreChanges;
            _levelService.MovementCountChanged += _hudController.OnMovementChanged;
            _levelService.TileCountChanged += _hudController.OnTileCounterChanged;
            _levelService.SpecialCountChanged += _hudController.OnSpecialCounterChanged;
            
            _gameController.SwapRequested += OnSwapRequested;
            _gameController.TurnCompleted += OnTurnCompleted;
        }

        private void OnDestroy()
        {
            _gameService.ComputeScore -= _scoreService.ComputeScore;
            _scoreService.ScoreChanged -= OnScoreChanges;
            _levelService.MovementCountChanged -= _hudController.OnMovementChanged;
            _levelService.TileCountChanged -= _hudController.OnTileCounterChanged;
            _levelService.SpecialCountChanged -= _hudController.OnSpecialCounterChanged;
            _gameController.SwapRequested -= OnSwapRequested;
            _gameController.TurnCompleted -= OnTurnCompleted;
            _gameService.ComputeMatches -= _levelService.ComputeMatches;
        }

        private void OnEnable()
        {
            StartGame();
        }

        private void OnDisable()
        {
            _gameController.Reset();
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
            var isWin = _levelService.IsCompleteAllTargets();
            var isEndGame = _levelService.IsEndGame();
            
            if (!isEndGame)
                return;
            
            if (isWin)
                _levelService.UpgradeLevel();
            
            _uiController.GameplayFinished(isWin);
        }
    }
}