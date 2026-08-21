using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Onboarding
{
    public class OnboardingButton : OnboardingElementView
    {
        public event Action ButtonClicked;
        
        [field: SerializeField] private Button targetButton;
        [field: SerializeField] private Image imageTarget;
        [field: SerializeField] private TextMeshProUGUI labelTarget;

        public void Init()
        {
            targetButton.onClick.AddListener(OnClickButton);    
        }

        public void UnregisterClick()
        {
            targetButton.onClick.RemoveListener(OnClickButton);
        }

        private void OnClickButton()
        {
            ButtonClicked?.Invoke();
        }

        public override Tween Show()
        {
            targetButton.gameObject.SetActive(true);
            labelTarget.gameObject.SetActive(true);
            
            targetButton.interactable = false;
            
            var sequence = DOTween.Sequence();
            
            imageTarget.color = new Color(imageTarget.color.r, imageTarget.color.g, imageTarget.color.b, 0.0f);
            labelTarget.color = new Color(labelTarget.color.r, labelTarget.color.g, labelTarget.color.b, 0.0f);
            
            sequence.Join(DOTween.To(
                () => labelTarget.color.a,
                alpha =>
                {
                    Color color = labelTarget.color;
                    color.a = alpha;
                    labelTarget.color = color;
                },
                1.0f,
                0.25f
            ));
            
            
            sequence.Join(DOTween.To(
                () => imageTarget.color.a,
                alpha =>
                {
                    Color color = imageTarget.color;
                    color.a = alpha;
                    imageTarget.color = color;
                },
                1.0f,
                0.25f
            ).OnComplete(() =>
            {
                targetButton.interactable = true;
            }));
            
            return sequence;
        }

        public override Tween Close()
        {
            targetButton.gameObject.SetActive(false);
            labelTarget.gameObject.SetActive(false);
            
            targetButton.interactable = false;
            var sequence = DOTween.Sequence();
            
            sequence.Join(DOTween.To(
                () => labelTarget.color.a,
                alpha =>
                {
                    Color color = labelTarget.color;
                    color.a = alpha;
                    labelTarget.color = color;
                },
                0.0f,
                0.25f
            ));

            sequence.Join( DOTween.To(
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