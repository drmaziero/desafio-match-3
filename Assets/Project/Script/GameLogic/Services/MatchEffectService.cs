using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Effects;
using GameLogic.Matching;
using Models;
using UnityEngine;

namespace GameLogic.Services
{
    public class MatchEffectService
    {
        private MatchConsumptionTracker _consumptionTracker = new();

        public IEnumerable<IMatchEffect> CreateEffects(DetectedMatches detectedMatches,
            IReadOnlyList<IReadOnlyList<Tile>> board, Vector2Int? movedPosition)
        {
            _consumptionTracker.Clear();

            var effects = new List<IMatchEffect>();

            if (detectedMatches.HasComplexMatches)
                CreateComplexEffects(detectedMatches.ComplexMatches, effects, board);

            if (detectedMatches.HasComposedMatches)
                CreateComposedEffects(detectedMatches.ComposedMatches, effects, board);

            if (detectedMatches.HasBasicMatches)
                CreateBasicEffects(detectedMatches.BasicMatches, effects, board, movedPosition);

            return effects;
        }

        private void CreateComplexEffects(IEnumerable<ComplexMatch> complexMatches, ICollection<IMatchEffect> effects,
            IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            foreach (var complexMatch in complexMatches)
            {
                if (_consumptionTracker.HasConsumedMatches(complexMatch.BasicMatches))
                    continue;

                var origin = complexMatch.Origin;
                var tileType = board[origin.y][origin.x].Type;

                effects.Add(new ExplosionAndCleanCrossEffect(origin, 3, tileType));
                _consumptionTracker.Consume(complexMatch.BasicMatches);
            }
        }

        private void CreateComposedEffects(IEnumerable<ComposedMatch> composedMatches,
            ICollection<IMatchEffect> effects, IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            foreach (var composedMatch in composedMatches)
            {
                if (_consumptionTracker.HasConsumedMatches(composedMatch.BasicMatches))
                    continue;

                var origin = composedMatch.IntersectionPoint;
                var tileType = board[origin.y][origin.x].Type;

                if (composedMatch.IsMatchL())
                    effects.Add(new ExplosionEffect(origin, 1, tileType));
                else
                    effects.Add(new ClearCrossEffect(origin, tileType));

                _consumptionTracker.Consume(composedMatch.BasicMatches);
            }
        }

        private void CreateBasicEffects(IEnumerable<BasicMatch> basicMatches, ICollection<IMatchEffect> effects,
            IReadOnlyList<IReadOnlyList<Tile>> board, Vector2Int? movedPosition)
        {
            foreach (var match in basicMatches)
            {
                if (_consumptionTracker.IsConsumed(match))
                    continue;
                
                if (match.Count <= 3)
                {
                    _consumptionTracker.Consume(match);
                    continue;
                }
                
                var origin = GetEffectOrigin(match, movedPosition);
                var tileType = board[origin.y][origin.x].Type;
                
                switch (match.Count)
                {
                    case > 4:
                        effects.Add(new ClearColor(origin, tileType));
                        break;
                    case > 3 when match.IsHorizontal():
                        effects.Add(new ClearLineEffect(origin, tileType));
                        break;
                    case > 3:
                        effects.Add(new ClearColumnEffect(origin, tileType));
                        break;
                }

                _consumptionTracker.Consume(match);
            }
        }

        private Vector2Int GetEffectOrigin(BasicMatch match, Vector2Int? movedPosition)
        {
            if (movedPosition.HasValue && match.GetSequencePosition().Contains(movedPosition.Value))
                return movedPosition.Value;

            return match.GetDefaultOrigin();
        }

        public HashSet<Vector2Int> GetEffectsPositions(List<List<Tile>> newBoard, IEnumerable<Vector2Int> positions)
        {
            var effectPositions = new HashSet<Vector2Int>();
            
            foreach (var position in positions)
            {
                if (!IsValidPosition(newBoard,position))
                    continue;

                var tile = newBoard[position.y][position.x];
                if (tile.SpecialType == SpecialTileType.None)
                    continue;
                
                
                var currentEffect = CreateEffectFromTile(tile, position);
                foreach (var affectPosition in currentEffect.GetAffectPositions(newBoard))
                {
                    effectPositions.Add(affectPosition);
                }
            }
            
            return effectPositions;
        }

        public Queue<PendingSpecial> GetEffectOnTiles(List<List<Tile>> board, IEnumerable<Vector2Int> positions)
        {
            var result = positions.Where(pos => HasEffectOnTile(board, pos));
            var pendingSpecial = new Queue<PendingSpecial>();

            foreach (var pos in result)
            {
                var curTile = board[pos.y][pos.x];
                pendingSpecial.Enqueue(new PendingSpecial(curTile.Id, curTile.SpecialType));
            }

            return pendingSpecial;
        }
        
        private bool HasEffectOnTile(List<List<Tile>> board, Vector2Int position)
        {
            return board[position.y][position.x].SpecialType != SpecialTileType.None;
        }

        private IMatchEffect CreateEffectFromTile(Tile tile, Vector2Int position)
        {
            IMatchEffect currentEffect = tile.SpecialType switch
            {
                SpecialTileType.ClearRow => new ClearLineEffect(position, tile.Type),
                SpecialTileType.ClearColumn => new ClearColumnEffect(position, tile.Type),
                SpecialTileType.ClearColor => new ClearColor(position, tile.Type),
                SpecialTileType.ExplosionRadius3 => new ExplosionEffect(position, 1, tile.Type),
                SpecialTileType.ClearCross => new ClearCrossEffect(position, tile.Type),
                SpecialTileType.ExplosionRadius5AndCross => new ExplosionAndCleanCrossEffect(position, 3, tile.Type),
                _ => throw new ArgumentOutOfRangeException($"{tile.SpecialType} not create an effect valid")
            };

            return currentEffect;
        }

        public IEnumerable<Vector2Int> GetSpecialAffected(List<List<Tile>> board, IEnumerable<Vector2Int> initPositions, int specialOriginId)
        {
            foreach (var position in initPositions)
            {
                var tile = board[position.y][position.x];
                
                if (tile.SpecialType == SpecialTileType.None)
                    continue;
                
                if (tile.Id == specialOriginId)
                    continue;

                yield return position;
            }
        }

        private bool IsValidPosition(List<List<Tile>> board, Vector2Int position)
        {
            return position.y >= 0 &&
                   position.y < board.Count &&
                   position.x >= 0 &&
                   position.x < board[position.y].Count;
        }
    }
}