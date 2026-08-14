using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class VictoryUiView : MonoBehaviour, IUiView
    {
        public event Action OnGoToMainMenu;
        public event Action OnGoToNextLevel; 
        
        [SerializeField] private Button NextLevelButton;
        [SerializeField] private Button MainMenuButton;

        private void Awake()
        {
            NextLevelButton.onClick.AddListener(GoToNextLevel);
            MainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        private void OnDestroy()
        {
            NextLevelButton.onClick.RemoveListener(GoToNextLevel);
            MainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            OnGoToMainMenu?.Invoke();
        }

        private void GoToNextLevel()
        {
            OnGoToNextLevel?.Invoke();
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