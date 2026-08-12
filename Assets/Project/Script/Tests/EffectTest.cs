using System.Collections.Generic;
using System.Linq;
using GameLogic.Effects;
using Models;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class EffectTest : BoardFactoryTest
    {
        [Test]
        public void ShouldReturnExpectedPositionOnClearLineEffect()
        {
            /*
                -  -  -  -  - 
                -  -  -  -  - 
                A  A  O  A  A
                -  -  -  -  - 
                -  -  -  -  - 
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ClearLineEffect(new Vector2Int(2, 2));

            var expectedPositions = new HashSet<Vector2Int>
            {
                new(0, 2),
                new(1, 2),
                new(2, 2),
                new(3, 2),
                new(4, 2)
            };

            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
        [Test]
        public void ShouldReturnExpectedPositionOnClearColumnEffect()
        {
            /*
                -  -  A  -  -
                -  -  A  -  -
                -  -  O  -  -
                -  -  A  -  -
                -  -  A  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ClearColumnEffect(new Vector2Int(2, 2));

            var expectedPositions = new HashSet<Vector2Int>
            {
                new(2, 0),
                new(2, 1),
                new(2, 2),
                new(2, 3),
                new(2, 4)
            };

            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }

        [Test]
        public void ShouldReturnExpectedPositionsOnCrossEffect()
        {
            /*
                -  -  A  -  -
                -  -  A  -  -
                A  A  O  A  A
                -  -  A  -  -
                -  -  A  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ClearCrossEffect(new Vector2Int(2, 2));

            var expectedPositions = new HashSet<Vector2Int>
            {
                new(2, 0),
                new(2, 1),
                new(2, 2),
                new(2, 3),
                new(2, 4),
                new(0, 2),
                new(1, 2),
                new(3, 2),
                new(4, 2)
            };

            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }

        [Test]
        public void ShouldReturnExpectedPositionsOnClearColor()
        {
            /*
                -  -  A  -  A
                -  A  -  -  -
                A  -  O  -  A
                -  -  -  -  A
                -  -  A  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            board[2][0].ChangeTileType(TileType.Blue);
            board[4][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[4][2].ChangeTileType(TileType.Blue);
            board[1][3].ChangeTileType(TileType.Blue);
            board[2][4].ChangeTileType(TileType.Blue);
            board[4][4].ChangeTileType(TileType.Blue);

            var effect = new ClearColor(new Vector2Int(2, 2));
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(0, 2),
                new(1, 4),
                new(2, 0),
                new(2, 2),
                new(2, 4),
                new(3, 1),
                new(4, 2),
                new(4, 4)
            };

            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }

        [Test]
        public void ShouldReturnExpectedPositionOnCenterExplosion()
        {
            /*
                -  -  -  -  -
                -  A  A  A  -
                -  A  O  A  -
                -  A  A  A  -
                -  -  -  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ExplosionEffect(new Vector2Int(2, 2), 1);
            
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(1, 1),
                new(1, 2),
                new(1, 3),
                new(2, 1),
                new(2, 2),
                new(2, 3),
                new(3, 1),
                new(3, 2),
                new(3, 3)
            };
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
        [Test]
        public void ShouldReturnExpectedPositionOnTopLeftExplosion()
        {
            /*
                O  A  -  -  -
                A  A  -  -  -
                -  -  -  -  -
                -  -  -  -  -
                -  -  -  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ExplosionEffect(new Vector2Int(4, 0), 1);
            
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(3, 0),
                new(3, 1),
                new(4, 0),
                new(4, 1)
            };
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
        [Test]
        public void ShouldReturnExpectedPositionOnTopRightExplosion()
        {
            /*
                -  -  -  A  O
                -  -  -  A  A
                -  -  -  -  -
                -  -  -  -  -
                -  -  -  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ExplosionEffect(new Vector2Int(4, 4), 1);
            
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(3, 3),
                new(3, 4),
                new(4, 3),
                new(4, 4)
            };
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
        [Test]
        public void ShouldReturnExpectedPositionOnBottomLeftExplosion()
        {
            /*
                -  -  -  -  -
                -  -  -  -  -
                -  -  -  -  -
                A  A  -  -  -
                O  A  -  -  -
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ExplosionEffect(new Vector2Int(0, 0), 1);
            
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(0, 0),
                new(0, 1),
                new(1, 0),
                new(1, 1)
            };
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
        [Test]
        public void ShouldReturnExpectedPositionOnBottomRightExplosion()
        {
            /*
                -  -  -  -  -
                -  -  -  -  -
                -  -  -  -  -
                -  -  -  A  A
                -  -  -  A  O
             */
            
            var board = CreateGenericBoard(5, 5);
            var effect = new ExplosionEffect(new Vector2Int(0, 4), 1);
            
            var expectedPositions = new HashSet<Vector2Int>
            {
                new(0, 3),
                new(0, 4),
                new(1, 3),
                new(1, 4)
            };
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }

        [Test]
        public void ShouldReturnExpectedPositionOnExplosionAndCross()
        {
            /*
               -  -  A  -  -
               -  A  A  A  -
               A  A  O  A  A
               -  A  A  A  -
               -  -  A  -  -
            */
            
            var board = CreateGenericBoard(5, 5);
            
            Vector2Int origin = new Vector2Int(2, 2);
            var effect = new ExplosionAndCleanCrossEffect(origin, 1);

            var explosion = new ExplosionEffect(origin, 1).GetAffectPositions(board);
            var cross = new ClearCrossEffect(origin).GetAffectPositions(board);

            var expectedPositions = explosion.Concat(cross).ToHashSet();
            
            var affectedPositions = effect.GetAffectPositions(board).ToHashSet();
            CollectionAssert.AreEquivalent(expectedPositions, affectedPositions);
        }
        
    }
}