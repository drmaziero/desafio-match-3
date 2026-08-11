using System.Collections.Generic;
using System.Linq;
using GameLogic.Matching;

namespace GameLogic.Services
{
    public class ScoreService
    {
        public int Score { get; private set; } = 0;

        private readonly int _horizontalMatchScore = 0;
        private readonly int _verticalMatchScore = 0;
        private readonly int _matchLScore = 5;
        private readonly int _matchTScore = 10;
        private readonly int _complexMatchScore = 15;

        private readonly int _horizontalSequenceBonus = 1;
        private readonly int _verticalSequenceBonus = 1;
        private readonly int _matchLSequenceBonus = 1;
        private readonly int _matchTSequenceBonus = 1;
        private readonly int _complexSequenceBonus = 1;

        private List<BasicMatch> _consumedMatches;

        public void Init()
        {
            Score = 0;
            _consumedMatches = new List<BasicMatch>();
        }

        public void ComputeScore(List<ComplexMatch> complexMatches, List<ComposedMatch> composedMatches,
            List<BasicMatch> horizontalMatches, List<BasicMatch> verticalMatches)
        {
            _consumedMatches.Clear();
            CalcComplexMatchScore(complexMatches);
            CalcComposeMatchScore(composedMatches);

            List<BasicMatch> basicMatches = new List<BasicMatch>();
            basicMatches.AddRange(horizontalMatches);
            basicMatches.AddRange(verticalMatches);
            
            CalcBasicMatchScore(basicMatches);
        }

        private void CalcComplexMatchScore(List<ComplexMatch> complexMatches)
        {
            foreach (var complexMatch in complexMatches)
            {
                Score += _complexMatchScore + complexMatch.GetSequencePosition().Count * _complexSequenceBonus;
                foreach (var composeMatch in complexMatch.ComposedMatches)
                {
                    foreach (var match in composeMatch.BasicMatches)
                        _consumedMatches.Add(match);
                }
            }
        }

        private void CalcComposeMatchScore(List<ComposedMatch> composedMatches)
        {
            foreach (var composedMatch in composedMatches)
            {
                if (OnConsumedMatches(new List<BasicMatch>(composedMatch.BasicMatches)))
                    continue;

                if (composedMatch.IsMatchL())
                    Score += _matchLScore + composedMatch.GetSequencePosition().Count * _matchLSequenceBonus;
                else
                    Score += _matchTScore + composedMatch.GetSequencePosition().Count * _matchTSequenceBonus;
                
                foreach (var match in composedMatch.BasicMatches)
                    _consumedMatches.Add(match);
            }
        }

        private void CalcBasicMatchScore(List<BasicMatch> basicMatches)
        {
            foreach (var match in basicMatches)
            {
                if (!OnConsumedMatches(new List<BasicMatch>() { match })) 
                    continue;
                
                if (match.IsHorizontal())
                    Score += _horizontalMatchScore + match.GetSequencePosition().Count * _horizontalSequenceBonus;
                else
                    Score += _verticalMatchScore + match.GetSequencePosition().Count * _verticalSequenceBonus;
            }
        }

        private bool OnConsumedMatches(List<BasicMatch> matches)
        {
            return matches.Any(match => _consumedMatches.Contains(match));
        }
    }
}