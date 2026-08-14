using UnityEngine;

namespace Project.Script.Manager
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject victoryUI;
        [SerializeField] private GameObject loseUI;

    }

    public enum UiType
    {
        MainMenu,
        Loading,
        Gameplay,
        Victory,
        Lose
    }
}