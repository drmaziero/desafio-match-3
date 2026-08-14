using GameLogic.Services;
using Views;

namespace Controllers
{
    public class HudController
    {
        private HudView _hudView;
        private ScoreService _scoreService;
        private LevelService _levelService;

        public HudController(HudView hudView, ScoreService scoreService, LevelService levelService)
        {
            _hudView = hudView;
            _scoreService = scoreService;
            _levelService = levelService;
            
            _scoreService.ScoreChanged += OnScoreChanged;
            OnScoreChanged(0);
            _hudView.Init(_levelService.GetCurrentLevel().Target);
        }
        

        private void OnScoreChanged(int score)
        {
            _hudView.UpdateScore(score);
        }

        public void Dispose()
        {
            _scoreService.ScoreChanged -= OnScoreChanged;
        }

    }
}