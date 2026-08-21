using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Views.Onboarding
{
    public class OnboardingText : OnboardingElementView
    {
        [field: SerializeField] private TextMeshProUGUI label;
        
        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            
            var initColor = label.color;
            initColor.a = 0.0f;
            label.color = initColor;
            
            sequence.Join(DOTween.To(
                () => label.color.a,
                alpha =>
                {
                    Color color = label.color;
                    color.a = alpha;
                    label.color = color;
                },
                1.0f,
                2.5f
            ));

            return sequence;
        }

        public override Tween Close()
        {
            var sequence = DOTween.Sequence();
            
            var initColor = label.color;
            initColor.a = 1.0f;
            label.color = initColor;
            
            sequence.Join(DOTween.To(
                () => label.color.a,
                alpha =>
                {
                    Color color = label.color;
                    color.a = alpha;
                    label.color = color;
                },
                0.0f,
                0.5f
            ));

            return sequence;
        }
    }
}