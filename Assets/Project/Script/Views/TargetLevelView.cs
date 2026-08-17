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
        private bool _initialized = false;
        private TileType _type;
        private SpecialTileType _specialType;
        
        
        public void Init(TileType type, SpecialTileType specialTileType, int count)
        {
            _tileTypeDictionary = new Dictionary<TileType, Color>();
            _specialTypeDictionary = new Dictionary<SpecialTileType, GameObject>();
            
            foreach (var typeIcon in typeIcons)
                _tileTypeDictionary.Add(typeIcon.type, typeIcon.color);
            
            foreach (var specialIcon in specialIcons)
                _specialTypeDictionary.Add(specialIcon.type, specialIcon.icon);

            _initialized = true;
            
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
            if (!_initialized)
                return;

            _type = TileType.None;
            _specialType = SpecialTileType.None;
            scoreIcon.SetActive(false);
            tileIcon.SetActive(false);
            foreach (var keyValuePair in _specialTypeDictionary)
                keyValuePair.Value.SetActive(false);

            _initialized = false;
        }

        public void UpdateCount(int value)
        {
            counter.SetText($"{value}/{_targetCount}");
        }

        public bool IsScoreView()
        {
            return _type == TileType.None && _specialType == SpecialTileType.None;
        }

        public bool IsTypeView()
        {
            return _type != TileType.None;
        }

        public bool IsSpecialTypeView()
        {
            return _specialType != SpecialTileType.None;
        }

        public void UpdateTileCounter(TileType type, int value)
        {
            if (_type == type)
                UpdateCount(value);
        }

        public void UpdateSpecialCounter(SpecialTileType specialType, int value)
        {
            if (_specialType == specialType)
                UpdateCount(value);
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