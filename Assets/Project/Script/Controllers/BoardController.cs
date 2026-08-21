using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        

        public Tween ClearTiles(IEnumerable<Vector2Int> matchedPosition, Vector2Int? activatedSpecialPosition)
        {
            var mainSequence = DOTween.Sequence();

            if (activatedSpecialPosition.HasValue)
            {
                var specialPosition = activatedSpecialPosition.Value;
                var specialTile = _tiles[specialPosition.y][specialPosition.x];

                mainSequence.Append(specialTile.GetView().AnimateClear());

                switch (specialTile.GetView().SpecialType)
                {
                    case SpecialTileType.None:
                        throw new InvalidDataException("Special none to Clear");
                    case SpecialTileType.ClearRow:
                        mainSequence.Append(ClearUsingClearRow(matchedPosition, activatedSpecialPosition.Value));
                        break;
                    case SpecialTileType.ClearColumn:
                        mainSequence.Append(ClearUsingClearColumn(matchedPosition, activatedSpecialPosition.Value));
                        break;
                    case SpecialTileType.ClearCross:
                        mainSequence.Append(ClearUsingClearCross(matchedPosition, activatedSpecialPosition.Value));
                        break;
                    case SpecialTileType.ClearColor:
                        mainSequence.Append(ClearUsingClearColor(matchedPosition, activatedSpecialPosition));
                        break;
                    default:
                        mainSequence.Append(DefaultClear(matchedPosition, activatedSpecialPosition));
                        break;
                }
            }
            else
                mainSequence.Append(DefaultClear(matchedPosition, null));
            
            return mainSequence;
        }

        private Tween DefaultClear(IEnumerable<Vector2Int> matchedPosition, Vector2Int? activatedSpecialPosition)
        {
            var mainSequence = DOTween.Sequence();
            var normalGroup = DOTween.Sequence();
            
            var positionsList = matchedPosition.ToList();

            foreach (var position in positionsList)
            {
                if (activatedSpecialPosition.HasValue && position == activatedSpecialPosition.Value)
                    continue;

                var tile =
                    _tiles[position.y][position.x];

                normalGroup.Join(
                    tile.GetView().AnimateClear()
                );
            }

            mainSequence.Append(normalGroup);
            return mainSequence;
        }

        private Tween ClearUsingClearRow(IEnumerable<Vector2Int> matchedPosition, Vector2Int activatedSpecialPosition)
        {
            var sequence = DOTween.Sequence();
            
            var positionsList = matchedPosition.ToList();
            var origin = activatedSpecialPosition;

            var stages = positionsList
                .Where(position => position != origin)
                .GroupBy(position => Mathf.Abs(position.x - origin.x))
                .OrderBy(group => group.Key);

            foreach (var stage in stages)
            {
                var stageSequence = DOTween.Sequence();

                foreach (var position in stage)
                {
                    var tile = _tiles[position.y][position.x];
                    stageSequence.Join(tile.GetView().AnimateClear());
                }

                sequence.Append(stageSequence);
            }
           
            return sequence;
        }
        
        private Tween ClearUsingClearColumn(IEnumerable<Vector2Int> matchedPosition, Vector2Int activatedSpecialPosition)
        {
            var sequence = DOTween.Sequence();
            
            var positionsList = matchedPosition.ToList();
            var origin = activatedSpecialPosition;

            var stages = positionsList
                .Where(position => position != origin)
                .GroupBy(position => Mathf.Abs(position.y - origin.y))
                .OrderBy(group => group.Key);

            foreach (var stage in stages)
            {
                var stageSequence = DOTween.Sequence();

                foreach (var position in stage)
                {
                    var tile = _tiles[position.y][position.x];
                    stageSequence.Join(tile.GetView().AnimateClear());
                }

                sequence.Append(stageSequence);
            }
           
            return sequence;
        }
        
        private Tween ClearUsingClearCross(IEnumerable<Vector2Int> matchedPosition, Vector2Int activatedSpecialPosition)
        {
            var sequence = DOTween.Sequence();
            
            var positionsList = matchedPosition.ToList();
            var origin = activatedSpecialPosition;

            var stages = positionsList
                .Where(position => position != origin)
                .GroupBy(position => Mathf.Abs(position.x - origin.x) + Mathf.Abs(position.y - origin.y))
                .OrderBy(group => group.Key);

            foreach (var stage in stages)
            {
                var stageSequence = DOTween.Sequence();

                foreach (var position in stage)
                {
                    var tile = _tiles[position.y][position.x];
                    stageSequence.Join(tile.GetView().AnimateClear());
                }

                sequence.Append(stageSequence);
            }
           
            return sequence;
        }
        
        private Tween ClearUsingClearColor(IEnumerable<Vector2Int> matchedPosition, Vector2Int? activatedSpecialPosition)
        {
            var mainSequence = DOTween.Sequence();
            var normalGroup = DOTween.Sequence();
            
            var positionsList = matchedPosition.ToList();

            foreach (var position in positionsList)
            {
                if (activatedSpecialPosition.HasValue && position == activatedSpecialPosition.Value)
                    continue;

                var tile =
                    _tiles[position.y][position.x];

                normalGroup.Join(
                    tile.GetView().AnimateClearByClearColor()
                );
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
                    State = fromController.GetView().GetState(),
                    IsPendingSpecial = fromController.GetView().IsPendingSpecial
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
                    
                    if (motion.IsPendingSpecial)
                        motion.To.StartSpecialShake();
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

        public void AnimateActiveSpecial(Vector2Int? position)
        {
            if (!position.HasValue)
                return;

            var tile = _tiles[position.Value.y][position.Value.x];
            tile.GetView().StopSpecialShake();
        }

        public void StartPendingSpecials(List<Vector2Int> pendingSpecialPositions)
        {
            foreach (var pendingSpecial in pendingSpecialPositions)
            {
                var tile = _tiles[pendingSpecial.y][pendingSpecial.x];
                tile.GetView().StartSpecialShake();
            }
            
        }
    }
}
