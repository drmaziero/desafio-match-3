using System.Collections.Generic;
using Gazeus.DesafioMatch3.Core.Matching;
using Gazeus.DesafioMatch3.Models;

namespace Gazeus.DesafioMatch3.Core
{
    public class MatchingService
    {
        //private List<List<bool>> _matchedTiles;
        private static List<BasicMatch> _horizontalMatches;
        private static List<BasicMatch> _verticalMatches;

        public void Init()
        {
            //_matchedTiles = new List<List<bool>>();
            _horizontalMatches = new List<BasicMatch>();
            _verticalMatches = new List<BasicMatch>();
        }

        private void Reset()
        {
            //_matchedTiles.Clear();
            _horizontalMatches.Clear();
            _verticalMatches.Clear();
        }
        
        public void FindMatches(List<List<Tile>> newBoard)
        {
            Reset();
            
            /*
            for (int y = 0; y < newBoard.Count; y++)
            {
                _matchedTiles.Add(new List<bool>(newBoard[y].Count));
                for (int x = 0; x < newBoard.Count; x++)
                {
                    _matchedTiles[y].Add(false);
                }
            }
            */

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (x > 1 &&
                        newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                        newBoard[y][x - 1].Type == newBoard[y][x - 2].Type)
                    {
                        //_matchedTiles[y][x] = true;
                       // _matchedTiles[y][x - 1] = true;
                        //_matchedTiles[y][x - 2] = true;
                        
                        _horizontalMatches.Add(new HorizontalMatch(y,3,x-2));
                    }

                    if (y > 1 &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
                       // _matchedTiles[y][x] = true;
                       // _matchedTiles[y - 1][x] = true;
                       // _matchedTiles[y - 2][x] = true;
                        
                        _verticalMatches.Add(new VerticalMatch(x,3,y-2));
                    }
                }
            }
        }

        public bool HasHorizontalMatch()
        {
            return _horizontalMatches.Count > 0;
        }

        public bool HasVerticalMatch()
        {
            return _verticalMatches.Count > 0;
        }

        public bool HasBasicMatch()
        {
            return HasHorizontalMatch() || HasVerticalMatch();
        }
    }
}