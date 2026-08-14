using System;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class BoardView : MonoBehaviour
    {
        public event Action<int, int> TileClicked;

        [SerializeField] private GridLayoutGroup _boardContainer;
        [SerializeField] private TilePrefabRepository _tilePrefabRepository;
        [SerializeField] private TileView tilePrefab;
        
        private TileView[][] _tiles;

        public void CreateBoard(List<List<Tile>> board)
        {
            _boardContainer.constraintCount = board[0].Count;
            _tiles = new TileView[board.Count][];

            for (int y = 0; y < board.Count; y++)
            {
                _tiles[y] = new TileView[board[0].Count];

                for (int x = 0; x < board[0].Count; x++)
                {
                    TileView tileView = Instantiate(tilePrefab, _boardContainer.transform, false);
                    tileView.SetPosition(x, y);
                    tileView.Clicked += TileSpot_Clicked;
                    tileView.ApplyState(new TileViewState(board[y][x].Type, board[y][x].SpecialType));
                    _tiles[y][x] = tileView;
                }
            }
        }

        public Tween RefillTiles(List<AddedTileInfo> addedTiles)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var addedTileInfo in addedTiles)
            {
                Vector2Int position = addedTileInfo.Position;
                TileView tile = _tiles[position.y][position.x];
                tile.ApplyTileType(addedTileInfo.Type);

                sequence.Join(tile.AnimateShow());
            }

            return sequence;
        }
        
        public Tween CreateSpecialTile(IEnumerable<AddedSpecialTileInfo> addedSpecialTiles)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var specialTileInfo in addedSpecialTiles)
            {
                Vector2Int position = specialTileInfo.Position;
                TileView tileView = _tiles[position.y][position.x];
                tileView.ApplySpecialType(specialTileInfo.SpecialTileType);
                
                sequence.Join(tileView.AnimateSpecialCreated());
            }
            return sequence;
        }
        

        public Tween ClearTiles(IEnumerable<Vector2Int> matchedPosition)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var position in matchedPosition)
            {
                TileView tileView = _tiles[position.y][position.x];
                sequence.Join(tileView.AnimateClear());
            }

            return sequence;
        }

        public Tween MoveTiles(List<MovedTileInfo> movedTiles)
        {
            var motionList = new List<TileViewMotion>();
            
            foreach (var moveInfo in movedTiles)
            {
                TileView fromView = _tiles[moveInfo.From.y][moveInfo.From.x];
                TileView toView = _tiles[moveInfo.To.y][moveInfo.To.x];
                
                motionList.Add(new TileViewMotion()
                {
                    From = fromView,
                    To = toView,
                    State = fromView.GetState()
                });
            }
            
            Sequence sequence = DOTween.Sequence();
            
            foreach (var motion in motionList)
                sequence.Join(motion.From.AnimateMoveTo(motion.To.transform.position));

            sequence.OnComplete(() =>
            {
                foreach (var motion in motionList)
                {
                    motion.From.SetEmpty();
                    motion.From.ResetVisualTransform();
                }

                foreach (var motion in motionList)
                {
                    motion.To.ApplyState(motion.State);
                    motion.To.ResetVisualTransform();
                }
            });
            
            return sequence;
        }

        public Tween SwapTiles(int fromX, int fromY, int toX, int toY)
        {
            var from = new Vector2Int(fromX, fromY);
            var to = new Vector2Int(toX, toY);

            var motions = new List<MovedTileInfo>()
            {
                new()
                {
                    From = from,
                    To = to
                },
                new()
                {
                    From = to,
                    To = from
                }
            };

            return MoveTiles(motions);
        }

        #region Events
        private void TileSpot_Clicked(int x, int y)
        {
            TileClicked(x, y);
        }
        #endregion
    }
}
