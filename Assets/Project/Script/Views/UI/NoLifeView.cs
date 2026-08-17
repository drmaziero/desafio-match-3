using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class NoLifeView : MonoBehaviour, IUiView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI nextLifeTimer;
        
        private TimeSpan? _remainingTime;
        private Coroutine _timerCoroutine;

        private void Awake()
        {
            closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Close);
        }
        
        public void Init(TimeSpan? remainingTime)
        {
            _remainingTime = remainingTime;
        }

        private IEnumerator UpdateTimer()
        {
            if (_remainingTime == null)
                yield break;
            
            while (_remainingTime.Value > TimeSpan.Zero)
            {
                _remainingTime -= TimeSpan.FromSeconds(1);
                UpdateTimerText();
                yield return new WaitForSecondsRealtime(1);
            }
            
            _remainingTime = TimeSpan.Zero;
            UpdateTimerText();
            Close();
        }

        private void UpdateTimerText()
        {
            if (_remainingTime != null)
                nextLifeTimer.SetText($"{_remainingTime.Value.Minutes:00}:{_remainingTime.Value.Seconds:00}");
        }
        
        
        private void Close()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            
            _timerCoroutine = StartCoroutine(UpdateTimer());
        }

        public void Hide()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
            gameObject.SetActive(false);
        }
    }
}