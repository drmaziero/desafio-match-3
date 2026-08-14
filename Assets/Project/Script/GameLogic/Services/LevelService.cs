using System;
using System.Collections.Generic;
using GameLogic.Matching;
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

        private Dictionary<TileType, int> _tileCounter;
        private Dictionary<SpecialTileType, int> _specialCounter;
        
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
            _tileCounter = new Dictionary<TileType, int>();
            _specialCounter = new Dictionary<SpecialTileType, int>();
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

        public void UpgradeLevel()
        {
            _currentLevel++;
            PlayerPrefs.SetInt(levelPlayerPrefKey,_currentLevel);
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

            if (currentLevel.Target.HasTargetType())
            {
                foreach (var typeCounter in currentLevel.Target.TypeCounter)
                {
                    if (!_tileCounter.ContainsKey(typeCounter.Type))
                    {
                        isCompleted = false;
                        continue;
                    }

                    isCompleted = isCompleted && _tileCounter[typeCounter.Type] >= typeCounter.Count;
                }
            }

            if (currentLevel.Target.HasTargetSpecial())
            {
                foreach (var specialCounter in currentLevel.Target.SpecialCounter)
                {
                    if (!_specialCounter.ContainsKey(specialCounter.Type))
                    {
                        isCompleted = false;
                        continue;
                    }

                    isCompleted = isCompleted && _specialCounter[specialCounter.Type] >= specialCounter.Count;
                }
            }

            return isCompleted;
        }

        public void ComputeMatches(HashSet<Vector2Int> matches, List<List<Tile>> board)
        {
            foreach (var match in matches)
            {
                if (board[match.y][match.x].SpecialType != SpecialTileType.None)
                {
                    if (!_specialCounter.TryAdd(board[match.y][match.x].SpecialType, 1))
                        _specialCounter[board[match.y][match.x].SpecialType]++;

                    continue;
                }

                if (!_tileCounter.TryAdd(board[match.y][match.x].Type, 1))
                    _tileCounter[board[match.y][match.x].Type]++;
            }
        }
    }
}