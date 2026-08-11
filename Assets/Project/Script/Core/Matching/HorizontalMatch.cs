using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class HorizontalMatch : BasicMatch
    {
        public HorizontalMatch(int mainIndex, int count, int startIndex)
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
                return new Vector2Int(StartIndex + centerOffset, MainIndex);
            }
          
            return new Vector2Int(-1, -1);
        }

        public override List<Vector2Int> GetSequencePosition()
        {
            var allPoints = new List<Vector2Int>();

            for (int i = 0; i < Count; i++)
                allPoints.Add(new Vector2Int(StartIndex + i, MainIndex));

            return allPoints;
        }

        public override bool IsHorizontal()
        {
            return true;
        }

        public override bool IsVertical()
        {
            return false;
        }
    }
}