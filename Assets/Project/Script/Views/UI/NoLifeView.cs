using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class NoLifeView : MonoBehaviour, IUiView
    {
        public event Action OnGoToMainMenu;

        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI nextLifeTimer;
        
        private IUiView _previousView;
        private TimeSpan? _remainingTime;

        private void Awake()
        {
            closeButton.onClick.AddListener(GoToMainMenu);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(GoToMainMenu);
        }
        
        public void Init(IUiView previousView, TimeSpan? remainingTime)
        {
            _previousView = previousView;
            _remainingTime = remainingTime;
        }

        private IEnumerator UpdateTimer()
        {
            if (_remainingTime == null)
                yield return null;
            
            while (true)
            {
                _remainingTime -= TimeSpan.FromSeconds(1);
                if (_remainingTime != null)
                    nextLifeTimer.SetText($"{_remainingTime.Value.Minutes}:{_remainingTime.Value.Seconds}");

                yield return new WaitForSeconds(1);
            }
        }
        
        
        private void GoToMainMenu()
        {
            OnGoToMainMenu?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            StartCoroutine(UpdateTimer());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _previousView.Hide();
        }
    }
}