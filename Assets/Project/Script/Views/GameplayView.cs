using UnityEngine;

namespace Views
{
    public class GameplayView : MonoBehaviour, IUiView
    {
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