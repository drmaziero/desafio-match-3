using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Matching;
using Models;

namespace GameLogic.Services
{
    public class ScoreService
    {
        public event Action<int> ScoreChanged;
        private int _score;
        private ScoreConfig _config;

        private HashSet<BasicMatch> _consumedMatches;

        public ScoreService(ScoreConfig config)
        {
            _score = 0;
            _config = config;
            _consumedMatches = new HashSet<BasicMatch>();
        }

        public void Init()
        {
            _score = 0;
            _consumedMatches.Clear();
        }

        public void ComputeScore(
            IEnumerable<ComplexMatch> complexMatches, 
            IEnumerable<ComposedMatch> composedMatches,
            IEnumerable<BasicMatch> horizontalMatches, 
            IEnumerable<BasicMatch> verticalMatches,
            int cascadeCounter)
        {
            _consumedMatches.Clear();

            int newScore = 0;
            newScore += CalcComplexMatchScore(complexMatches);
            newScore += CalcComposeMatchScore(composedMatches);
            newScore += CalcBasicMatchScore(horizontalMatches.Concat(verticalMatches));

            int multiplier = _config.GetCascadeMultiplier(cascadeCounter);
            AddScore(newScore * multiplier);
        }

        private void AddScore(int newScore)
        {
            _score += newScore;
            ScoreChanged?.Invoke(_score);
        }
        
        private int CalcComplexMatchScore(IEnumerable<ComplexMatch> complexMatches)
        {
            int score = 0;
            foreach (var complexMatch in complexMatches)
            {
                if (HasConsumedMatches(complexMatch.BasicMatches))
                    continue;
                
                score += _config.ComplexMatchScore + complexMatch.SequenceCount() * _config.ComplexElementScore;
                Consume(complexMatch.BasicMatches);
            }

            return score;
        }

        private int CalcComposeMatchScore(IEnumerable<ComposedMatch> composedMatches)
        {
            int score = 0;
            
            foreach (var composedMatch in composedMatches)
            {
                if (HasConsumedMatches(composedMatch.BasicMatches))
                    continue;

                if (composedMatch.IsMatchL())
                    score += _config.MatchLScore + composedMatch.SequenceCount() * _config.MatchLElementScore;
                else
                    score += _config.MatchTScore + composedMatch.SequenceCount() * _config.MatchTElementScore;
                
                Consume(composedMatch.BasicMatches);
            }

            return score;
        }

        private int CalcBasicMatchScore(IEnumerable<BasicMatch> basicMatches)
        {
            int score = 0;
            foreach (var match in basicMatches)
            {
                if (IsConsumed(match)) 
                    continue;
                
                if (match.IsHorizontal())
                    score += _config.HorizontalMatchScore + match.Count * _config.HorizontalElementScore;
                else
                    score += _config.VerticalMatchScore + match.Count * _config.VerticalElementScore;
                
                _consumedMatches.Add(match);
            }

            return score;
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