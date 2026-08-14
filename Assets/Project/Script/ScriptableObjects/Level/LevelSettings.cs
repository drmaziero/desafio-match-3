using System.Collections.Generic;
using Models;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Gameplay/Level Settings")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private Vector2Int boardSize;
        [SerializeField] private int maxSwapTiles;
        [SerializeField] private List<TileType> typeList;
        [SerializeField] private TargetLevelSettings targetSettings;

        public LevelConfig CreateLevel()
        {
            var tileCounterList = new List<TileTypeCounter>();
            foreach (var tileSetting in targetSettings.tileSettings)
                tileCounterList.Add(new TileTypeCounter(tileSetting.type, tileSetting.count));

            var specialCounterList = new List<SpecialTypeCounter>(); 
            foreach (var specialSetting in targetSettings.specialSettings)
                specialCounterList.Add(new SpecialTypeCounter(specialSetting.type, specialSetting.count));

            var targetLevel = new TargetLevel(targetSettings.score, tileCounterList, specialCounterList);

            return new LevelConfig(boardSize, targetLevel, maxSwapTiles, typeList);
        }
    }
}