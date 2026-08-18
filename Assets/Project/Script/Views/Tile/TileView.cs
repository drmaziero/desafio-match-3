using System;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Tile
{
    public class TileView : MonoBehaviour
    {
        public event Action<int, int> Clicked;

        [Header("Normal State")]
        [SerializeField] private Button _button;
        [SerializeField] private Image liquid;
        [SerializeField] private List<TileColor> colors;

        [Header("Special State")] 
        [SerializeField] private List<SpecialComponents> specialComponents;

        [Header("Containers")] 
        [SerializeField] private GameObject normalStateContainer;
        [SerializeField] private GameObject specialStateContainer;
        [SerializeField] private Transform visualContainer;


        [Header("FX")] [SerializeField] private List<TileViewFX> fxComponents;
        
        private Dictionary<TileType, Color> _colorDictionary;
        private Dictionary<SpecialTileType, GameObject> _specialRoots;
        private Dictionary<SpecialTileType, Image> _specialLiquids;
        private Dictionary<TileFX, TileViewFX> _fxDictionary;
        
        private int _x;
        private int _y;
        private TileType _type;
        private SpecialTileType _specialType;
        
        private void Awake()
        {
            _button.onClick.AddListener(OnTileClick);
            _colorDictionary = new Dictionary<TileType, Color>();
            _specialRoots = new Dictionary<SpecialTileType, GameObject>();
            _specialLiquids = new Dictionary<SpecialTileType, Image>();
            _fxDictionary = new Dictionary<TileFX, TileViewFX>();

            foreach (var tileColor in colors)
                _colorDictionary.Add(tileColor.type, tileColor.color);

            foreach (var specialComponent in specialComponents)
            {
                _specialRoots.Add(specialComponent.type, specialComponent.root);
                _specialLiquids.Add(specialComponent.type, specialComponent.liquid);
            }
            
            foreach (var fx in fxComponents)
            {
                _fxDictionary.Add(fx.type, fx);
                fx.root.SetActive(false);
            }
            
            SetEmpty();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTileClick);
        }
        
        public void ApplyTileType(TileType type)
        {
            if (_specialType != SpecialTileType.None)
                _specialRoots[_specialType].SetActive(false);

            _specialType = SpecialTileType.None;
            _type = type;
            
            liquid.color = _colorDictionary[type];

            specialStateContainer.SetActive(false);
            normalStateContainer.SetActive(true);
            
            _button.interactable = true;
        }
        
        public void ApplySpecialType(SpecialTileType type)
        {
            if (_specialType != SpecialTileType.None)
                _specialRoots[_specialType].SetActive(false);
            
            _specialType = type;
            
            _specialLiquids[type].color = _colorDictionary[_type];
            
            normalStateContainer.SetActive(false);
            specialStateContainer.SetActive(true);
            
            _specialRoots[type].SetActive(true);
            
            _button.interactable = true;
        }

        public void SetEmpty()
        {
            _type = TileType.None;
            normalStateContainer.SetActive(false);
            specialStateContainer.SetActive(false);
            
            if (_specialType != SpecialTileType.None)
                _specialRoots[_specialType].SetActive(false);
            
            _specialType = SpecialTileType.None;
            
            _button.interactable = false;
        }

        public TileViewState GetState()
        {
            return new TileViewState(_type, _specialType);
        }
        
        public void ApplyState(TileViewState state)
        {
            if (state.IsEmpty)
                SetEmpty();
            else
            {
                ApplyTileType(state.Type);

                if (state.SpecialType != SpecialTileType.None) 
                    ApplySpecialType(state.SpecialType);
            }
        }

        public void ResetVisualTransform()
        {
            visualContainer.localPosition = Vector3.zero;
            visualContainer.localScale = Vector3.one;
        }

        public Tween AnimateShow()
        {
            visualContainer.DOKill();
            visualContainer.localScale = Vector3.zero;
            return visualContainer.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack);
        }

        public Tween AnimateSpecialCreated()
        {
            visualContainer.DOKill();

            Sequence sequence = DOTween.Sequence();
            visualContainer.localScale = Vector3.zero;
            sequence.Append(visualContainer.DOScale(0.5f, 0.1f));
            sequence.AppendCallback(PlayCreateSpecialTileFX);
            sequence.Append(visualContainer.DOScale(1.0f, 0.1f)).SetEase(Ease.OutBack);
            return sequence;
        }

        public Tween AnimateClear()
        {
            visualContainer.DOKill();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(visualContainer.DOScale(1.08f, 0.05f));
            sequence.AppendCallback(() =>
            {
                PlayClearTileFX(_colorDictionary[_type]);
            });
            sequence.Append(visualContainer.DOScale(Vector3.zero, 0.05f).SetEase(Ease.InBack)).OnComplete(SetEmpty);
            return sequence;
        }

        public Tween AnimateMoveTo(Vector3 target)
        {
            visualContainer.DOKill();
            return visualContainer.DOMove(target, 0.2f);
        }
        
        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Vector2Int GetPositon()
        {
            return new Vector2Int(_x, _y);
        }

        private void OnTileClick()
        {
            Clicked?.Invoke(_x, _y);
        }

        private void PlayClearTileFX(Color color)
        {
            var currentFx = _fxDictionary[TileFX.ExplodeTile];
            currentFx.image.color = Color.Lerp(color, Color.white, 0.25f);
            currentFx.root.SetActive(true);
            currentFx.animator.Play(currentFx.animationName, 0, 0.0f);

            DOVirtual.DelayedCall(0.2f, () =>
            {
                currentFx.root.SetActive(false);
            });
        }

        private void PlayCreateSpecialTileFX()
        {
            var currentFx = _fxDictionary[TileFX.CreateSpecialTile];
            currentFx.root.SetActive(true);
            currentFx.animator.Play(currentFx.animationName, 0, 0.0f);

            DOVirtual.DelayedCall(0.2f, () =>
            {
                currentFx.root.SetActive(false);
            });
        }
    }
}
