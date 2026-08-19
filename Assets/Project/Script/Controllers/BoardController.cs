using System;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Views.Tile;

namespace Controllers
{
    public class BoardController : MonoBehaviour, IPointerExitHandler
    {
        public event Action<TileController> TileCreated;
        public event Action OutOfBoard;

        [SerializeField] private GridLayoutGroup _boardContainer;
        [SerializeField] private TileController tilePrefab;
        
        private TileController[][] _tiles;

        public void CreateBoard(List<List<Tile>> board)
        {
            _boardContainer.constraintCount = board[0].Count;
            _tiles = new TileController[board.Count][];

            for (int y = 0; y < board.Count; y++)
            {
                _tiles[y] = new TileController[board[0].Count];

                for (int x = 0; x < board[0].Count; x++)
                {
                    TileController tileController = Instantiate(tilePrefab, _boardContainer.transform, false);
                    TileView tileView = tileController.GetView();
                    
                    tileView.SetPosition(x, y);
                    tileView.ApplyState(new TileViewState(board[y][x].Type, board[y][x].SpecialType));
                    
                    TileCreated?.Invoke(tileController);
                    _tiles[y][x] = tileController;
                }
            }
        }

        public void ClearBoard()
        {
            if (_tiles == null)
                return;

            foreach (var row in _tiles)
            {
                foreach (var tile in row)
                {
                    if (tile != null)
                        Destroy(tile.gameObject);
                }
            }

            _tiles = null;
        }

        public Tween RefillTiles(List<AddedTileInfo> addedTiles)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var addedTileInfo in addedTiles)
            {
                Vector2Int position = addedTileInfo.Position;
                TileController tile = _tiles[position.y][position.x];
                tile.GetView().ApplyTileType(addedTileInfo.Type);

                sequence.Join(tile.GetView().AnimateShow());
            }

            return sequence;
        }
        
        public Tween CreateSpecialTile(IEnumerable<AddedSpecialTileInfo> addedSpecialTiles)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var specialTileInfo in addedSpecialTiles)
            {
                Vector2Int position = specialTileInfo.Position;
                TileController tile = _tiles[position.y][position.x];
                tile.GetView().ApplySpecialType(specialTileInfo.SpecialTileType);
                
                sequence.Join(tile.GetView().AnimateSpecialCreated());
            }
            return sequence;
        }
        

        public Tween ClearTiles(IEnumerable<Vector2Int> matchedPosition)
        {
            Sequence mainSequence = DOTween.Sequence();
            Sequence normalGroup = DOTween.Sequence();
            
            foreach (var position in matchedPosition)
            {
                TileController tile = _tiles[position.y][position.x];
                var tileState = tile.GetView().GetState();
                bool isExplosion = tileState.SpecialType is SpecialTileType.ExplosionRadius3
                    or SpecialTileType.ExplosionRadius5AndCross;
                
                if (isExplosion)
                {
                    mainSequence.Append(normalGroup);
                    mainSequence.Append(tile.GetView().AnimateClear());
                    normalGroup = DOTween.Sequence();
                }
                else
                {
                    normalGroup.Join(tile.GetView().AnimateClear());
                }
            }

            mainSequence.Append(normalGroup);
            return mainSequence;
        }

        public Tween MoveTiles(List<MovedTileInfo> movedTiles)
        {
            var motionList = new List<TileViewMotion>();
            
            foreach (var moveInfo in movedTiles)
            {
                TileController fromController = _tiles[moveInfo.From.y][moveInfo.From.x];
                TileController toController = _tiles[moveInfo.To.y][moveInfo.To.x];
                
                motionList.Add(new TileViewMotion()
                {
                    From = fromController.GetView(),
                    To = toController.GetView(),
                    State = fromController.GetView().GetState()
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
        
        public void OnPointerExit(PointerEventData eventData)
        {
            OutOfBoard?.Invoke();
        }
    }
}
