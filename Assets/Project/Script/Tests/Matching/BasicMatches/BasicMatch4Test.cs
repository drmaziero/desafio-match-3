using GameLogic.Services;
using Models;
using NUnit.Framework;
using Tests.Matching.Interfaces;

namespace Tests.Matching.BasicMatches
{
    public class BasicMatch4Test : BoardFactoryTest, IBasicMatchTests
    {
        [Test]
        public void ShouldDetectHorizontalMatch()
        {
            /*
                Board
                A  A  A  A
                -  -  -  -
                -  -  -  -
            */
            var board = CreateGenericBoard(3, 4);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.HorizontalMatchesCount,1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnBeginLine()
        {
            /*
                Board
                A  A  A  A  -  -
                -  -  -  -  -  -
                -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 6);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.HorizontalMatchesCount,1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnMiddleLine()
        {
            /*
                Board
                -  A  A  A  A  -
                -  -  -  -  -  -
                -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 6);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[0][4].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.HorizontalMatchesCount,1);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnFinalLine()
        {
            /*
                Board
                -  -  A  A  A  A 
                -  -  -  -  -  -
                -  -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 6);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[0][4].ChangeTileType(TileType.Blue);
            board[0][5].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.HorizontalMatchesCount,1);
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
            */
            var board = CreateGenericBoard(4, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.VerticalMatchesCount,1);
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
                -  -  - 
                -  -  -
            */
            var board = CreateGenericBoard(6, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.VerticalMatchesCount,1);
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
                -  -  -
            */
            var board = CreateGenericBoard(6, 3);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.VerticalMatchesCount,1);
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
            */
            var board = CreateGenericBoard(6, 3);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);
            board[5][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,4);
            Assert.AreEqual(detectedMatches.VerticalMatchesCount,1);
        }
    }
}