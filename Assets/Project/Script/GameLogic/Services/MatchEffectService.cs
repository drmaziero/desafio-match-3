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
    }
}