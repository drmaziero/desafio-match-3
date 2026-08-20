using DG.Tweening;
using UnityEngine;

namespace Views.Onboarding
{
    public class OnboardingPageView : OnboardingElementView
    {
        [field: SerializeField] private OnboardingElementView[] components;
        
        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            foreach (var component in components)
                sequence.Append(component.Show());
            
            return sequence;
        }

        public override Tween Close()
        {
            var sequence = DOTween.Sequence();
            foreach (var component in components)
                sequence.Join(component.Close());

            return sequence;
        }
    }
}