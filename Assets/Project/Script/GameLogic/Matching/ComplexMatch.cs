using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Matching
{
    public class ComplexMatch
    {
        private ComposedMatch[] _composedMatches;

        public ComplexMatch(List<ComposedMatch> composedMatches)
        {
            _composedMatches = composedMatches.ToArray();
        }
        
        public List<Vector2Int> GetSequencePosition()
        {
            var sequence = new List<Vector2Int>();

            foreach (var composedMatch in _composedMatches)
                sequence.AddRange(composedMatch.GetSequencePosition());

            return sequence;
        }

        public List<BasicMatch> GetBasicMatches()
        {
            var basicMatches = new List<BasicMatch>();

            foreach (var composedMatch in _composedMatches)
                basicMatches.AddRange(composedMatch.BasicMatches);

            return basicMatches;
        }
    }
}