using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Services
{
    public class LevelService
    {
        public event Action<int> MovementCountChanged;
        public event Action<TileType, int> TileCountChanged;
        public event Action<SpecialTileType, int> SpecialCountChanged; 
        
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
                var specialType = board[match.y][match.x].SpecialType;
                if (specialType != SpecialTileType.None)
                {
                    if (_specialCounter.TryAdd(specialType, 1))
                        SpecialCountChanged?.Invoke(specialType,1);
                    else
                    {
                        _specialCounter[specialType]++;
                        SpecialCountChanged?.Invoke(specialType,_specialCounter[specialType]);
                    }
                    continue;
                }


                var curType = board[match.y][match.x].Type;
                if (_tileCounter.TryAdd(curType, 1))
                    TileCountChanged?.Invoke(curType,1);
                else
                {
                    _tileCounter[curType]++;
                    TileCountChanged?.Invoke(curType,_tileCounter[curType]);
                }
            }
        }
    }
}