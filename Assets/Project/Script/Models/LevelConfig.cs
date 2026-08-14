using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class LevelConfig
    {
       public Vector2Int BoardSize { get; private set; }
       public TargetLevel Target { get; private set; }
       public int MaxSwapTile { get; private set; }
       public List<TileType> Types { get; private set; }

       public LevelConfig(Vector2Int boardSize, TargetLevel target, int maxSwapTile, List<TileType> types)
       {
           BoardSize = boardSize;
           Target = target;
           MaxSwapTile = maxSwapTile;
           Types = types;
       }
    }
}