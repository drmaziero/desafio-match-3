using System.Collections.Generic;
using UnityEngine;
using Views;
using Views.UI;

namespace Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private MainMenuUiView mainMenuUI;
        [SerializeField] private GameplayView gameplayUI;
        [SerializeField] private VictoryUiView victoryUI;
        [SerializeField] private LoseUiView loseUI;

        private Dictionary<UiType, IUiView> _screens;
        private IUiView _currentUI;
        
        
        private void Awake()
        {
            _screens = new Dictionary<UiType, IUiView>();
            _screens.Add(UiType.MainMenu, mainMenuUI);
            _screens.Add(UiType.Gameplay, gameplayUI);
            _screens.Add(UiType.Victory, victoryUI);
            _screens.Add(UiType.Lose, loseUI);

            victoryUI.OnGoToMainMenu += GoToMainMenu;
            victoryUI.OnGoToNextLevel += GoToNextLevel;
            loseUI.OnGoToMainMenu += GoToMainMenu;
            loseUI.OnGoToRetry += GoToRetryLevel;
            mainMenuUI.GoToGamePlay += GoToGameplay;

            _currentUI = _screens[UiType.MainMenu];
            _currentUI.Show();
        }

        private void OnDestroy()
        {
            victoryUI.OnGoToMainMenu -= GoToMainMenu;
            victoryUI.OnGoToNextLevel -= GoToNextLevel;
            loseUI.OnGoToMainMenu -= GoToMainMenu;
            loseUI.OnGoToRetry -= GoToRetryLevel;
            mainMenuUI.GoToGamePlay -= GoToGameplay;
        }

        private void GoToGameplay()
        {
            _currentUI.Hide();
            _currentUI = _screens[UiType.Gameplay];
            _currentUI.Show();
        }

        private void GoToRetryLevel()
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
        
        
    }

    public enum UiType
    {
        MainMenu,
        Gameplay,
        Victory,
        Lose
    }
}