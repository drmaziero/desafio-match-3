using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

namespace GameLogic.Matching
{
    public class ComposedMatch
    {
        public BasicMatch[] BasicMatches { get; private set; }
        public Vector2Int IntersectionPoint { get; private set; }
        
        private HashSet<Vector2Int> _sequencePositions;

        public ComposedMatch(Vector2Int intersectionPoint, List<BasicMatch> basicMatches)
        {
            IntersectionPoint = intersectionPoint;
            BasicMatches = basicMatches.ToArray();
            _sequencePositions = BasicMatches.SelectMany(match => match.GetSequencePosition()).ToHashSet();
        }

        public bool IsMatchT()
        {
            foreach (var basicMatch in BasicMatches)
            {
                if (!basicMatch.HasCentralPoint()) continue;
                if (basicMatch.GetCentralPoint().Equals(IntersectionPoint))
                    return true;
            }

            return false;
        }

        public bool IsMatchL()
        {
            return !IsMatchT();
        }

        public bool HasIntersection(ComposedMatch otherMatch)
        {
            foreach (var basicMatch in BasicMatches)
            {
                foreach (var otherBasicMatch in otherMatch.BasicMatches)
                {
                    if (basicMatch.Equals(otherBasicMatch))
                        return true;
                }
            }

            return false;
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