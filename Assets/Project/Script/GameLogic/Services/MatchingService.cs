using System.Collections.Generic;
using System.Linq;
using GameLogic.Matching;
using Models;
using UnityEngine;

namespace GameLogic.Services
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
        
        public DetectedMatches FindMatches(List<List<Tile>> newBoard)
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

            ShowMatchLogs(); // Debug

            return new DetectedMatches(_complexMatches, _composedMatches, _horizontalMatches.Concat(_verticalMatches));
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

        private bool HasHorizontalMatch()
        {
            return _horizontalMatches.Count > 0;
        }

        private bool HasVerticalMatch()
        {
            return _verticalMatches.Count > 0;
        }

        private bool HasBasicMatch()
        {
            return HasHorizontalMatch() || HasVerticalMatch();
        }

        private bool HasComposeMatch()
        {
            return _composedMatches.Count > 0;
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

            var visited = new HashSet<ComposedMatch>();
            
            foreach (var composedMatch in _composedMatches)
            {
                if (visited.Contains(composedMatch))
                    continue;

                var connectedMatches = GetConnectedMatches(composedMatch, visited);
                
                if (connectedMatches.Count > 1)
                    _complexMatches.Add(new ComplexMatch(connectedMatches));
            }
        }

        private List<ComposedMatch> GetConnectedMatches(ComposedMatch initMatch, HashSet<ComposedMatch> visitedMatches)
        {
            var result = new List<ComposedMatch>();
            var pending = new Queue<ComposedMatch>();
            
            pending.Enqueue(initMatch);
            visitedMatches.Add(initMatch);

            while (pending.Count > 0)
            {
                var currentMatch = pending.Dequeue();
                result.Add(currentMatch);
                
                foreach (var candidateMatch in _composedMatches)
                {
                    if (visitedMatches.Contains(candidateMatch))
                        continue;
                    
                    if (!currentMatch.HasIntersection(candidateMatch))
                        continue;

                    visitedMatches.Add(candidateMatch);
                    pending.Enqueue(candidateMatch);
                }
            }

            return result;
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