using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class MainMenuUiView : MonoBehaviour,IUiView
    {
        public event Action GoToGamePlay;
        public event Action GoToOnboarding;
        
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button OnboardingButton;

        private void Awake()
        {
            PlayButton.onClick.AddListener(PlayGame);
            OnboardingButton.onClick.AddListener(Onboarding);
        }
        
        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(PlayGame);
            OnboardingButton.onClick.RemoveListener(Onboarding);
        }
        
        private void Onboarding()
        {
           GoToOnboarding?.Invoke();
        }


        void PlayGame()
        {
            GoToGamePlay?.Invoke();
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}