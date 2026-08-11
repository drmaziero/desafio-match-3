using GameLogic.Services;
using UnityEngine;
using Views;

namespace Controllers
{
    public class ScoreController
    {
        private ScoreView _scoreView;
        private ScoreService _scoreService;

        public ScoreController(ScoreView scoreView, ScoreService scoreService)
        {
            _scoreView = scoreView;
            _scoreService = scoreService;
        }

        public void UpdateScore()
        {
            _scoreView.UpdateScore(_scoreService.Score);
        }

    }
}