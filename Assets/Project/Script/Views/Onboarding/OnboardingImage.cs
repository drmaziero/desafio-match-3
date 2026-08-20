using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Onboarding
{
    public class OnboardingImage : OnboardingElementView
    {
        [field: SerializeField] private Image imageTarget;
        
        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            
            imageTarget.color = new Color(imageTarget.color.r, imageTarget.color.g, imageTarget.color.b, 0.0f);
            sequence.Append(DOTween.To(
                () => imageTarget.color.a,
                alpha =>
                {
                    Color color = imageTarget.color;
                    color.a = alpha;
                    imageTarget.color = color;
                },
                1.0f,
                0.25f
            ));
            
            return sequence;
        }

        public override Tween Close()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(
                () => imageTarget.color.a,
                alpha =>
                {
                    Color color = imageTarget.color;
                    color.a = alpha;
                    imageTarget.color = color;
                },
                0.0f,
                0.25f
            ));
            
            return sequence;
        }
    }
}