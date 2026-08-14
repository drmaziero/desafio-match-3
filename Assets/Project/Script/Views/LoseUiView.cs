using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LoseUiView : MonoBehaviour, IUiView
    {
        public event Action OnGoToMainMenu;
        public event Action OnGoToRetry;
        
        [SerializeField] private Button RetryButton;
        [SerializeField] private Button MainMenuButton;

        private void Awake()
        {
            RetryButton.onClick.AddListener(GoToRetry);
            MainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        
        private void GoToMainMenu()
        {
            OnGoToMainMenu?.Invoke();
        }

        private void GoToRetry()
        {
            OnGoToRetry?.Invoke();
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