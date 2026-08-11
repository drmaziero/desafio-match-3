using GameLogic.Services;
using NUnit.Framework;

namespace Tests.Matching
{
    public class ComplexMatchTest : MatchTests
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
            board[0][0].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[3][0].Type = 0;
            board[3][2].Type = 0;
            board[4][0].Type = 0;
            board[4][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[3][0].Type = 0;
            board[4][0].Type = 0;
            board[4][1].Type = 0;
            board[4][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[3][2].Type = 0;
            board[4][0].Type = 0;
            board[4][1].Type = 0;
            board[4][2].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][2].Type = 0;
            board[0][4].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[1][4].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[2][4].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[0][3].Type = 0;
            board[0][4].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[1][4].Type = 0;
            board[2][0].Type = 0;
            board[2][2].Type = 0;
            board[2][4].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[0][3].Type = 0;
            board[1][2].Type = 0;
            board[2][2].Type = 0;
            board[3][1].Type = 0;
            board[3][2].Type = 0;
            board[3][3].Type = 0;
            
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][0].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[3][2].Type = 0;
            board[4][2].Type = 0;
            
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
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
            board[0][1].Type = 0;
            board[0][3].Type = 0;
            board[1][0].Type = 0;
            board[1][1].Type = 0;
            board[1][2].Type = 0;
            board[1][3].Type = 0;
            board[1][4].Type = 0;
            board[2][1].Type = 0;
            board[2][3].Type = 0;
            board[3][0].Type = 0;
            board[3][1].Type = 0;
            board[3][2].Type = 0;
            board[3][3].Type = 0;
            board[3][4].Type = 0;
            board[4][1].Type = 0;
            board[4][3].Type = 0;
            
            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComplexMatch());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,16);
        }
        
    }
}