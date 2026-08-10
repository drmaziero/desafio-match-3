using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class VerticalMatch : BasicMatch
    {
        public VerticalMatch(int mainIndex, int count, int startIndex)
        {
            MainIndex = mainIndex;
            Count = count;
            StartIndex = startIndex;
        }

        public override Vector2Int GetCentralPoint()
        {
            if (HasCentralPoint())
            {
                int centerOffset = Count / 2;
                return new Vector2Int(MainIndex, StartIndex + centerOffset);
            }
            
            return new Vector2Int(-1, -1);
        }

        public override List<Vector2Int> GetSequencePosition()
        {
            var allPoints = new List<Vector2Int>();

            for (int i = 0; i < Count; i++)
                allPoints.Add(new Vector2Int(MainIndex, StartIndex + i));

            return allPoints;
        }
        
    }
}