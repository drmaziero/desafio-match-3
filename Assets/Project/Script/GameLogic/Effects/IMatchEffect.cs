using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public interface IMatchEffect
    {
        Vector2Int Origin { get; }
        IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board);
    }
}