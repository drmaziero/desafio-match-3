using System.Collections.Generic;
using System.Linq;
using Gazeus.DesafioMatch3.Core.Matching;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class MatchingService
    {
        private static List<BasicMatch> _horizontalMatches;
        private static List<BasicMatch> _verticalMatches;

        public void Init()
        {
            _horizontalMatches = new List<BasicMatch>();
            _verticalMatches = new List<BasicMatch>();
        }

        private void Reset()
        {
            _horizontalMatches.Clear();
            _verticalMatches.Clear();
        }
        
        public void FindMatches(List<List<Tile>> newBoard)
        {
            Reset();

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (x > 1 &&
                        newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                        newBoard[y][x - 1].Type == newBoard[y][x - 2].Type)
                    {
                        _horizontalMatches.Add(new HorizontalMatch(y,3,x-2));
                    }

                    if (y > 1 &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
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

        public List<Vector2Int> GetMatchedPositions()
        {
            var allMatchedPositions = new HashSet<Vector2Int>();

            foreach (var position in _horizontalMatches.SelectMany(horizontalMatch => horizontalMatch.GetSequencePosition()))
                allMatchedPositions.Add(position);
            
            foreach (var position in _verticalMatches.SelectMany(verticalMatch => verticalMatch.GetSequencePosition()))
                allMatchedPositions.Add(position);

            return new List<Vector2Int>(allMatchedPositions);
        }
    }
}