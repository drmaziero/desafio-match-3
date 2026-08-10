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
        
        public override bool HasNewElementOnMatch(Vector2 point)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetCentralPoint()
        {
            if (HasCentralPoint())
            {
                int centerOffset = _count / 2;
                return new Vector2(_mainIndex, _startIndex + centerOffset);
            }
          
            return new Vector2(-1, -1);
        }

        public override string ToDebugString()
        {
            string log = "";
            log += $"Horizontal, start in [{_mainIndex}][{_startIndex}]";
            
            return log;
        }
    }
}