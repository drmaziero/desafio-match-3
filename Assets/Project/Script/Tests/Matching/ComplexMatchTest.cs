using GameLogic.Services;
using Models;
using NUnit.Framework;

namespace Tests.Matching
{
    public class ComplexMatchTest : BoardFactoryTest
    {
        [Test]
        public void ShouldDetectMatchU()
        {
            /*
                A  -  A
                A  -  A
                A  A  A  
             */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectMatchUInverse()
        {
            /*
                A  A  A
                A  -  A
                A  -  A
             */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectMatchC()
        {
            /*
                A  A  A
                A  -  -
                A  A  A
             */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectMatchCInverse()
        {
            /*
                A  A  A
                -  -  A
                A  A  A
             */
            var board = CreateGenericBoard(3, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectMatchH()
        {
            /*
                A  -  A
                A  -  A
                A  A  A
                A  -  A
                A  -  A
             */
            var board = CreateGenericBoard(5, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[3][2].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);
            board[4][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,11);
        }
        
        [Test]
        public void ShouldDetectMatchE()
        {
            /*
                A  A  A
                A  -  -
                A  A  A
                A  -  -
                A  A  A
             */
            var board = CreateGenericBoard(5, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);
            board[4][1].ChangeTileType(TileType.Blue);
            board[4][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,11);
        }
        
        [Test]
        public void ShouldDetectMatchEReflected()
        {
            /*
                A  A  A
                -  -  A
                A  A  A
                -  -  A
                A  A  A
             */
            var board = CreateGenericBoard(5, 3);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[3][2].ChangeTileType(TileType.Blue);
            board[4][0].ChangeTileType(TileType.Blue);
            board[4][1].ChangeTileType(TileType.Blue);
            board[4][2].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,11);
        }
        
        [Test]
        public void ShouldDetectMatchEInverted()
        {
            /*
                A  -  A  -  A
                A  -  A  -  A
                A  A  A  A  A
             */
            var board = CreateGenericBoard(3, 5);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][4].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[1][4].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[2][3].ChangeTileType(TileType.Blue);
            board[2][4].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,11);
        }
        
        [Test]
        public void ShouldDetectMatchEInvertedReflected()
        {
            /*
                A  A  A  A  A
                A  -  A  -  A
                A  -  A  -  A
             */
            var board = CreateGenericBoard(3, 5);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[0][4].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[1][4].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[2][4].ChangeTileType(TileType.Blue);

            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,11);
        }
        
        [Test]
        public void ShouldDetectMatchI()
        {
            /*
               -  A  A  A  -
               -  -  A  -  -
               -  -  A  -  -
               -  A  A  A  -
             */
            var board = CreateGenericBoard(5, 4);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[3][1].ChangeTileType(TileType.Blue);
            board[3][2].ChangeTileType(TileType.Blue);
            board[3][3].ChangeTileType(TileType.Blue);
            
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,8);
        }
        
        [Test]
        public void ShouldDetectMatch4()
        {
            /*
               A  -  A  -
               A  -  A  -
               A  A  A  A
               -  -  A  -
               -  -  A  -
             */
            var board = CreateGenericBoard(5, 4);
            board[0][0].ChangeTileType(TileType.Blue);
            board[0][2].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[2][0].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][2].ChangeTileType(TileType.Blue);
            board[2][3].ChangeTileType(TileType.Blue);
            board[3][2].ChangeTileType(TileType.Blue);
            board[4][2].ChangeTileType(TileType.Blue);
            
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,10);
        }
        
        [Test]
        public void ShouldDetectMatchHashtag()
        {
            /*
               -  A  -  A  -  
               A  A  A  A  A
               -  A  -  A  -  
               A  A  A  A  A
               -  A  -  A  -
             */
            var board = CreateGenericBoard(5, 5);
            board[0][1].ChangeTileType(TileType.Blue);
            board[0][3].ChangeTileType(TileType.Blue);
            board[1][0].ChangeTileType(TileType.Blue);
            board[1][1].ChangeTileType(TileType.Blue);
            board[1][2].ChangeTileType(TileType.Blue);
            board[1][3].ChangeTileType(TileType.Blue);
            board[1][4].ChangeTileType(TileType.Blue);
            board[2][1].ChangeTileType(TileType.Blue);
            board[2][3].ChangeTileType(TileType.Blue);
            board[3][0].ChangeTileType(TileType.Blue);
            board[3][1].ChangeTileType(TileType.Blue);
            board[3][2].ChangeTileType(TileType.Blue);
            board[3][3].ChangeTileType(TileType.Blue);
            board[3][4].ChangeTileType(TileType.Blue);
            board[4][1].ChangeTileType(TileType.Blue);
            board[4][3].ChangeTileType(TileType.Blue);
            
            var matchingService = new MatchingService();
            matchingService.Init();
            var detectedMatches = matchingService.FindMatches(board);
            Assert.IsTrue(detectedMatches.HasComplexMatches);
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,16);
        }
        
    }
}