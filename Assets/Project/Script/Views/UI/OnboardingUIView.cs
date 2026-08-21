using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class OnboardingUIView : MonoBehaviour, IUiView
    {
        public event Action GoToMainMenu;
        
        [field: SerializeField] 
        private Button closeButton { get; set; }

        private void Awake()
        {
            closeButton.onClick.AddListener(CloseUI);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(CloseUI);
        }

        private void CloseUI()
        {
            GoToMainMenu?.Invoke();
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