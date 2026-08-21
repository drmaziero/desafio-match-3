using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class ScoreConfig
    {
        public int HorizontalMatchScore { get; private set; }
        public int VerticalMatchScore { get; private set; }
        public int MatchLScore { get; private set; }
        public int MatchTScore { get; private set; }
        public int ComplexMatchScore { get; private set; }
        
        public int HorizontalElementScore { get; private set; }
        public int VerticalElementScore { get; private set; }
        public int MatchLElementScore { get; private set; }
        public int MatchTElementScore { get; private set; }
        public int ComplexElementScore { get; private set; }
        
        public Dictionary<SpecialTileType, int> SpecialScore { get; private set; }
        public Dictionary<SpecialTileType, int> SpecialElementScore { get; private set; }

        public Dictionary<int, int> _cascadeMultiplier;

        public ScoreConfig(int horizontalMatchScore, int verticalMatchScore, int matchLScore, int matchTScore,
            int complexMatchScore, int horizontalElementScore, int verticalElementScore, int matchLElementScore,
            int matchTElementScore, int complexElementScore, Dictionary<int, int> cascadeMultiplier,
            Dictionary<SpecialTileType, int> specialScore, Dictionary<SpecialTileType, int> specialElementScore)
        {
            HorizontalMatchScore = horizontalMatchScore;
            VerticalMatchScore = verticalMatchScore;
            MatchLScore = matchLScore;
            MatchTScore = matchTScore;
            ComplexMatchScore = complexMatchScore;
            HorizontalElementScore = horizontalElementScore;
            VerticalElementScore = verticalElementScore;
            MatchLElementScore = matchLElementScore;
            MatchTElementScore = matchTElementScore;
            ComplexElementScore = complexElementScore;
            _cascadeMultiplier = cascadeMultiplier;
            SpecialScore = specialScore;
            SpecialElementScore = specialElementScore;
        }

        public int GetCascadeMultiplier(int cascadeIndex)
        {
            return cascadeIndex >= _cascadeMultiplier.Count ? _cascadeMultiplier.Last().Value : _cascadeMultiplier[cascadeIndex];
        }

        public int GetSpecialScore(SpecialTileType type)
        {
            return SpecialScore.ContainsKey(type) ? SpecialScore[type] : 0;
        }
        
        public int GetSpecialScoreElement(SpecialTileType type)
        {
            return SpecialElementScore.ContainsKey(type) ? SpecialScore[type] : 0;
        }
    }
}