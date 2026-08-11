using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.Matching
{
    public class ComplexMatch
    {
        public BasicMatch[] BasicMatches { get; private set; }
        private ComposedMatch[] _composedMatches;
        private HashSet<Vector2Int> _sequencePositions;

        public ComplexMatch(List<ComposedMatch> composedMatches)
        {
            _composedMatches = composedMatches.ToArray();
            BasicMatches = _composedMatches.SelectMany(composedMatch => composedMatch.BasicMatches).Distinct().ToArray();
            _sequencePositions = BasicMatches.SelectMany(match => match.GetSequencePosition()).ToHashSet();
        }
        
        public IEnumerable<Vector2Int> GetSequencePosition()
        {
            return _sequencePositions;
        }
        
        public int SequenceCount()
        {
            return _sequencePositions.Count;
        }
    }
}