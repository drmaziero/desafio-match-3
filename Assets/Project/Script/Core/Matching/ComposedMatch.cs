using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class ComposedMatch
    {
        private Vector2Int _intersectionPoint;
        private BasicMatch[] _basicMatches;

        public ComposedMatch(Vector2Int intersectionPoint, List<BasicMatch> basicMatches)
        {
            _intersectionPoint = intersectionPoint;
            _basicMatches = basicMatches.ToArray();
        }

        public bool IsMatchT()
        {
            foreach (var basicMatch in _basicMatches)
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
    }
}