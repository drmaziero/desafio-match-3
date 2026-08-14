using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "LevelListSettings", menuName = "Gameplay/Level List Settings")]
    public class LevelListSettings : ScriptableObject
    {
        [field: SerializeField] private List<LevelSettings> allLevels;

        public List<LevelConfig> GetLevelConfigs()
        {
            return allLevels.Select(setting => setting.CreateLevel()).ToList();
        }
    }
}