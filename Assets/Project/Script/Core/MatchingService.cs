using System.Collections.Generic;
using System.Linq;
using Gazeus.DesafioMatch3.Core.Matching;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class MatchingService
    {
        private List<BasicMatch> _horizontalMatches;
        private List<BasicMatch> _verticalMatches;
        private List<ComposedMatch> _composedMatches;
        private List<ComplexMatch> _complexMatches;

        public void Init()
        {
            _horizontalMatches = new List<BasicMatch>();
            _verticalMatches = new List<BasicMatch>();
            _composedMatches = new List<ComposedMatch>();
            _complexMatches = new List<ComplexMatch>();
        }

        private void Reset()
        {
            _horizontalMatches.Clear();
            _verticalMatches.Clear();
            _composedMatches.Clear();
            _complexMatches.Clear();
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
                        AddOrIncreaseHorizontalMatch(y, x);
                    }

                    if (y > 1 &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
                        AddOrIncreaseVerticalMatch(y, x);
                    }
                }
            }
            
            DetectComposeMatch();
            DetectedComplexMatch();

            ShowMatchLogs();
        }

        private void AddOrIncreaseHorizontalMatch(int row, int column)
        {
            var lastMatch = _horizontalMatches.LastOrDefault();
            
            if (lastMatch != null && 
                lastMatch.MainIndex == row && 
                lastMatch.StartIndex + lastMatch.Count == column)
            { 
                lastMatch.Increase();
                return;
            }
            
            _horizontalMatches.Add(new HorizontalMatch(row,3,column-2));
        }
        
        private void AddOrIncreaseVerticalMatch(int row, int column)
        {
            var lastMatch = _verticalMatches.LastOrDefault();
            
            if (lastMatch != null && 
                lastMatch.MainIndex == column && 
                lastMatch.StartIndex + lastMatch.Count == row)
            { 
                lastMatch.Increase();
                return;
            }
            
            _verticalMatches.Add(new VerticalMatch(column,3,row-2));
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

        public bool HasComposeMatch()
        {
            return _composedMatches.Count > 0;
        }

        public bool HasComposeMatchL()
        {
            if (!HasComposeMatch())
                return false;

            return _composedMatches.Any(x => x.IsMatchL());
        }
        
        public bool HasComposeMatchT()
        {
            if (!HasComposeMatch())
                return false;

            return _composedMatches.Any(x => x.IsMatchT());
        }

        public bool HasComplexMatch()
        {
            return _complexMatches.Count > 0;
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

        private void DetectComposeMatch()
        {
            if (!HasBasicMatch())
                return;
            
            foreach (var horizontalMatch in _horizontalMatches)
            {
                foreach (var verticalMatch in _verticalMatches)
                {
                    var notIntersection = new Vector2Int(-1, -1);
                    var intersectionPoint = horizontalMatch.HasIntersection(verticalMatch);
                    if (!intersectionPoint.Equals(notIntersection))
                    {
                        _composedMatches.Add(new ComposedMatch(intersectionPoint,
                            new List<BasicMatch>() { horizontalMatch, verticalMatch }));
                    }
                }
            }
        }

        private void DetectedComplexMatch()
        {
            if (!HasComposeMatch())
                return;

            for (var i = 0; i < _composedMatches.Count - 1; i++)
            {
                for (var j = i + 1; j < _composedMatches.Count; j++)
                {
                    if (_composedMatches[i].HasIntersection(_composedMatches[j]))
                        _complexMatches.Add(new ComplexMatch(new List<ComposedMatch>()
                            { _composedMatches[i], _composedMatches[j] }));
                }
            }
        }
        
        public int HorizontalMatchesCounter()
        {
            return _horizontalMatches.Count;
        }
        
        public int VerticalMatchesCounter()
        {
            return _verticalMatches.Count;
        }
        
        
        //------- Debug
        private void ShowMatchLogs()
        {
            foreach (var horizontalMatch in _horizontalMatches)
                Debug.LogWarning("Horizontal Match!");

            foreach (var verticalMatch in _verticalMatches)
                Debug.LogWarning("Vertical Match!");
            
            foreach (var composedMatch in _composedMatches)
                Debug.LogWarning("Compose Match!");
            
            foreach (var complexMatch in _complexMatches)
                Debug.LogWarning("Complex Match!");
        }
    }
}