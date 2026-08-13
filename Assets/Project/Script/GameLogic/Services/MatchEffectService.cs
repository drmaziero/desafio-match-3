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

                effects.Add(new ExplosionAndCleanCrossEffect(origin, 5, tileType));
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
                    effects.Add(new ExplosionEffect(origin, 3, tileType));
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

        public List<Vector2Int> GetEffectsPositions(List<List<Tile>> newBoard, IEnumerable<Vector2Int> positions)
        {
            var effectPositions = new List<Vector2Int>();
            
            foreach (var position in positions)
            {
                var currentEffect = CreateEffectFromTile(newBoard[position.y][position.x], position);
                effectPositions.AddRange(currentEffect.GetAffectPositions(newBoard));
            }
            
            return effectPositions;
        }

        public List<Vector2Int> GetEffectOnTiles(List<List<Tile>> board, IEnumerable<Vector2Int> positions)
        {
            return positions.Where(pos => HasEffectOnTile(board, pos)).ToList();
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
                SpecialTileType.ExplosionRadius3 => new ExplosionEffect(position, 3, tile.Type),
                SpecialTileType.ClearCross => new ClearCrossEffect(position, tile.Type),
                SpecialTileType.ExplosionRadius5AndCross => new ExplosionAndCleanCrossEffect(position, 5, tile.Type),
                _ => throw new ArgumentOutOfRangeException($"{tile.SpecialType} not create an effect valid")
            };

            return currentEffect;
        }

        public HashSet<Vector2Int> ResolveEffectCascate(List<List<Tile>> board, IEnumerable<Vector2Int> initPositions)
        {
            var finalPositions = new HashSet<Vector2Int>(initPositions);
            var activeTileIds = new HashSet<int>();
            var pendingPositions = new Queue<Vector2Int>(initPositions);

            while (pendingPositions.Count > 0)
            {
                var position = pendingPositions.Dequeue();
                var tile = board[position.y][position.x];
                
                if (tile.SpecialType == SpecialTileType.None)
                    continue;
                
                if (!activeTileIds.Add(tile.Id))
                    continue;

                IMatchEffect currentEffect = CreateEffectFromTile(tile, position);
                
                foreach (var affectPosition in currentEffect.GetAffectPositions(board))
                {
                    if (finalPositions.Add(affectPosition))
                    {
                        pendingPositions.Enqueue(affectPosition);
                    }
                }
            }

            return finalPositions;
        }
    }
}