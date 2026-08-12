using System.Collections.Generic;
using System.Linq;
using GameLogic.Effects;
using GameLogic.Matching;
using UnityEngine;

namespace GameLogic.Services
{
    public class MatchEffectService
    {
        private MatchConsumptionTracker _consumptionTracker = new();

        public IEnumerable<IMatchEffect> CreateEffects(
            IEnumerable<ComplexMatch> complexMatches, 
            IEnumerable<ComposedMatch> composedMatches,
            IEnumerable<BasicMatch> horizontalMatches, 
            IEnumerable<BasicMatch> verticalMatches,
            Vector2Int? movedPosition)
        {
            _consumptionTracker.Clear();

            var effects = new List<IMatchEffect>();
            
            CreateComplexEffects(complexMatches, effects);
            CreateComposedEffects(composedMatches, effects);
            CreateBasicEffects(horizontalMatches.Concat(verticalMatches), effects, movedPosition);
            return effects;
        }

        private void CreateComplexEffects(IEnumerable<ComplexMatch> complexMatches, ICollection<IMatchEffect> effects)
        {
            foreach (var complexMatch in complexMatches)
            {
                if (_consumptionTracker.HasConsumedMatches(complexMatch.BasicMatches))
                    continue;
                
                effects.Add(new ExplosionAndCleanCrossEffect(complexMatch.Origin,5));
                _consumptionTracker.Consume(complexMatch.BasicMatches);
            }
        }
        
        private void CreateComposedEffects(IEnumerable<ComposedMatch> composedMatches, ICollection<IMatchEffect> effects)
        {
            foreach (var composedMatch in composedMatches)
            {
                if (_consumptionTracker.HasConsumedMatches(composedMatch.BasicMatches))
                    continue;

                if (composedMatch.IsMatchL())
                    effects.Add(new ExplosionEffect(composedMatch.IntersectionPoint,3));
                else
                    effects.Add(new ClearCrossEffect(composedMatch.IntersectionPoint));
                
                _consumptionTracker.Consume(composedMatch.BasicMatches);
            }
        }
        
        private void CreateBasicEffects(IEnumerable<BasicMatch> basicMatches, ICollection<IMatchEffect> effects, Vector2Int? movedPosition)
        {
            foreach (var match in basicMatches)
            {
                if (_consumptionTracker.IsConsumed(match)) 
                    continue;

                switch (match.Count)
                {
                    case > 4:
                        effects.Add(new ClearColor(GetEffectOrigin(match, movedPosition)));
                        break;
                    case > 3 when match.IsHorizontal():
                        effects.Add(new ClearLineEffect(GetEffectOrigin(match, movedPosition)));
                        break;
                    case > 3:
                        effects.Add(new ClearColumnEffect(GetEffectOrigin(match, movedPosition)));
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