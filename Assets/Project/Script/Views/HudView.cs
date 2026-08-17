using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class HudView : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI life;
        [field: SerializeField] private TextMeshProUGUI score;
        [field: SerializeField] private TextMeshProUGUI swapTile;
        [field: SerializeField] private TargetLevelView[] targetPool;
        [field: SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
        
        private Queue<TargetLevelView> _targetPoolQueue;
        private List<TargetLevelView> _allTargetViews;
        private bool _initialized;

        private void InitializeIfNeeded()
        {
            if (_initialized)
                return;
            
            _targetPoolQueue = new Queue<TargetLevelView>();
            _allTargetViews = new List<TargetLevelView>();

            _initialized = true;
        }

        public void Init(TargetLevel target)
        {
            InitializeIfNeeded();
            Reset();

            UpdateScore(0);
            
            if (target.HasTargetScore())
            {
                var poolObject = GetPool();
                poolObject.Init(TileType.None, SpecialTileType.None, target.Score);
                poolObject.gameObject.SetActive(true);
                _allTargetViews.Add(poolObject);
            }

            if (target.HasTargetType())
            {
                foreach (var tileTypeCounter in target.TypeCounter)
                {
                    var poolObject = GetPool();
                    poolObject.Init(tileTypeCounter.Type, SpecialTileType.None, tileTypeCounter.Count);
                    poolObject.gameObject.SetActive(true);
                    _allTargetViews.Add(poolObject);
                }
                    
            }

            if (target.HasTargetSpecial())
            {
                foreach (var specialTypeCounter in target.SpecialCounter)
                {
                    var poolObject = GetPool();
                    poolObject.Init(TileType.None, specialTypeCounter.Type, specialTypeCounter.Count);
                    poolObject.gameObject.SetActive(true);
                    _allTargetViews.Add(poolObject);
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.GetComponent<RectTransform>());
        }

        public void Reset()
        {
            _targetPoolQueue.Clear();
            _allTargetViews.Clear();

            foreach (var targetLevel in targetPool)
            {
                targetLevel.Reset();
                targetLevel.gameObject.SetActive(false);
                _targetPoolQueue.Enqueue(targetLevel);
            }
        }

        private TargetLevelView GetPool()
        {
            return _targetPoolQueue.Count > 0
                ? _targetPoolQueue.Dequeue()
                : throw new ArgumentOutOfRangeException("Not Elements on Target Level Pool");
        }
        
        public void UpdateLife(int lifeCount)
        {
            life.SetText($"{lifeCount}");
        }

        public void UpdateScore(int currentScore)
        {
            score.SetText($"{currentScore}");
        }

        public void UpdateSwapTile(int swapCount)
        {
            swapTile.SetText($"{swapCount}");
        }

        public void UpdateTargetScore(int newScore)
        {
            var view = _allTargetViews.FirstOrDefault(x=> x.IsScoreView());
            view?.UpdateCount(newScore);
        }

        public void UpdateTileCounter(TileType type, int newCounter)
        {
            var view = _allTargetViews.FirstOrDefault(x => x.IsTypeView(type));
            view?.UpdateCount(newCounter);
        }

        public void UpdateSpecialCounter(SpecialTileType type, int newCounter)
        {
            var view = _allTargetViews.FirstOrDefault(x => x.IsSpecialTypeView(type));
            view?.UpdateCount(newCounter);
        }
    }
}