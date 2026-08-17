using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class HudController : MonoBehaviour
    {
        [SerializeField]
        private HudView _hudView;

        public void Init(TargetLevel targetLevel)
        {
            _hudView.Init(targetLevel);
        }
        

        public void OnScoreChanged(int score)
        {
            _hudView.UpdateScore(score);
            _hudView.UpdateTargetScore(score);
        }

        public void OnMovementChanged(int movementCounter)
        {
            _hudView.UpdateSwapTile(movementCounter);
        }

        public void OnTileCounterChanged(TileType type, int newCounter)
        {
            _hudView.UpdateTileCounter(type, newCounter);
        }

        public void OnSpecialCounterChanged(SpecialTileType type, int newCounter)
        {
            _hudView.UpdateSpecialCounter(type, newCounter);
        }
    }
}