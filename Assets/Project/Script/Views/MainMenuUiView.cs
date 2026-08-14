using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuUiView : MonoBehaviour,IUiView
    {
        public event Action GoToGamePlay; 
        [SerializeField] private Button PlayButton;

        private void Awake()
        {
            PlayButton.onClick.AddListener(PlayGame);
        }

        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(PlayGame);
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