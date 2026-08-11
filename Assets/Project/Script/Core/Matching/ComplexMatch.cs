using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class ComplexMatch
    {
        public ComposedMatch[] ComposedMatches { get; private set; }

        public ComplexMatch(List<ComposedMatch> composedMatches)
        {
            ComposedMatches = composedMatches.ToArray();
        }
        
        public List<Vector2Int> GetSequencePosition()
        {
            var sequence = new List<Vector2Int>();

            foreach (var composedMatch in ComposedMatches)
                sequence.AddRange(composedMatch.GetSequencePosition());

            return sequence;
        }
    }
}