using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ClearLineEffect : IMatchEffect
    {
        public Vector2Int Origin { get; }
        public TileType TileType { get; }
        public SpecialTileType SpecialTileType => SpecialTileType.ClearRow;

        public ClearLineEffect(Vector2Int origin, TileType tileType)
        {
            Origin = origin;
            TileType = tileType;
        }
        
        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            for (int i = 0; i < board[Origin.y].Count; i++)
                yield return new Vector2Int(i, Origin.y);
        }
    }
}