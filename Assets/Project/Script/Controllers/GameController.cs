using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public event Action<int, int, int, int> SwapRequested;
        public event Action TurnCompleted; 
        
        [Header("Views")]
        [SerializeField] private BoardView _boardView;
        [SerializeField] private HudView _hudView;
        
        private HudController _hudController;
        
        private bool _isAnimating;
        private int _selectedX = -1;
        private int _selectedY = -1;

        #region Unity
        private void Awake()
        {
            _boardView.TileClicked += OnTileClick;
        }

        private void OnDestroy()
        {
            _boardView.TileClicked -= OnTileClick;
        }

        public void Init(List<List<Tile>> board)
        {;
            _boardView.CreateBoard(board);
        }

        public void Reset()
        {
            _boardView.ClearBoard();
        }
        
        private void OnDisable()
        {
            Reset();
        }

        #endregion

        public void AnimateInvalidSwap(Vector2Int from, Vector2Int to)
        {
            StartCoroutine(AnimateInvalidSwapCoroutine(from, to));
        }

        private IEnumerator AnimateInvalidSwapCoroutine(Vector2Int from, Vector2Int to)
        {
            yield return _boardView.SwapTiles(from.x, from.y, to.x, to.y).WaitForCompletion();
            yield return _boardView.SwapTiles(to.x, to.y, from.x, from.y).WaitForCompletion();

            _isAnimating = false;
        }

        public void AnimateValidSwap(Vector2Int from, Vector2Int to, List<BoardSequence> boardSequences)
        {
            StartCoroutine(AnimateValidSwapCoroutine(from, to, boardSequences));
        }

        private IEnumerator AnimateValidSwapCoroutine(Vector2Int from, Vector2Int to,
            IEnumerable<BoardSequence> boardSequences)
        {
            yield return _boardView.SwapTiles(from.x, from.y, to.x, to.y).WaitForCompletion();

            foreach (var boardSequence in boardSequences)
            {
                yield return _boardView.ClearTiles(boardSequence.MatchedPosition).WaitForCompletion();
                yield return _boardView.CreateSpecialTile(boardSequence.AddedSpecialTiles).WaitForCompletion();
                yield return _boardView.MoveTiles(boardSequence.MovedTiles).WaitForCompletion();
                yield return _boardView.RefillTiles(boardSequence.AddedTiles).WaitForCompletion();
            }

            _isAnimating = false;
            TurnCompleted?.Invoke();
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
                    SwapRequested?.Invoke(_selectedX, _selectedY, x, y);
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
