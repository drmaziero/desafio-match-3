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
        private readonly MatchConsumptionTracker _consumptionTracker;

        public ScoreService(ScoreConfig config)
        {
            _score = 0;
            _config = config;
            _consumptionTracker = new MatchConsumptionTracker();
        }

        public void Init()
        {
            _score = 0;
            _consumptionTracker.Clear();
        }

        public void ComputeScore(DetectedMatches detectedMatches ,int cascadeCounter)
        {
            _consumptionTracker.Clear();

            int newScore = 0;
            
            if (detectedMatches.HasComplexMatches)
                newScore += CalcComplexMatchScore(detectedMatches.ComplexMatches);
            
            if (detectedMatches.HasComposedMatches)
                newScore += CalcComposeMatchScore(detectedMatches.ComposedMatches);
            
            if (detectedMatches.HasBasicMatches)
                newScore += CalcBasicMatchScore(detectedMatches.BasicMatches);

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
                if (_consumptionTracker.HasConsumedMatches(complexMatch.BasicMatches))
                    continue;
                
                score += _config.ComplexMatchScore + complexMatch.SequenceCount() * _config.ComplexElementScore;
                _consumptionTracker.Consume(complexMatch.BasicMatches);
            }

            return score;
        }

        private int CalcComposeMatchScore(IEnumerable<ComposedMatch> composedMatches)
        {
            int score = 0;
            
            foreach (var composedMatch in composedMatches)
            {
                if (_consumptionTracker.HasConsumedMatches(composedMatch.BasicMatches))
                    continue;

                if (composedMatch.IsMatchL())
                    score += _config.MatchLScore + composedMatch.SequenceCount() * _config.MatchLElementScore;
                else
                    score += _config.MatchTScore + composedMatch.SequenceCount() * _config.MatchTElementScore;
                
                _consumptionTracker.Consume(composedMatch.BasicMatches);
            }

            return score;
        }

        private int CalcBasicMatchScore(IEnumerable<BasicMatch> basicMatches)
        {
            int score = 0;
            foreach (var match in basicMatches)
            {
                if (_consumptionTracker.IsConsumed(match)) 
                    continue;
                
                if (match.IsHorizontal())
                    score += _config.HorizontalMatchScore + match.Count * _config.HorizontalElementScore;
                else
                    score += _config.VerticalMatchScore + match.Count * _config.VerticalElementScore;
                
                _consumptionTracker.Consume(match);
            }

            return score;
        }
    }
}