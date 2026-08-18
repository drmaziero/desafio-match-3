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
        [SerializeField] private HudController _hudController;
        [SerializeField] private UiController _uiController;
        [SerializeField] private InputController _inputController;
        [SerializeField] private BoardController _boardController;
        [SerializeField] private GameController _gameController;
        
        [Header("Score Settings")] 
        [SerializeField] private ScoreSettings _scoreSettings;

        [Header("Level Config")] 
        [SerializeField] private LevelListSettings _levelListSettings;
        
        private GameService _gameService;
        private ScoreService _scoreService;
        private LevelService _levelService;
        private LifeService _lifeService;
        private bool _isResolvingTurn;


        private void Awake()
        {
            _gameService = new GameService();
            _scoreService = new ScoreService(_scoreSettings.CreateConfig());
            _levelService = new LevelService(_levelListSettings.GetLevelConfigs());
            _lifeService = new LifeService();
            
            _gameService.ComputeScore += _scoreService.ComputeScore;
            _gameService.ComputeMatches += _levelService.ComputeMatches;
            _scoreService.ScoreChanged += OnScoreChanges;
            _levelService.MovementCountChanged += _hudController.OnMovementChanged;
            _levelService.TileCountChanged += _hudController.OnTileCounterChanged;
            _levelService.SpecialCountChanged += _hudController.OnSpecialCounterChanged;
            _lifeService.LifeChanged += OnLifeChanged;
            
            _inputController.SwapRequested += OnSwapRequested;
            _gameController.TurnCompleted += OnTurnCompleted;
            _gameController.InvalidMovementCompleted += OnInvalidMovementCompleted;
            _gameController.TurnCompleted += _inputController.SetDragCompleted;
            _gameController.InvalidMovementCompleted += _inputController.SetDragCompleted;

            _uiController.TryRetryGameRequest += TryStartGame;
            _uiController.TryStartGameRequest += TryStartGame;
            
            _boardController.TileCreated += OnTileCreated;
            _boardController.OutOfBoard += OnOutBoard;
        }

        private void OnDestroy()
        {
            _gameService.ComputeScore -= _scoreService.ComputeScore;
            _gameService.ComputeMatches -= _levelService.ComputeMatches;
            
            _scoreService.ScoreChanged -= OnScoreChanges;
            _levelService.MovementCountChanged -= _hudController.OnMovementChanged;
            _levelService.TileCountChanged -= _hudController.OnTileCounterChanged;
            _levelService.SpecialCountChanged -= _hudController.OnSpecialCounterChanged;
            _lifeService.LifeChanged -= OnLifeChanged;
            
            _inputController.SwapRequested -= OnSwapRequested;
            _gameController.TurnCompleted -= OnTurnCompleted;
            _gameController.InvalidMovementCompleted -= OnInvalidMovementCompleted;
            _gameController.TurnCompleted -= _inputController.SetDragCompleted;
            _gameController.InvalidMovementCompleted -= _inputController.SetDragCompleted;
            
            _uiController.TryRetryGameRequest -= TryStartGame;
            _uiController.TryStartGameRequest -= TryStartGame;
            
            _boardController.TileCreated -= OnTileCreated;
            _boardController.OutOfBoard -= OnOutBoard;
        }

        private void StartGame()
        {
            _scoreService.Init();
            _levelService.Init();
            
            LevelConfig currentLevel = _levelService.GetCurrentLevel();
            _hudController.Init(currentLevel.Target, _lifeService.Life);
            List<List<Tile>> board = _gameService.StartGame(currentLevel.BoardSize.x, currentLevel.BoardSize.y, currentLevel.Types);
            
            _gameController.Init(board);
        }

        private void ShowNoLife()
        {
            _uiController.ShowNoLife(_lifeService.GetTimeUntilNextLife());
        }


        private void OnScoreChanges(int score)
        {
            _hudController.OnScoreChanged(score);
            _levelService.UpdateScore(score);
        }
        
        private void OnSwapRequested(int fromX, int fromY, int toX, int toY)
        {
            if (_isResolvingTurn)
                return;

            _isResolvingTurn = true;
            
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
            _isResolvingTurn = false;
            
            var isWin = _levelService.IsCompleteAllTargets();
            var isEndGame = _levelService.IsEndGame();
            
            if (!isEndGame)
                return;
            
            if (isWin)
                _levelService.UpgradeLevel();
            else
                _lifeService.TryDecreaseLife();
            
            _uiController.GameplayFinished(isWin);
        }

        private void OnInvalidMovementCompleted()
        {
            _isResolvingTurn = false;
        }

        private void OnLifeChanged(int life)
        {
            _hudController.UpdateLife(life);
        }
        
        private void TryStartGame()
        {
            if (!_lifeService.HasLife())
            {
                ShowNoLife();
                return;
            }
            
            _uiController.GoToGamePlay();
            StartGame();
        }
        
        private void OnTileCreated(TileController tileController)
        {
            _inputController.RegisterTileViewController(tileController);
        }
        
        private void OnOutBoard()
        {
            _inputController.SetDragCompleted();
        }
        
    }
}