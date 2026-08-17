using System;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TargetLevelView : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI counter;

        [field: SerializeField] private GameObject scoreIcon;
        [field: SerializeField] private GameObject tileIcon;
        [field: SerializeField] private Image TileLiquidIcon;
        [field: SerializeField] private List<TargetLevelViewTypeIcons> typeIcons;
        [field: SerializeField] private List<TargetLevelViewSpecialIcons> specialIcons;
        [field: SerializeField] private HorizontalLayoutGroup HorizontalLayoutGroup;

        private Dictionary<TileType, Color> _tileTypeDictionary;
        private Dictionary<SpecialTileType, GameObject> _specialTypeDictionary;
        private int _targetCount;
        private TileType _type;
        private SpecialTileType _specialType;
        private bool _initialized;

        private void InitializeIfNeeded()
        {
            if (_initialized)
                return;
            
            _tileTypeDictionary = new Dictionary<TileType, Color>();
            _specialTypeDictionary = new Dictionary<SpecialTileType, GameObject>();
            
            foreach (var typeIcon in typeIcons)
                _tileTypeDictionary.Add(typeIcon.type, typeIcon.color);
            
            foreach (var specialIcon in specialIcons)
                _specialTypeDictionary.Add(specialIcon.type, specialIcon.icon);

            _initialized = true;
        }

        public void Init(TileType type, SpecialTileType specialTileType, int count)
        {
            InitializeIfNeeded();
            Reset();
            
            _type = type;
            _specialType = specialTileType;
            _targetCount = count;
            if (type == TileType.None && specialTileType == SpecialTileType.None)
                scoreIcon.SetActive(true);
            else
            {
                if (type != TileType.None)
                {
                    tileIcon.SetActive(true);
                    TileLiquidIcon.color = _tileTypeDictionary[type];
                }
                else
                {
                    _specialTypeDictionary[specialTileType].SetActive(true);   
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(HorizontalLayoutGroup.GetComponent<RectTransform>());
            UpdateCount(0);
        }

        public void Reset()
        {
            _type = TileType.None;
            _specialType = SpecialTileType.None;
            scoreIcon.SetActive(false);
            tileIcon.SetActive(false);

            if (_specialTypeDictionary == null) return;
            foreach (var keyValuePair in _specialTypeDictionary)
                keyValuePair.Value.SetActive(false);
        }

        public void UpdateCount(int value)
        {
            counter.SetText($"{value}/{_targetCount}");
        }

        public bool IsScoreView()
        {
            return _type == TileType.None && _specialType == SpecialTileType.None;
        }

        public bool IsTypeView(TileType type)
        {
            return _type != type;
        }

        public bool IsSpecialTypeView(SpecialTileType type)
        {
            return _specialType != type;
        }
    }

    [Serializable]
    public class TargetLevelViewTypeIcons
    {
        public TileType type;
        public Color color;
    }

    [Serializable]
    public class TargetLevelViewSpecialIcons
    {
        public SpecialTileType type;
        public GameObject icon;
    }
}