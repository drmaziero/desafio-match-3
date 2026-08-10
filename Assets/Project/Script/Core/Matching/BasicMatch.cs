using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public abstract class BasicMatch
    {
        public int MainIndex { get; protected set; }
        public int StartIndex { get; protected set; }
        public int Count { get; protected set; }

        public bool HasCentralPoint()
        {
            return Count % 2 == 1;
        }

        public abstract Vector2Int GetCentralPoint();

        public abstract List<Vector2Int> GetSequencePosition();

        public void Increase()
        {
            Count++;
        }

        public Vector2Int HasIntersection(BasicMatch otherMatch)
        {
            foreach (var position in GetSequencePosition())
            {
                foreach (var otherPosition in otherMatch.GetSequencePosition())
                {
                    if (position.Equals(otherPosition))
                        return position;
                }
            }

            return new Vector2Int(-1, -1);
        }

        public override bool Equals(object obj)
        {
            if (obj is not BasicMatch other)
                return false;

            return MainIndex == other.MainIndex &&
                   StartIndex == other.StartIndex &&
                   Count == other.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MainIndex, StartIndex, Count);
        }
    }
}