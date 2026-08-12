using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ClearColumnEffect : IMatchEffect
    {
        public Vector2Int Origin { get; }

        public ClearColumnEffect(Vector2Int origin)
        {
            Origin = origin;
        }
        
        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            for (int i = 0; i < board.Count; i++)
                yield return new Vector2Int(Origin.x, i);
        }
    }
}