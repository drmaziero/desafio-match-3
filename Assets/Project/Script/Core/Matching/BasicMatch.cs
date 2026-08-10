using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public abstract class BasicMatch
    {
        protected int _mainIndex;
        protected int _count;
        protected int _startIndex;
        
        public abstract bool HasNewElementOnMatch(Vector2Int point);

        public bool HasCentralPoint()
        {
            return _count % 2 == 1;
        }

        public abstract Vector2Int GetCentralPoint();

        public abstract List<Vector2Int> GetSequencePosition();

    }
}