using System.Collections.Generic;
using Models;
using UnityEngine;

namespace GameLogic.Effects
{
    public class ClearColor : IMatchEffect
    {
        public Vector2Int Origin { get; }

        public ClearColor(Vector2Int origin)
        {
            Origin = origin;
        }

        public IEnumerable<Vector2Int> GetAffectPositions(IReadOnlyList<IReadOnlyList<Tile>> board)
        {
            for (int y = 0; y < board.Count; y++)
            {
                int targetType = board[Origin.y][Origin.x].Type;
                
                for (int x = 0; x < board[y].Count; x++)
                {
                    if (board[y][x].Type == targetType)
                        yield return new Vector2Int(x, y);
                }
            }
        }
    }
}