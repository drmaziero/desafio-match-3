using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Matching
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

        public override IEnumerable<Vector2Int> GetSequencePosition()
        {
            for (int i = 0; i < Count; i++)
                yield return new Vector2Int(StartIndex + i, MainIndex);
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