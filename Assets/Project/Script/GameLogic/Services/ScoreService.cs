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
            IEnumerable<BasicMatch> verticalMatches)
        {
            _consumedMatches.Clear();
            CalcComplexMatchScore(complexMatches);
            CalcComposeMatchScore(composedMatches);
            CalcBasicMatchScore(horizontalMatches.Concat(verticalMatches));
        }

        private void AddScore(int newScore)
        {
            _score += newScore;
            ScoreChanged?.Invoke(_score);
        }
        
        private void CalcComplexMatchScore(IEnumerable<ComplexMatch> complexMatches)
        {
            foreach (var complexMatch in complexMatches)
            {
                if (HasConsumedMatches(complexMatch.BasicMatches))
                    continue;
                
                AddScore(_config.ComplexMatchScore + complexMatch.SequenceCount() * _config.ComplexElementScore);
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
                   AddScore(_config.MatchLScore + composedMatch.SequenceCount() * _config.MatchLElementScore);
                else
                    AddScore(_config.MatchTScore + composedMatch.SequenceCount() * _config.MatchTElementScore);
                
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
                    AddScore(_config.HorizontalMatchScore + match.Count * _config.HorizontalElementScore);
                else
                    AddScore(_config.VerticalMatchScore + match.Count * _config.VerticalElementScore);
                
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