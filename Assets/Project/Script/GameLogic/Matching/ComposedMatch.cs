using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Matching
{
    public class ComposedMatch
    {
        private Vector2Int _intersectionPoint;
        public BasicMatch[] BasicMatches { get; private set; }

        public ComposedMatch(Vector2Int intersectionPoint, List<BasicMatch> basicMatches)
        {
            _intersectionPoint = intersectionPoint;
            BasicMatches = basicMatches.ToArray();
        }

        public bool IsMatchT()
        {
            foreach (var basicMatch in BasicMatches)
            {
                if (!basicMatch.HasCentralPoint()) continue;
                if (basicMatch.GetCentralPoint().Equals(_intersectionPoint))
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

        public List<Vector2Int> GetSequencePosition()
        {
            var sequence = new List<Vector2Int>();

            foreach (var basicMatch in BasicMatches)
                sequence.AddRange(basicMatch.GetSequencePosition());

            return sequence;
        }
    }
}