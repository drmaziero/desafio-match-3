using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Views.Tile;

namespace Controllers
{
    public class TileViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler
    {
        public event Action<Vector2Int> EnterOnTile;
        public event Action<Vector2Int> ExitOnTile;
        public event Action DragRequested;
        
        [SerializeField] private TileView tileView;

        private Vector2 _initDragPosition;
        private const float _thresholdDistance = 5.0f;
        private bool _isInitDrag;

        private void Awake()
        {
            _isInitDrag = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnterOnTile?.Invoke(tileView.GetPositon());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ExitOnTile?.Invoke(tileView.GetPositon());
            _isInitDrag = false;
        }

        public TileView GetView()
        {
            return tileView;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isInitDrag) return;
            var currentDistance = Vector2.Distance(eventData.position, _initDragPosition);
            if (currentDistance >= _thresholdDistance)
                DragRequested?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _initDragPosition = eventData.position;
            _isInitDrag = true;
        }

        
    }
}