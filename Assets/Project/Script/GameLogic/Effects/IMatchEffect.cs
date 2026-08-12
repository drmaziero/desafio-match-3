using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public interface IMatchEffect
    {
        Vector2Int Origin { get; }
        TileType TileType { get; }
        SpecialTileType SpecialTileType { get; }
        IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board);
    }
}