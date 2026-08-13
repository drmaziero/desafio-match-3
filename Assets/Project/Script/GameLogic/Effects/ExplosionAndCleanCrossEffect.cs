using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ExplosionAndCleanCrossEffect : IMatchEffect
    {
        public Vector2Int Origin { get; }
        public TileType TileType { get; }
        public SpecialTileType SpecialTileType => SpecialTileType.ExplosionRadius5AndCross;
        
        private ExplosionEffect _explosionEffect;
        private ClearCrossEffect _crossEffect;

        public ExplosionAndCleanCrossEffect(Vector2Int origin, int radius, TileType tileType)
        {
            Origin = origin;
            TileType = tileType;

            _explosionEffect = new ExplosionEffect(Origin, radius, tileType);
            _crossEffect = new ClearCrossEffect(Origin, tileType);
        }

        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            foreach (var explosionPos in _explosionEffect.GetAffectPositions(board))
                positions.Add(explosionPos);

            foreach (var crossPos in _crossEffect.GetAffectPositions(board))
                positions.Add(crossPos);

            return positions;
        }
    }
}