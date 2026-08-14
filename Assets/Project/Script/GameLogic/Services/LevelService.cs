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
        private int _maxTileMovements;
        private int _currentLevel;
        private const string levelPlayerPrefKey = "Level";
        
        public LevelService(List<LevelConfig> levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public void Init()
        {
            if (!PlayerPrefs.HasKey(levelPlayerPrefKey))
                PlayerPrefs.SetInt(levelPlayerPrefKey,0);

            _currentLevel = PlayerPrefs.GetInt(levelPlayerPrefKey);
            _maxTileMovements = GetCurrentLevel().MaxSwapTile;
            MovementCountChanged?.Invoke(_maxTileMovements);
        }

        public LevelConfig GetCurrentLevel()
        {
            return _levelConfigs[_currentLevel];
        }

        public void DecreaseMovementCount()
        {
            _maxTileMovements--;
            MovementCountChanged?.Invoke(_maxTileMovements);
        }

        /*
        public bool VerifyEndGame()
        {
            
            bool isEndGame;


        }

        private bool ISCompleteAllTargets(int score)
        {
            var currentLevel = GetCurrentLevel();

            var isCompleted = true;

            if (currentLevel.Target.HasTargetScore())
                isCompleted = isCompleted && score >= currentLevel.Target.Score;

            return isCompleted;
        }

        public bool IsWin()
        {
            
        }
        */
    }
}