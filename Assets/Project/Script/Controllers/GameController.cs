using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameLogic.Services;
using Models;
using ScriptableObjects;
using ScriptableObjects.Score;
using UnityEngine;
using Views;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private ScoreView _scoreView; 
        [SerializeField] private BoardView _boardView;
        
        [Header("Board Config")]
        [SerializeField] private int _boardHeight = 10;
        [SerializeField] private int _boardWidth = 10;

        [Header("Score Settings")] 
        [SerializeField] private ScoreSettings _scoreSettings;

        private GameService _gameService;

        private ScoreController _scoreController;
        
        private bool _isAnimating;
        private int _selectedX = -1;
        private int _selectedY = -1;

        #region Unity
        private void Awake()
        {
            _gameService = new GameService(_scoreSettings.CreateConfig());
            _boardView.TileClicked += OnTileClick;
        }

        private void OnDestroy()
        {
            _boardView.TileClicked -= OnTileClick;
            _scoreController?.Dispose();
        }

        private void Start()
        {
            List<List<Tile>> board = _gameService.StartGame(_boardWidth, _boardHeight);
            _scoreController = new ScoreController(_scoreView, _gameService.ScoreService);
            _boardView.CreateBoard(board);
        }
        #endregion

        private IEnumerator AnimateBoard(IEnumerable<BoardSequence> boardSequences, Action onComplete)
        {
            foreach (var boardSequence in boardSequences)
            {
                yield return _boardView.DestroyTiles(boardSequence.MatchedPosition).WaitForCompletion();
                yield return _boardView.CreateSpecialTile(boardSequence.AddedSpecialTiles).WaitForCompletion();
                yield return _boardView.MoveTiles(boardSequence.MovedTiles).WaitForCompletion();
                yield return _boardView.CreateTile(boardSequence.AddedTiles);
            }
            
            onComplete?.Invoke();
        }

        private void OnTileClick(int x, int y)
        {
            if (_isAnimating) return;

            if (_selectedX > -1 && _selectedY > -1)
            {
                if (Mathf.Abs(_selectedX - x) + Mathf.Abs(_selectedY - y) > 1)
                {
                    _selectedX = -1;
                    _selectedY = -1;
                }
                else
                {
                    _isAnimating = true;
                    _boardView.SwapTiles(_selectedX, _selectedY, x, y).onComplete += () =>
                    {
                        bool isValid = _gameService.IsValidMovement(_selectedX, _selectedY, x, y);
                        if (isValid)
                        {
                            List<BoardSequence> swapResult = _gameService.SwapTile(_selectedX, _selectedY, x, y);
                            StartCoroutine(AnimateBoard(swapResult, () => { _isAnimating = false; }));
                        }
                        else
                        {
                            _boardView.SwapTiles(x, y, _selectedX, _selectedY).onComplete += () => _isAnimating = false;
                        }
                        _selectedX = -1;
                        _selectedY = -1;
                    };
                }
            }
            else
            {
                _selectedX = x;
                _selectedY = y;
            }
        }
    }
}
