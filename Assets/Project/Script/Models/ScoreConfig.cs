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

        public ScoreConfig(int horizontalMatchScore, int verticalMatchScore, int matchLScore, int matchTScore,
            int complexMatchScore, int horizontalElementScore, int verticalElementScore, int matchLElementScore,
            int matchTElementScore, int complexElementScore)
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
        }
    }
}