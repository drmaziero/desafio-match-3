using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class TargetLevel
    {
        public int Score { get; private set; }
        public TileTypeCounter[] TypeCounter { get; private set; }
        public SpecialTypeCounter[] SpecialCounter { get; private set; }

        public TargetLevel(int score, IEnumerable<TileTypeCounter> typeCounter, IEnumerable<SpecialTypeCounter> specialCounter)
        {
            Score = score;
            TypeCounter = typeCounter.ToArray();
            SpecialCounter = specialCounter.ToArray();
        }

        public bool HasTargetScore()
        {
            return Score > 0;
        }

        public bool HasTargetType()
        {
            return TypeCounter.Length > 0;
        }

        public bool HasTargetSpecial()
        {
            return SpecialCounter.Length > 0;
        }
    }

    public class TileTypeCounter
    {
        public TileType Type { get; private set; }
        public int Count { get; private set; }

        public TileTypeCounter(TileType type, int count)
        {
            Type = type;
            Count = count;
        }
    }

    public class SpecialTypeCounter
    {
        public SpecialTileType Type { get; private set; }
        public int Count { get; private set; }

        public SpecialTypeCounter(SpecialTileType type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}