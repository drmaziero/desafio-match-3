using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TileSpotView : MonoBehaviour
    {
        public event Action<int, int> Clicked;

        [SerializeField] private Button _button;

        private int _x;
        private int _y;

        #region Unity
        private void Awake()
        {
            _button.onClick.AddListener(OnTileClick);
        }
        #endregion

        public Tween AnimatedSetTile(GameObject tile)
        {
            tile.transform.DOKill();

            return tile.transform.DOMove(transform.position, 0.3f).OnComplete(() =>
            {
                tile.transform.SetParent(transform);
                tile.transform.localPosition = Vector3.zero;
            });
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void SetTile(GameObject tile)
        {
            if (transform.childCount > 0)
            {
                Debug.LogError($"Bad Tile Detected! Desactive child");
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(false);
            }
            tile.transform.SetParent(transform, false);
            tile.transform.position = transform.position;
        }

        public void ReplaceTile(GameObject oldTile, GameObject newTile)
        {
            if (oldTile != null)
            {
                oldTile.transform.DOKill();
                Destroy(oldTile);
            }

            SetTile(newTile);
        }

        private void OnTileClick()
        {
            Clicked?.Invoke(_x, _y);
        }
    }
}
