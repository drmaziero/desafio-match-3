using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class TileInputController : MonoBehaviour
    {
        [SerializeField] private Text EnterTileLabel;
        [SerializeField] private Text ExitTileLabel;
        
        private Vector2Int _enterTilePosition;
        private Vector2Int _exitTilePosition;

        private void Awake()
        {
            ResetPosition();
        }

        public void ResetPosition()
        {
            _enterTilePosition = new Vector2Int(-1, -1);
            _exitTilePosition = new Vector2Int(-1, -1);
            
            EnterTileLabel.text = $"{_enterTilePosition.x}, {_enterTilePosition.y}";
            ExitTileLabel.text = $"{_exitTilePosition.x}, {_exitTilePosition.y}";
        }

        private void SetEnterTilePosition(Vector2Int position)
        {
            _enterTilePosition = position;
            EnterTileLabel.text = $"{_enterTilePosition.x}, {_enterTilePosition.y}";
        } 
        private void SetExitTilePosition(Vector2Int position)
        {
            _exitTilePosition = position;
            ExitTileLabel.text = $"{_exitTilePosition.x}, {_exitTilePosition.y}";
        }
        
        public void RegisterTileViewController(TileViewController tileViewController)
        {
            tileViewController.EnterOnTile += SetEnterTilePosition;
            tileViewController.ExitOnTile += SetExitTilePosition;
        }
    }
}