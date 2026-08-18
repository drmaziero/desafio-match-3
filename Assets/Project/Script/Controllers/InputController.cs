using System;
using UnityEngine;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        public event Action<int, int, int, int> SwapRequested;
        
        private Vector2Int _dragFrom;
        private Vector2Int _dragTo;
        private bool _isDragRequested;
        private bool _isInputLocked;
        private static readonly Vector2Int InvalidPosition = new Vector2Int(-1, -1);

        private void Awake()
        {
            ResetPosition();
            _isDragRequested = false;
            _isInputLocked = false;
        }

        private void ResetPosition()
        {
            _dragFrom = InvalidPosition;
            _dragTo = InvalidPosition;
        }
        
        private void OnDragRequested(Vector2Int position)
        {
            if (_isInputLocked)
                return;
            
            if (_isDragRequested) 
                return;

            _dragFrom = position;
            _dragTo = InvalidPosition;
            
            _isDragRequested = true;
        }

        private void OnEnterTile(Vector2Int position)
        {
            if (_isInputLocked)
                return;
            
            if (!_isDragRequested)
                return;
            
            if (position == _dragFrom)
                return;
            
            _dragTo = position;
            TryToCompleteDrag();
        }

        private void TryToCompleteDrag()
        {
            if (_dragFrom == InvalidPosition || _dragTo == InvalidPosition)
                return;

            var distance = Mathf.Abs(_dragTo.x - _dragFrom.x) +
                           Mathf.Abs(_dragTo.y - _dragFrom.y);

            if (distance != 1)
            {
                _dragTo = InvalidPosition;
                return;
            }
            
            _isInputLocked = true;
            
            SwapRequested?.Invoke(_dragFrom.x, _dragFrom.y, _dragTo.x,
                _dragTo.y);
        }
        
        public void SetDragCompleted()
        {
            _isDragRequested = false;
            _isInputLocked = false;
            
            ResetPosition();
        }
        

        public void RegisterTileViewController(TileController tileController)
        {
            tileController.EnterOnTile += OnEnterTile;
            tileController.DragRequested += OnDragRequested;
        }
        
    }
}