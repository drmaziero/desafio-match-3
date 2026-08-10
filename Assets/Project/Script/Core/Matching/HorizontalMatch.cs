using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class HorizontalMatch : BasicMatch
    {
        public HorizontalMatch(int mainIndex, int count, int startIndex)
        {
            _mainIndex = mainIndex;
            _count = count;
            _startIndex = startIndex;
        }
        
        public override bool HasNewElementOnMatch(Vector2Int point)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2Int GetCentralPoint()
        {
            if (HasCentralPoint())
            {
                int centerOffset = _count / 2;
                return new Vector2Int(_startIndex + centerOffset, _mainIndex);
            }
          
            return new Vector2Int(-1, -1);
        }

        public override List<Vector2Int> GetSequencePosition()
        {
            var allPoints = new List<Vector2Int>();

            for (int i = 0; i < _count; i++)
                allPoints.Add(new Vector2Int(_startIndex + i, _mainIndex));

            return allPoints;
        }
    }
}