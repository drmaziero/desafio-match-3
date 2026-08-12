using GameLogic.Services;
using Models;
using NUnit.Framework;
using Tests.Matching.Interfaces;

namespace Tests.Matching.BasicMatches
{
    public class BasicMatch5Test : BoardFactoryTest, IBasicMatchTests
    {
        [Test]
        public void ShouldDetectHorizontalMatch()
        {
            /*
                Board
                A  A  A  A  A
                -  -  -  -  -
                -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 5);
            board[0][0].Type = TileType.Blue;
            board[0][1].Type = TileType.Blue;
            board[0][2].Type = TileType.Blue;
            board[0][3].Type = TileType.Blue;
            board[0][4].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.HorizontalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnBeginLine()
        {
            /*
                Board
                A  A  A  A  A  -  - 
                -  -  -  -  -  -  -
                -  -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 7);
            board[0][0].Type = TileType.Blue;
            board[0][1].Type = TileType.Blue;
            board[0][2].Type = TileType.Blue;
            board[0][3].Type = TileType.Blue;
            board[0][4].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.HorizontalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnMiddleLine()
        {
            /*
                Board
                -  A  A  A  A  A  -
                -  -  -  -  -  -  -
                -  -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 7);
            board[0][1].Type = TileType.Blue;
            board[0][2].Type = TileType.Blue;
            board[0][3].Type = TileType.Blue;
            board[0][4].Type = TileType.Blue;
            board[0][5].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.HorizontalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnFinalLine()
        {
            /*
                Board
                -  -  A  A  A  A  A 
                -  -  -  -  -  -  -
                -  -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 7);
            board[0][2].Type = TileType.Blue;
            board[0][3].Type = TileType.Blue;
            board[0][4].Type = TileType.Blue;
            board[0][5].Type = TileType.Blue;
            board[0][6].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
       
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.HorizontalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectVerticalMatch()
        {
            /*
                Board
                A  -  -
                A  -  -
                A  -  -  
                A  -  -
                A  -  -
            */
            var board = CreateGenericBoard(5, 3);
            board[0][0].Type = TileType.Blue;
            board[1][0].Type = TileType.Blue;
            board[2][0].Type = TileType.Blue;
            board[3][0].Type = TileType.Blue;
            board[4][0].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.VerticalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectVerticalMatchOnBeginLine()
        {
            /*
                Board
                A  -  - 
                A  -  -
                A  -  -
                A  -  -
                A  -  - 
                -  -  -
                -  -  -
            */
            var board = CreateGenericBoard(7, 3);
            board[0][0].Type = TileType.Blue;
            board[1][0].Type = TileType.Blue;
            board[2][0].Type = TileType.Blue;
            board[3][0].Type = TileType.Blue;
            board[4][0].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.VerticalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectVerticalMatchOnMiddleLine()
        {
            /*
                Board
                -  -  -
                A  -  -
                A  -  -
                A  -  -
                A  -  -
                A  -  -
                -  -  -
            */
            var board = CreateGenericBoard(7, 3);
            board[1][0].Type = TileType.Blue;
            board[2][0].Type = TileType.Blue;
            board[3][0].Type = TileType.Blue;
            board[4][0].Type = TileType.Blue;
            board[5][0].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.VerticalMatchesCounter(),1);
        }
        
        [Test]
        public void ShouldDetectVerticalMatchOnFinalLine()
        {
            /*
                Board
                -  -  -
                -  -  -
                A  -  -
                A  -  -
                A  -  -
                A  -  -
                A  -  -
            */
            var board = CreateGenericBoard(7, 3);
            board[2][0].Type = TileType.Blue;
            board[3][0].Type = TileType.Blue;
            board[4][0].Type = TileType.Blue;
            board[5][0].Type = TileType.Blue;
            board[6][0].Type = TileType.Blue;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
            Assert.AreEqual(matchingService.VerticalMatchesCounter(),1);
        }
    }
}