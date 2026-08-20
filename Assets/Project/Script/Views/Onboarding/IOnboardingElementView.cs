using DG.Tweening;

namespace Views.Onboarding
{
    public interface IOnboardingElementView
    {
        Tween Show();
        Tween Close();
    }
}