using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Effects;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.Services
{
    public class GameService
    {
        private List<List<Tile>> _boardTiles;
        private List<TileType> _tilesTypes;
        private int _tileCount;
        private MatchingService _matchingService;
        private MatchEffectService _effectService;
        
        public ScoreService ScoreService { get; private set; }

        public GameService(ScoreConfig scoreConfig)
        {
            _matchingService = new MatchingService();
            _effectService = new MatchEffectService();
            
            ScoreService = new ScoreService(scoreConfig);
            
        }
        public List<List<Tile>> StartGame(int boardWidth, int boardHeight)
        {
            _tilesTypes = new List<TileType> { TileType.Blue, TileType.Green, TileType.Orange, TileType.Yellow };
            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);
            
            _matchingService.Init();
            ScoreService.Init();
            
            return _boardTiles;
        }
        
        public bool IsValidMovement(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            if (_matchingService.HasMatchWithPosition(newBoard, new Vector2Int(fromX, fromY), newBoard[fromY][fromX].Type))
                return true;

            if (_matchingService.HasMatchWithPosition(newBoard, new Vector2Int(toX, toY), newBoard[toY][toX].Type))
                return true;

            if (newBoard[toY][toX].SpecialType != SpecialTileType.None ||
                newBoard[fromY][fromX].SpecialType != SpecialTileType.None)
                return true;

            return false;
        }

        public List<BoardSequence> SwapTile(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            List<BoardSequence> boardSequences = new();

            var detectedMatches = _matchingService.FindMatches(newBoard);
            var effectTiles = _effectService.GetEffectOnTiles(newBoard,
                new List<Vector2Int>() { new(fromX, fromY), new(toX, toY) });
            
            int cascadeCounter = 1;
            
            while (detectedMatches.HasBasicMatches || effectTiles.Any())
            {
                ScoreService.ComputeScore(detectedMatches, cascadeCounter);

                Vector2Int? movedPosition = cascadeCounter == 1 ? new Vector2Int(toX, toY) : null;
                var effects = _effectService.CreateEffects(detectedMatches, newBoard, movedPosition);

                var initPositions = new HashSet<Vector2Int>();
                
                if (detectedMatches.HasBasicMatches)
                    initPositions.UnionWith(_matchingService.GetMatchedPositions());
               
                if (effectTiles.Any())
                    initPositions.UnionWith(_effectService.GetEffectsPositions(newBoard, effectTiles));

                effectTiles.Clear();
                var matchedPosition = _effectService.ResolveEffectCascate(newBoard, initPositions);

                var addedSpecialTileInfo = CreateEffectTiles(newBoard, effects, matchedPosition);
                RemovedMatchedTiles(newBoard, matchedPosition);

                var movedTilesList = DroppingTiles(matchedPosition, newBoard);
                var addedTiles = FillingTiles(newBoard);

                BoardSequence sequence = new()
                {
                    MatchedPosition = matchedPosition,
                    MovedTiles = movedTilesList,
                    AddedTiles = addedTiles,
                    AddedSpecialTiles = addedSpecialTileInfo
                };
                boardSequences.Add(sequence);
                detectedMatches = _matchingService.FindMatches(newBoard);

                cascadeCounter++;
            }

            _boardTiles = newBoard;

            return boardSequences;
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
            // Dropping the tiles
            Dictionary<int, MovedTileInfo> movedTiles = new();
            List<MovedTileInfo> movedTilesList = new();
            foreach (var position in matchedPosition)
            {
                int x = position.x;
                int y = position.y;
                if (y > 0)
                {
                    for (int j = y; j > 0; j--)
                    {
                        Tile movedTile = newBoard[j - 1][x];
                        newBoard[j][x] = movedTile;
                        if (movedTile.Type != TileType.None)
                        {
                            if (movedTiles.ContainsKey(movedTile.Id))
                            {
                                movedTiles[movedTile.Id].To = new Vector2Int(x, j);
                            }
                            else
                            {
                                MovedTileInfo movedTileInfo = new()
                                {
                                    From = new Vector2Int(x, j - 1),
                                    To = new Vector2Int(x, j)
                                };
                                movedTiles.Add(movedTile.Id, movedTileInfo);
                                movedTilesList.Add(movedTileInfo);
                            }
                        }
                    }

                    newBoard[0][x] = new Tile(-1, TileType.None, SpecialTileType.None);
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
    }
}
