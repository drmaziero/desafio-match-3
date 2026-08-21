using DG.Tweening;
using UnityEngine;

namespace Views.Onboarding
{
    public abstract class OnboardingElementView : MonoBehaviour,IOnboardingElementView
    {
        public abstract Tween Show();

        public abstract Tween Close();
    }
}