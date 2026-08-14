using System;
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
            OnScoreChanged(0);
            _hudView.Init(targetLevel);
        }
        

        public void OnScoreChanged(int score)
        {
            _hudView.UpdateScore(score);
        }

        public void OnMovementChanged(int movementCounter)
        {
            _hudView.UpdateSwapTile(movementCounter);
        }
    }
}