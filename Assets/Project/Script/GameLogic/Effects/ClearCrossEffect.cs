using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ClearCrossEffect : IMatchEffect
    {
        public Vector2Int Origin { get; }
        public TileType TileType { get; }
        public SpecialTileType SpecialTileType => SpecialTileType.ClearCross;

        private ClearLineEffect _clearLineEffect;
        private ClearColumnEffect _clearColumnEffect;
        
        public ClearCrossEffect(Vector2Int origin, TileType tileType)
        {
            Origin = origin;
            TileType = tileType;

            _clearColumnEffect = new ClearColumnEffect(Origin, tileType);
            _clearLineEffect = new ClearLineEffect(Origin, tileType);
        }
        
        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            foreach (var columnEffectPos in _clearColumnEffect.GetAffectPositions(board))
                positions.Add(columnEffectPos);

            foreach (var lineEffectPos in _clearLineEffect.GetAffectPositions(board))
                positions.Add(lineEffectPos);

            return positions;
        }
    }
}