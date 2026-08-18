using System;
using UnityEngine;

namespace Controllers
{
    public class TileInputController : MonoBehaviour
    {
        public event Action<int, int, int, int> SwapRequested;
        
        private Vector2Int _enterTilePosition;
        private Vector2Int _exitTilePosition;
        private Vector2Int _currentTilePosition;
        private bool _isDragRequested;
        private bool _waitingTargetToDrag;

        private void Awake()
        {
            ResetPosition();
            _isDragRequested = false;
            _waitingTargetToDrag = false;
        }

        public void ResetPosition()
        {
            SetEnterTilePosition(new Vector2Int(-1, -1));
            SetExitTilePosition(new Vector2Int(-1, -1));
        }

        private void SetEnterTilePosition(Vector2Int position)
        {
            _enterTilePosition = position;
            TryToCompleteDrag();
        }

        private void TryToCompleteDrag()
        {
            if (!_waitingTargetToDrag) return;
            _waitingTargetToDrag = false;

            if (_enterTilePosition is { x: -1, y: -1 })
            {
                SetDragCompleted();
                return;
            }

            var horizontalDiff = Mathf.Abs(_exitTilePosition.x - _enterTilePosition.x);
            var verticalDiff = Mathf.Abs(_exitTilePosition.y - _enterTilePosition.y);

            if (horizontalDiff > 0 && verticalDiff > 0)
            {
                SetDragCompleted();
                return;
            }
            
            SwapRequested?.Invoke(_enterTilePosition.x, _enterTilePosition.y, _exitTilePosition.x,
                _exitTilePosition.y);
        }

        private void SetExitTilePosition(Vector2Int position)
        {
            _exitTilePosition = position;
            TryWaitingTargetDrag();
        }

        private void TryWaitingTargetDrag()
        {
            if (!_isDragRequested) return;
            if (_exitTilePosition.x == _currentTilePosition.x && _exitTilePosition.y == _currentTilePosition.y)
                _waitingTargetToDrag = true;
        }

        public void RegisterTileViewController(TileViewController tileViewController)
        {
            tileViewController.EnterOnTile += SetEnterTilePosition;
            tileViewController.ExitOnTile += SetExitTilePosition;
            tileViewController.DragRequested += OnDragRequested;
        }

        private void OnDragRequested()
        {
            if (!_isDragRequested)
            {
                _currentTilePosition = _enterTilePosition;
                _isDragRequested = true;
                _waitingTargetToDrag = false;
            }
        }

        public void SetDragCompleted()
        {
            _isDragRequested = false;
        }
    }
}