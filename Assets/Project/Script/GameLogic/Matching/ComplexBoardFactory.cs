using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.Matching
{
    public class ComplexBoardFactory
    {
        public BasicMatch[] BasicMatches { get; private set; }
        private ComposedBoardFactory[] _composedMatches;
        private HashSet<Vector2Int> _sequencePositions;

        public ComplexBoardFactory(List<ComposedBoardFactory> composedMatches)
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