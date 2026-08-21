using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Effects;
using GameLogic.Matching;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.Services
{
    public class GameService
    {
        public event Action<DetectedMatches, int> ComputeScore;
        public event Action<IEnumerable<Vector2Int>, SpecialTileType, int> ComputeSpecialScore;
        public event Action<HashSet<Vector2Int>,List<List<Tile>>> ComputeMatches; 
        
        private List<List<Tile>> _boardTiles;
        private List<TileType> _tilesTypes;
        private int _tileCount;
        private MatchingService _matchingService;
        private MatchEffectService _effectService;

        private int _cascadeCounter;
        private Vector2Int? _movePosition;
        private Queue<PendingSpecial> _pendingSpecials;
        private bool _isTurnActive;

        public GameService()
        {
            _matchingService = new MatchingService();
            _effectService = new MatchEffectService();
            _pendingSpecials = new Queue<PendingSpecial>();
        }
        
        public List<List<Tile>> StartGame(int boardWidth, int boardHeight, List<TileType> tileTypes)
        {
            _tilesTypes = tileTypes;
            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);
            
            _matchingService.Init();
            return _boardTiles;
        }
        
        public bool IsValidMovement(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            if (_matchingService.HasMatchWithPosition(newBoard, new Vector2Int(fromX, fromY),
                    newBoard[fromY][fromX].Type))
            {
                return true;
            }

            if (_matchingService.HasMatchWithPosition(newBoard, new Vector2Int(toX, toY), newBoard[toY][toX].Type))
            {
                return true;
            }

            return newBoard[toY][toX].SpecialType != SpecialTileType.None ||
                   newBoard[fromY][fromX].SpecialType != SpecialTileType.None;
        }

        public BoardSequence SwapTile(Vector2Int from, Vector2Int to)
        {
            ResetTurn();

            _isTurnActive = true;
            _cascadeCounter = 1;

            (_boardTiles[to.y][to.x], _boardTiles[from.y][from.x]) = (_boardTiles[from.y][from.x], _boardTiles[to.y][to.x]);

            _movePosition = new Vector2Int(to.x, to.y);

            _pendingSpecials = _effectService.GetEffectOnTiles(_boardTiles,
                new List<Vector2Int>() { new(to.x, to.y), new(from.x, from.y) });

            return TryResolveCurrentStep();
        }

        public BoardSequence ResolveNextStep()
        {
            if (!_isTurnActive)
                return null;

            _movePosition = null;

            _cascadeCounter++;
            
            var step = ResolveStep();

            if (step != null) return step;
            EndTurn();
            return null;
        }

        private BoardSequence TryResolveCurrentStep()
        {
            var step = ResolveStep();

            if (step != null) return step;
            EndTurn();
            return null;
        }

        private BoardSequence ResolveStep()
        {
            if (_pendingSpecials.Count > 0)
            {
                var pendingSpecial = _pendingSpecials.Dequeue();
                return ResolvePendingSpecial(pendingSpecial);
            }

            var detectedMatches = _matchingService.FindMatches(_boardTiles);
            return !detectedMatches.HasBasicMatches ? null : ResolveMatches(detectedMatches);
        }

        private BoardSequence ResolvePendingSpecial(PendingSpecial pendingSpecial)
        {
            var hasPending = TryGetPendingSpecialPosition(pendingSpecial, out var pendingPos);
            if (!hasPending)
                return null;
            
            var positions = _effectService.GetEffectsPositions(_boardTiles, new[] { pendingPos })
                .ToHashSet();
            var addedPendingSpecials = RegisterPendingSpecials(new Queue<Vector2Int>(positions), pendingPos);
            var pendingSpecialPos = GetPendingSpecialPositions(addedPendingSpecials);

            ComputeSpecialScore?.Invoke(positions, pendingSpecial.Type, _cascadeCounter);
            var positionsToClear = RemovePendingSpecials(positions);
            ComputeMatches?.Invoke(positionsToClear.ToHashSet(),_boardTiles);
            RemovedMatchedTiles(_boardTiles, positionsToClear);
            var movedTilesList = DroppingTiles(positionsToClear, _boardTiles);
            var addedTiles = FillingTiles(_boardTiles);

            return new BoardSequence(positionsToClear, movedTilesList, addedTiles, new List<AddedSpecialTileInfo>(), pendingSpecialPos, pendingPos);
        }

        private List<PendingSpecial> RegisterPendingSpecials(Queue<Vector2Int> positions, Vector2Int? pendingSpecial)
        {
            var specialOriginId = -1;
            var addedPendingSpecials = new List<PendingSpecial>();

            if (pendingSpecial.HasValue)
            {
                var origin = pendingSpecial.Value;
                specialOriginId = _boardTiles[origin.y][origin.x].Id;
            }

            var specialAffectedPositions = new Queue<Vector2Int>(_effectService.GetSpecialAffected(_boardTiles, positions, specialOriginId));
            
            while (specialAffectedPositions.Count > 0)
            {
                var curPos = specialAffectedPositions.Dequeue();
                var curTile = _boardTiles[curPos.y][curPos.x];
                if (!_pendingSpecials.Any(x => x.TileId == curTile.Id))
                {
                    var currentPendingSpecial = new PendingSpecial(curTile.Id, curTile.SpecialType);
                    _pendingSpecials.Enqueue(currentPendingSpecial);
                    addedPendingSpecials.Add(currentPendingSpecial);
                }
            }
            
            return addedPendingSpecials;
        }
        
        private BoardSequence ResolveMatches(DetectedMatches detectedMatches)
        {
            ComputeScore?.Invoke(detectedMatches, _cascadeCounter);
            var effects = _effectService.CreateEffects(detectedMatches, _boardTiles, _movePosition);
            var positions = new HashSet<Vector2Int>(_matchingService.GetMatchedPositions());
            ComputeMatches?.Invoke(positions,_boardTiles);
            var addedPendingSpecial = RegisterPendingSpecials(new Queue<Vector2Int>(positions),null);
            var pendingSpecialPos = GetPendingSpecialPositions(addedPendingSpecial);
            var addedSpecialTileInfo = CreateEffectTiles(_boardTiles, effects, positions);
            var positionsToClear = RemovePendingSpecials(positions);
            RemovedMatchedTiles(_boardTiles, positionsToClear);
            var movedTilesList = DroppingTiles(positionsToClear, _boardTiles);
            var addedTiles = FillingTiles(_boardTiles);

            return new BoardSequence(positionsToClear, movedTilesList, addedTiles, addedSpecialTileInfo, pendingSpecialPos, null);
        }

        private List<Vector2Int> RemovePendingSpecials(IEnumerable<Vector2Int> positions)
        {
            var resultList = new List<Vector2Int>();

            foreach (var curPos in positions)
            {
                var tile = _boardTiles[curPos.y][curPos.x];
                bool isPending = _pendingSpecials.Any(pending => pending.TileId == tile.Id);
                if (isPending)
                    continue;
                
                resultList.Add(curPos);
            }

            return resultList;
        }

        private void EndTurn()
        {
            ResetTurn();
        }

        private void ResetTurn()
        {
            _cascadeCounter = 0;
            _movePosition = null;
            _pendingSpecials.Clear();
            _isTurnActive = false;
        }

        private List<AddedTileInfo> FillingTiles(List<List<Tile>> newBoard)
        {
            // Filling the board
            List<AddedTileInfo> addedTiles = new();
            for (int y = newBoard.Count - 1; y > -1; y--)
            {
                for (int x = newBoard[y].Count - 1; x > -1; x--)
                {
                    if (newBoard[y][x].Type == TileType.None)
                    {
                        int tileIndex = Random.Range(0, _tilesTypes.Count);
                        Tile tile = newBoard[y][x];
                        tile.ChangeId(_tileCount++);
                        tile.ChangeTileType(_tilesTypes[tileIndex]);
                        addedTiles.Add(new AddedTileInfo
                        {
                            Position = new Vector2Int(x, y),
                            Type = tile.Type
                        });
                    }
                }
            }

            return addedTiles;
        }

        private static List<MovedTileInfo> DroppingTiles(IEnumerable<Vector2Int> matchedPosition, List<List<Tile>> newBoard)
        {
            var movedTilesList = new List<MovedTileInfo>();
            var affectedColumns = matchedPosition.Select(pos => pos.x).Distinct();
            
            foreach (var x in affectedColumns)
            {
                var writeY = newBoard.Count - 1;
                
                for (var readY = newBoard.Count -1; readY >= 0; readY--)
                {
                    Tile tile = newBoard[readY][x];
                    
                    if (tile.Type == TileType.None)
                        continue;

                    if (readY != writeY)
                    {
                        newBoard[writeY][x] = tile;
                        movedTilesList.Add(new MovedTileInfo()
                        {
                            From = new Vector2Int(x, readY),
                            To = new Vector2Int(x, writeY)
                        });
                    }

                    writeY--;
                }

                for (var y = writeY; y >= 0; y--)
                {
                    newBoard[y][x] = new Tile(-1, TileType.None, SpecialTileType.None);
                }
            }
            return movedTilesList;
        }

        private void RemovedMatchedTiles(List<List<Tile>> board, IEnumerable<Vector2Int> matchedPosition)
        {
            foreach (var matchedPos in matchedPosition)
                board[matchedPos.y][matchedPos.x] = new Tile(-1, TileType.None, SpecialTileType.None);
        }

        private List<AddedSpecialTileInfo> CreateEffectTiles(List<List<Tile>> board, IEnumerable<IMatchEffect> effects, HashSet<Vector2Int> matchedPositions)
        {
            var specialTileInfos = new List<AddedSpecialTileInfo>();

            foreach (var effect in effects)
            {
                var id = board[effect.Origin.y][effect.Origin.x].Id;
                board[effect.Origin.y][effect.Origin.x] = new Tile(id, effect.TileType, effect.SpecialTileType);
                specialTileInfos.Add(new AddedSpecialTileInfo()
                    { Position = effect.Origin, Type = effect.TileType, SpecialTileType = effect.SpecialTileType });
                matchedPositions.Remove(effect.Origin);
            }

            return specialTileInfos;
        }

        private static List<List<Tile>> CopyBoard(List<List<Tile>> boardToCopy)
        {
            List<List<Tile>> newBoard = new(boardToCopy.Count);
            for (int y = 0; y < boardToCopy.Count; y++)
            {
                newBoard.Add(new List<Tile>(boardToCopy[y].Count));
                for (int x = 0; x < boardToCopy[y].Count; x++)
                {
                    Tile tile = boardToCopy[y][x];
                    newBoard[y].Add(new Tile(tile.Id, tile.Type, tile.SpecialType));
                }
            }

            return newBoard;
        }

        private List<List<Tile>> CreateBoard(int width, int height, List<TileType> tileTypes)
        {
            List<List<Tile>> board = new(height);
            _tileCount = 0;
            for (int y = 0; y < height; y++)
            {
                board.Add(new List<Tile>(width));
                for (int x = 0; x < width; x++)
                {
                    board[y].Add(new Tile(-1, TileType.None, SpecialTileType.None));
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<TileType> noMatchTypes = new(tileTypes.Count);
                    for (int i = 0; i < tileTypes.Count; i++)
                    {
                        noMatchTypes.Add(_tilesTypes[i]);
                    }

                    if (x > 1 &&
                        board[y][x - 1].Type == board[y][x - 2].Type)
                    {
                        noMatchTypes.Remove(board[y][x - 1].Type);
                    }

                    if (y > 1 &&
                        board[y - 1][x].Type == board[y - 2][x].Type)
                    {
                        noMatchTypes.Remove(board[y - 1][x].Type);
                    }

                    board[y][x].ChangeId(_tileCount++);
                    board[y][x].ChangeTileType(noMatchTypes[Random.Range(0, noMatchTypes.Count)]);
                }
            }

            return board;
        }

        private bool TryGetPendingSpecialPosition(PendingSpecial pendingSpecial, out Vector2Int position)
        {
            for (var y = 0; y < _boardTiles.Count; y++)
            {
                for (var x = 0; x < _boardTiles[y].Count; x++)
                {
                    if (_boardTiles[y][x].Id != pendingSpecial.TileId)
                        continue;

                    position = new Vector2Int(x, y);
                    return true;
                }
            }

            position = new Vector2Int(-1, -1);
            return false;
        }

        private List<Vector2Int> GetPendingSpecialPositions(List<PendingSpecial> pendingSpecials)
        {
            var pendingPos = new List<Vector2Int>();

            foreach (var pendingSpecial in pendingSpecials)
            {
                if (TryGetPendingSpecialPosition(pendingSpecial, out var pos))
                    pendingPos.Add(pos);
            }

            return pendingPos;
        }
    }
}
