using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ExplosionEffect : IMatchEffect
    {
        public Vector2Int Origin { get; }
        public TileType TileType { get; }
        public SpecialTileType SpecialTileType => SpecialTileType.ExplosionRadius3;
        private int _radius;

        public ExplosionEffect(Vector2Int origin, int radius, TileType tileType)
        {
            Origin = origin;
            _radius = radius;
            TileType = tileType;
        }
        
        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            int minX = Mathf.Max(Origin.x - _radius, 0);
            int minY = Mathf.Max(Origin.y - _radius, 0);
            int maxX = Mathf.Min(Origin.x + _radius + 1, board[Origin.y].Count);
            int maxY = Mathf.Min(Origin.y + _radius + 1, board.Count);
            

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                    yield return new Vector2Int(x, y);
            }
        }
    }
}