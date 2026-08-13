using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Matching
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

        public override IEnumerable<Vector2Int> GetSequencePosition()
        {
            for (int i = 0; i < Count; i++)
                yield return new Vector2Int(MainIndex, StartIndex + i);
        }

        public override bool IsHorizontal()
        {
            return false;
        }

        public override bool IsVertical()
        {
            return true;
        }

        public override Vector2Int GetDefaultOrigin()
        {
            return new Vector2Int(MainIndex, StartIndex + Count / 2);
        }
    }
}