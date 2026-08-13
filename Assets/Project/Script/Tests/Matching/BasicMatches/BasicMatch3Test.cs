using GameLogic.Services;
using Models;
using NUnit.Framework;
using Tests.Matching.Interfaces;

namespace Tests.Matching.BasicMatches
{
    public class BasicMatch3Test : BoardFactoryTest, IBasicMatchTests
    {
        [Test]
        public void ShouldDetectHorizontalMatch()
        {
            /*
                Board
                A  A  A
                -  -  -
                -  -  -  
            */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnBeginLine()
        {
            /*
                Board
                A  A  A  -  - 
                -  -  -  -  -
                -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 5);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnMiddleLine()
        {
            /*
                Board
                -  A  A  A  -
                -  -  -  -  -
                -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 5);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
        }
        
        [Test]
        public void ShouldDetectHorizontalMatchOnFinalLine()
        {
            /*
                Board
                -  -  A  A  A 
                -  -  -  -  -
                -  -  -  -  -
            */
            var board = CreateGenericBoard(3, 5);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[0][4].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
       
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsTrue(detectedMatches.HasHorizontalMatched);
            Assert.IsFalse(detectedMatches.HasVerticalMatched);
        }
        
        [Test]
        public  void ShouldDetectVerticalMatch()
        {
            /*
                Board
                A  -  -
                A  -  -
                A  -  -  
            */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
        }
        
        [Test]
        public void ShouldDetectVerticalMatchOnBeginLine()
        {
            /*
                Board
                A  -  - 
                A  -  -
                A  -  -
                -  -  -
                -  -  - 
            */
            var board = CreateGenericBoard(5, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
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
                -  -  -
            */
            var board = CreateGenericBoard(5, 3);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
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
            */
            var board = CreateGenericBoard(5, 3);
            board[2][0].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            
            Assert.IsTrue(detectedMatches.HasBasicMatches);
            Assert.IsFalse(detectedMatches.HasHorizontalMatched);
            Assert.IsTrue(detectedMatches.HasVerticalMatched);
        }
    }
}
