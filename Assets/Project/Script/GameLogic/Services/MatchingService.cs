using System.Collections.Generic;
using System.Linq;
using GameLogic.Matching;
using Models;
using UnityEngine;

namespace GameLogic.Services
{
    public class MatchingService
    {
        public List<BasicMatch> HorizontalMatches { get; private set; }
        public List<BasicMatch> VerticalMatches { get; private set; }
        public List<ComposedMatch> ComposedMatches { get; private set; }
        public List<ComplexMatch> ComplexMatches { get; private set; }

        public void Init()
        {
            HorizontalMatches = new List<BasicMatch>();
            VerticalMatches = new List<BasicMatch>();
            ComposedMatches = new List<ComposedMatch>();
            ComplexMatches = new List<ComplexMatch>();
        }

        private void Reset()
        {
            HorizontalMatches.Clear();
            VerticalMatches.Clear();
            ComposedMatches.Clear();
            ComplexMatches.Clear();
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
            var lastMatch = HorizontalMatches.LastOrDefault();
            
            if (lastMatch != null && 
                lastMatch.MainIndex == row && 
                lastMatch.StartIndex + lastMatch.Count == column)
            { 
                lastMatch.Increase();
                return;
            }
            
            HorizontalMatches.Add(new HorizontalMatch(row,3,column-2));
        }
        
        private void AddOrIncreaseVerticalMatch(int row, int column)
        {
            var lastMatch = VerticalMatches.LastOrDefault();
            
            if (lastMatch != null && 
                lastMatch.MainIndex == column && 
                lastMatch.StartIndex + lastMatch.Count == row)
            { 
                lastMatch.Increase();
                return;
            }
            
            VerticalMatches.Add(new VerticalMatch(column,3,row-2));
        }

        public bool HasHorizontalMatch()
        {
            return HorizontalMatches.Count > 0;
        }

        public bool HasVerticalMatch()
        {
            return VerticalMatches.Count > 0;
        }

        public bool HasBasicMatch()
        {
            return HasHorizontalMatch() || HasVerticalMatch();
        }

        public bool HasComposeMatch()
        {
            return ComposedMatches.Count > 0;
        }

        public bool HasComposeMatchL()
        {
            if (!HasComposeMatch())
                return false;

            return ComposedMatches.Any(x => x.IsMatchL());
        }
        
        public bool HasComposeMatchT()
        {
            if (!HasComposeMatch())
                return false;

            return ComposedMatches.Any(x => x.IsMatchT());
        }

        public bool HasComplexMatch()
        {
            return ComplexMatches.Count > 0;
        }

        public List<Vector2Int> GetMatchedPositions()
        {
            var allMatchedPositions = new HashSet<Vector2Int>();

            foreach (var position in HorizontalMatches.SelectMany(horizontalMatch => horizontalMatch.GetSequencePosition()))
                allMatchedPositions.Add(position);
            
            foreach (var position in VerticalMatches.SelectMany(verticalMatch => verticalMatch.GetSequencePosition()))
                allMatchedPositions.Add(position);

            return new List<Vector2Int>(allMatchedPositions);
        }

        private void DetectComposeMatch()
        {
            if (!HasBasicMatch())
                return;
            
            foreach (var horizontalMatch in HorizontalMatches)
            {
                foreach (var verticalMatch in VerticalMatches)
                {
                    var notIntersection = new Vector2Int(-1, -1);
                    var intersectionPoint = horizontalMatch.HasIntersection(verticalMatch);
                    if (!intersectionPoint.Equals(notIntersection))
                    {
                        ComposedMatches.Add(new ComposedMatch(intersectionPoint,
                            new List<BasicMatch>() { horizontalMatch, verticalMatch }));
                    }
                }
            }
        }

        private void DetectedComplexMatch()
        {
            if (!HasComposeMatch())
                return;

            for (var i = 0; i < ComposedMatches.Count - 1; i++)
            {
                for (var j = i + 1; j < ComposedMatches.Count; j++)
                {
                    if (ComposedMatches[i].HasIntersection(ComposedMatches[j]))
                        ComplexMatches.Add(new ComplexMatch(new List<ComposedMatch>()
                            { ComposedMatches[i], ComposedMatches[j] }));
                }
            }
        }
        
        public int HorizontalMatchesCounter()
        {
            return HorizontalMatches.Count;
        }
        
        public int VerticalMatchesCounter()
        {
            return VerticalMatches.Count;
        }
        
        
        //------- Debug
        private void ShowMatchLogs()
        {
            foreach (var horizontalMatch in HorizontalMatches)
                Debug.LogWarning("Horizontal Match!");

            foreach (var verticalMatch in VerticalMatches)
                Debug.LogWarning("Vertical Match!");
            
            foreach (var composedMatch in ComposedMatches)
                Debug.LogWarning("Compose Match!");
            
            foreach (var complexMatch in ComplexMatches)
                Debug.LogWarning("Complex Match!");
        }
    }
}