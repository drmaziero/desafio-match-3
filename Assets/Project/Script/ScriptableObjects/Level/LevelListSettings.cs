using System.Collections.Generic;
using Models;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "LevelListSettings", menuName = "Gameplay/Level List Settings")]
    public class LevelListSettings : ScriptableObject
    {
        [field: SerializeField] private List<LevelSettings> allLevels;

        public LevelConfig GetLevel(int levelIndex)
        {
            return allLevels[levelIndex].CreateLevel();
        }
    }
}