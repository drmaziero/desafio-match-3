using System;
using GameLogic.Services;
using Views;

namespace Controllers
{
    public class ScoreController : IDisposable
    {
        private ScoreView _scoreView;
        private ScoreService _scoreService;

        public ScoreController(ScoreView scoreView, ScoreService scoreService)
        {
            _scoreView = scoreView;
            _scoreService = scoreService;

            _scoreService.ScoreChanged += OnScoreChanged;
            OnScoreChanged(0);
        }
        

        private void OnScoreChanged(int score)
        {
            _scoreView.UpdateScore(score);
        }

        public void Dispose()
        {
            _scoreService.ScoreChanged -= OnScoreChanged;
        }
    }
}