using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Views.Tile;

namespace Controllers
{
    public class TileViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int> EnterOnTile;
        public event Action<Vector2Int> ExitOnTile;
        
        [SerializeField] private TileView tileView;
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            EnterOnTile?.Invoke(tileView.GetPositon());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ExitOnTile?.Invoke(tileView.GetPositon());
        }

        public TileView GetView()
        {
            return tileView;
        }
    }
}