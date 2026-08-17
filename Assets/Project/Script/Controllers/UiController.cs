using System;
using System.Collections.Generic;
using UnityEngine;
using Views.UI;

namespace Controllers
{
    public class UiController : MonoBehaviour
    {
        public event Action TryRetryGameRequest;
        public event Action TryStartGameRequest;
        
        [SerializeField] private MainMenuUiView mainMenuUI;
        [SerializeField] private GameplayView gameplayUI;
        [SerializeField] private VictoryUiView victoryUI;
        [SerializeField] private LoseUiView loseUI;
        [SerializeField] private NoLifeView noLifeUI;

        private Dictionary<UiType, IUiView> _screens;
        private IUiView _currentUI;
        
        
        private void Awake()
        {
            _screens = new Dictionary<UiType, IUiView>();
            _screens.Add(UiType.MainMenu, mainMenuUI);
            _screens.Add(UiType.Gameplay, gameplayUI);
            _screens.Add(UiType.Victory, victoryUI);
            _screens.Add(UiType.Lose, loseUI);
            _screens.Add(UiType.NoLife, noLifeUI);

            victoryUI.OnGoToMainMenu += GoToMainMenu;
            victoryUI.OnGoToNextLevel += GoToNextLevel;
            loseUI.OnGoToMainMenu += GoToMainMenu;
            loseUI.OnGoToRetry += TryRetryGame;
            mainMenuUI.GoToGamePlay += TryStartGame;

            _currentUI = _screens[UiType.MainMenu];
            _currentUI.Show();
        }

        private void OnDestroy()
        {
            victoryUI.OnGoToMainMenu -= GoToMainMenu;
            victoryUI.OnGoToNextLevel -= GoToNextLevel;
            loseUI.OnGoToMainMenu -= GoToMainMenu;
            loseUI.OnGoToRetry -= TryRetryGame;
            mainMenuUI.GoToGamePlay -= TryStartGame;
        }

        private void TryStartGame()
        {
            TryStartGameRequest?.Invoke();
        }

        private void TryRetryGame()
        {
            TryRetryGameRequest?.Invoke();
        }

        public void GoToGamePlay()
        {
            _currentUI.Hide();
            _currentUI = _screens[UiType.Gameplay];
            _currentUI.Show();
        }

        private void GoToNextLevel()
        {
            _currentUI.Hide();
            _currentUI = _screens[UiType.Gameplay];
            _currentUI.Show();
        }

        private void GoToMainMenu()
        {
            _currentUI.Hide();
            _currentUI = _screens[UiType.MainMenu];
            _currentUI.Show();
        }

        public void GameplayFinished(bool isWin)
        {
            _currentUI.Hide();
            _currentUI = isWin ? _screens[UiType.Victory] : _screens[UiType.Lose];
            _currentUI.Show();
        }

        public void ShowNoLife(TimeSpan? remaining)
        {
            NoLifeView noLifeView =  ((NoLifeView)_screens[UiType.NoLife]);
            noLifeView.Init(remaining);
            noLifeView.Show();
        }
    }

    public enum UiType
    {
        MainMenu,
        Gameplay,
        Victory,
        Lose,
        NoLife
    }
}