using System.Collections.Generic;
using System.Linq;
using GameLogic.Matching;
using UnityEngine;

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

        private HashSet<BasicMatch> _consumedMatches;

        public void Init()
        {
            Score = 0;
            _consumedMatches = new HashSet<BasicMatch>();
        }

        public void ComputeScore(
            IEnumerable<ComplexMatch> complexMatches, 
            IEnumerable<ComposedMatch> composedMatches,
            IEnumerable<BasicMatch> horizontalMatches, 
            IEnumerable<BasicMatch> verticalMatches)
        {
            _consumedMatches.Clear();
            CalcComplexMatchScore(complexMatches);
            CalcComposeMatchScore(composedMatches);
            CalcBasicMatchScore(horizontalMatches.Concat(verticalMatches));
        }

        private void CalcComplexMatchScore(IEnumerable<ComplexMatch> complexMatches)
        {
            foreach (var complexMatch in complexMatches)
            {
                if (HasConsumedMatches(complexMatch.BasicMatches))
                    continue;
                
                Score += _complexMatchScore + complexMatch.SequenceCount() * _complexSequenceBonus;
                Consume(complexMatch.BasicMatches);
            }
        }

        private void CalcComposeMatchScore(IEnumerable<ComposedMatch> composedMatches)
        {
            foreach (var composedMatch in composedMatches)
            {
                if (HasConsumedMatches(composedMatch.BasicMatches))
                    continue;

                if (composedMatch.IsMatchL())
                    Score += _matchLScore + composedMatch.SequenceCount() * _matchLSequenceBonus;
                else
                    Score += _matchTScore + composedMatch.SequenceCount() * _matchTSequenceBonus;
                
                Consume(composedMatch.BasicMatches);
            }
        }

        private void CalcBasicMatchScore(IEnumerable<BasicMatch> basicMatches)
        {
            foreach (var match in basicMatches)
            {
                if (IsConsumed(match)) 
                    continue;
                
                if (match.IsHorizontal())
                    Score += _horizontalMatchScore + match.Count * _horizontalSequenceBonus;
                else
                    Score += _verticalMatchScore + match.Count * _verticalSequenceBonus;
                
                _consumedMatches.Add(match);
            }
        }

        private void Consume(IEnumerable<BasicMatch> matches)
        {
            foreach (var match in matches)
                _consumedMatches.Add(match);
        }

        private bool IsConsumed(BasicMatch match)
        {
            return _consumedMatches.Contains(match);
        }

        private bool HasConsumedMatches(IEnumerable<BasicMatch> matches)
        {
            return matches.Any(IsConsumed);
        }
    }
}