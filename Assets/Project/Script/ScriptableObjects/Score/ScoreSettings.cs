using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace ScriptableObjects.Score
{
    [CreateAssetMenu(fileName = "ScoreSettings", menuName = "Gameplay/Score Settings")]
    public class ScoreSettings : ScriptableObject
    {
        [Header("Match")]
        [field: SerializeField]
        private int _horizontalMatchScore = 0;
        [field: SerializeField]
        private int _verticalMatchScore = 0;
        [field: SerializeField]
        private int _matchLScore = 5;
        [field: SerializeField]
        private int _matchTScore = 10;
        [field: SerializeField]
        private int _complexMatchScore = 15;

        [Header("Elements on Match")]
        [field: SerializeField]
        private int _horizontalElementScore = 1;
        [field: SerializeField]
        private int _verticalElementScore = 1;
        [field: SerializeField]
        private int _matchLElementScore = 1;
        [field: SerializeField]
        private int _matchTElementScore = 1;
        [field: SerializeField]
        private int _complexElementScore = 1;

        [Header("Special")] 
        [field: SerializeField]
        private List<SpecialTypeScore> _specialScore;

        [Header("Elements On Match")] 
        [field: SerializeField]
        private List<SpecialTypeScore> _specialElementScore;
        
        [Header("Cascate")] 
        [SerializeField] 
        private List<CascadeMultiplierEntry> _cascadeMultiplierEntries;

        public ScoreConfig CreateConfig()
        {
            return new ScoreConfig(_horizontalMatchScore, _verticalMatchScore, _matchLScore, _matchTScore,
                _complexMatchScore, _horizontalElementScore, _verticalElementScore, _matchLElementScore,
                _matchTElementScore, _complexElementScore,
                _cascadeMultiplierEntries.ToDictionary(x => x.cascadeCount, x => x.multiplier),
                _specialScore.ToDictionary(specialTypeScore => specialTypeScore.type, specialTypeScore => specialTypeScore.score),
                _specialElementScore.ToDictionary(specialTypeScore => specialTypeScore.type, specialTypeScore => specialTypeScore.score));
        }
    }

    [Serializable]
    public class SpecialTypeScore
    {
        public SpecialTileType type;
        public int score;
    }
}