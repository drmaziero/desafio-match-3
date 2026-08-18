using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Views.Tile;

namespace Controllers
{
    public class TileController : MonoBehaviour, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<Vector2Int> EnterOnTile;
        public event Action<Vector2Int> DragRequested;
        
        [SerializeField] private TileView tileView;

        private Vector2 _initDragPosition;
        private const float ThresholdDistance = 20.0f;
        private bool _isInitDrag;
        private bool _dragRequested;
        
        private void Awake()
        {
            _isInitDrag = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnterOnTile?.Invoke(tileView.GetPositon());
        }

        public TileView GetView()
        {
            return tileView;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isInitDrag || _dragRequested) 
                return;
            
            var currentDistance = Vector2.Distance(eventData.position, _initDragPosition);
            if (currentDistance < ThresholdDistance)
                return;

            _dragRequested = true;
                
            DragRequested?.Invoke(tileView.GetPositon());
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _initDragPosition = eventData.position;
            _isInitDrag = true;
            _dragRequested = false;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            _isInitDrag = false;
            _dragRequested = false;
        }
    }
}