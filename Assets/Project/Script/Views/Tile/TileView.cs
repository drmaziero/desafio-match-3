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
        [Header("Normal State")]
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
        public SpecialTileType SpecialType { get; private set; }

        private Tween _pendingShakeTween;
        public bool IsPendingSpecial { get; private set; }
        
        private void Awake()
        {
            _colorDictionary = new Dictionary<TileType, Color>();
            _specialRoots = new Dictionary<SpecialTileType, GameObject>();
            _specialLiquids = new Dictionary<SpecialTileType, Image>();
            _fxDictionary = new Dictionary<TileFX, TileViewFX>();

            _pendingShakeTween = null;
            IsPendingSpecial = false;

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
        
        
        public void ApplyTileType(TileType type)
        {
            if (SpecialType != SpecialTileType.None)
                _specialRoots[SpecialType].SetActive(false);

            SpecialType = SpecialTileType.None;
            _type = type;
            
            liquid.color = _colorDictionary[type];

            specialStateContainer.SetActive(false);
            normalStateContainer.SetActive(true);
        }
        
        public void ApplySpecialType(SpecialTileType type)
        {
            if (SpecialType != SpecialTileType.None)
                _specialRoots[SpecialType].SetActive(false);
            
            SpecialType = type;
            
            _specialLiquids[type].color = _colorDictionary[_type];
            
            normalStateContainer.SetActive(false);
            specialStateContainer.SetActive(true);
            
            _specialRoots[type].SetActive(true);
        }

        public void SetEmpty()
        {
            StopSpecialShake();
            
            _type = TileType.None;
            normalStateContainer.SetActive(false);
            specialStateContainer.SetActive(false);
            
            if (SpecialType != SpecialTileType.None)
                _specialRoots[SpecialType].SetActive(false);
            
            SpecialType = SpecialTileType.None;
            
            _pendingShakeTween = null;
            IsPendingSpecial = false;
        }

        public TileViewState GetState()
        {
            return new TileViewState(_type, SpecialType);
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
            return SpecialType is SpecialTileType.ExplosionRadius3 or
                SpecialTileType.ExplosionRadius5AndCross
                ? AnimateExplosionClear()
                : AnimateNormalClear();
        }
        
        private Tween AnimateNormalClear()
        {
            Sequence sequence = DOTween.Sequence();
            var color = _colorDictionary[_type];
            sequence.Append(visualContainer.DOScale(1.08f, 0.05f));
            sequence.AppendCallback(() =>
            {
                PlayClearTileFX(color);
            });
            sequence.Append(visualContainer.DOScale(Vector3.zero, 0.05f).SetEase(Ease.InBack));
                sequence.OnComplete(()=>
                {
                    SetEmpty();
                    ResetVisualTransform();
                });
                
            return sequence;
        }

        private Tween AnimateExplosionClear()
        {
            const float shakeDuration = 1.0f;
            const float explosionDuration = 0.3f;

            var type = SpecialType;
            var color = _colorDictionary[_type];

            Sequence sequence = DOTween.Sequence();

            sequence.Append(visualContainer.DOScale(1.08f, 0.05f));
            sequence.Append(visualContainer.DOShakePosition(shakeDuration, 8.0f, 25, 30.0f));
            sequence.AppendCallback(() =>
            {
                PlayClearWithExplosionTileFX(type, color);
            });

            sequence.AppendInterval(explosionDuration);
            sequence.AppendCallback(StopClearTileFX);

            sequence.OnComplete(() =>
            {
                SetEmpty();
                ResetVisualTransform();
            });

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

        private void PlayClearTileFX(Color color)
        {
            var currentFx = _fxDictionary[TileFX.ExplodeTile];
            currentFx.image.color = Color.Lerp(color, Color.white, 0.25f);
            currentFx.root.SetActive(true);
            currentFx.animator.SetTrigger("Normal");

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
        
        private void PlayClearWithExplosionTileFX(SpecialTileType type, Color color)
        {
            var currentFx = _fxDictionary[TileFX.ExplodeTile];
            currentFx.image.color = Color.Lerp(color, Color.white, 0.25f);
            currentFx.root.SetActive(true);
            currentFx.animator.SetTrigger(type == SpecialTileType.ExplosionRadius3 ? "Radius1" : "Radius3");
        }

        private void StopClearTileFX()
        {
            var currentFx = _fxDictionary[TileFX.ExplodeTile];
            currentFx.root.SetActive(false);
        }

        public void StartSpecialShake()
        {
            if (IsPendingSpecial)
                return;

            IsPendingSpecial = true;
            _pendingShakeTween?.Kill();

            _pendingShakeTween = specialStateContainer.transform.DOShakePosition(0.4f, 5.0f, 15, 20.0f, false, false)
                .SetLoops(-1, LoopType.Restart);
        }

        public void StopSpecialShake()
        {
            IsPendingSpecial = false;
            _pendingShakeTween?.Kill();
            _pendingShakeTween = null;
            
            specialStateContainer.transform.localPosition = Vector3.zero;
        }

        public Tween AnimateClearByClearColor()
        {
            var sequence = DOTween.Sequence();
            const float effectDuration = 0.5f;

            Tween shakeTween = null;
            Tween scaleTween = null;
            
            sequence.AppendCallback(() =>
            {
                shakeTween =  visualContainer.DOShakePosition(0.2f, 5.0f, 15, 20.0f, false, false).SetLoops(-1, LoopType.Restart);
                scaleTween = visualContainer.DOScale(1.2f, 0.1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            });
            sequence.AppendInterval(effectDuration);
            sequence.AppendCallback(() =>
            {
                shakeTween?.Kill();
                scaleTween?.Kill();
                
                visualContainer.localPosition = Vector3.zero;
                visualContainer.localScale = Vector3.one;
            });
            sequence.Append(AnimateNormalClear());

            return sequence;
        }
        
    }
}
