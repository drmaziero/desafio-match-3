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
                if (HasConsumedMatches(complexMatch.GetBasicMatches()))
                    continue;
                
                Score += _complexMatchScore + complexMatch.GetSequencePosition().Count * _complexSequenceBonus;
                Consume(complexMatch.GetBasicMatches());
            }
        }

        private void CalcComposeMatchScore(List<ComposedMatch> composedMatches)
        {
            foreach (var composedMatch in composedMatches)
            {
                if (HasConsumedMatches(composedMatch.GetBasicMatches()))
                    continue;

                if (composedMatch.IsMatchL())
                    Score += _matchLScore + composedMatch.GetSequencePosition().Count * _matchLSequenceBonus;
                else
                    Score += _matchTScore + composedMatch.GetSequencePosition().Count * _matchTSequenceBonus;
                
                Consume(composedMatch.GetBasicMatches());
            }
        }

        private void CalcBasicMatchScore(List<BasicMatch> basicMatches)
        {
            foreach (var match in basicMatches)
            {
                string log = $"Score: {Score} ";
                if (IsConsumed(match)) 
                    continue;
                
                if (match.IsHorizontal())
                    Score += _horizontalMatchScore + match.GetSequencePosition().Count * _horizontalSequenceBonus;
                else
                    Score += _verticalMatchScore + match.GetSequencePosition().Count * _verticalSequenceBonus;
                
                _consumedMatches.Add(match);
                log += $" ->  {Score}";
                Debug.LogWarning(log);
            }
        }

        private void Consume(List<BasicMatch> matches)
        {
            foreach (var match in matches)
                _consumedMatches.Add(match);
        }

        private bool IsConsumed(BasicMatch match)
        {
            return _consumedMatches.Contains(match);
        }

        private bool HasConsumedMatches(List<BasicMatch> matches)
        {
            return matches.Any(IsConsumed);
        }
    }
}