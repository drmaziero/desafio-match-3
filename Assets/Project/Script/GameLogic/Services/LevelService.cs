using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Services
{
    public class LevelService
    {
        public event Action<int> MovementCountChanged; 
        private List<LevelConfig> _levelConfigs;
        private int _tileMovementCount;
        private int _currentLevel;
        private const string levelPlayerPrefKey = "Level";
        private int _currentScore;
        
        public LevelService(List<LevelConfig> levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public void Init()
        {
            if (!PlayerPrefs.HasKey(levelPlayerPrefKey))
                PlayerPrefs.SetInt(levelPlayerPrefKey,0);

            _currentLevel = PlayerPrefs.GetInt(levelPlayerPrefKey);
            _tileMovementCount = GetCurrentLevel().MaxSwapTile;
            _currentScore = 0;
            MovementCountChanged?.Invoke(_tileMovementCount);
        }

        public LevelConfig GetCurrentLevel()
        {
            return _levelConfigs[_currentLevel];
        }

        public void DecreaseMovementCount()
        {
            _tileMovementCount--;
            MovementCountChanged?.Invoke(_tileMovementCount);
        }

        public void UpdateScore(int score)
        {
            _currentScore = score;
        }

        
        public bool IsEndGame()
        {
            return !CanMoveTiles() || IsCompleteAllTargets();
        }

        private bool CanMoveTiles()
        {
            return _tileMovementCount > 0;
        }

        public bool IsCompleteAllTargets()
        {
            var currentLevel = GetCurrentLevel();

            var isCompleted = true;

            if (currentLevel.Target.HasTargetScore())
                isCompleted = isCompleted && _currentScore >= currentLevel.Target.Score;

            return isCompleted;
        }
    }
}