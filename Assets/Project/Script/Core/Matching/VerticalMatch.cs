using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class VerticalMatch : BasicMatch
    {
        public VerticalMatch(int mainIndex, int count, int startIndex)
        {
            _mainIndex = mainIndex;
            _count = count;
            _startIndex = startIndex;
        }
        
        public override bool HasNewElementOnMatch(Vector2 point)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetCentralPoint()
        {
            if (HasCentralPoint())
            {
                int centerOffset = _count / 2;
                return new Vector2(_startIndex + centerOffset, _mainIndex);
            }
            
            return new Vector2(-1, -1);
        }

        public override string ToDebugString()
        {
            string log = "";

            log += $"Vertical, start in [{_startIndex}][{_mainIndex}]";
            return log;
        }
    }
}