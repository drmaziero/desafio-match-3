using GameLogic.Services;
using NUnit.Framework;

namespace Tests.Matching
{
    public class ComposedMatchTest : MatchTests
    {
        [Test]
        public void ShouldDetectedMatch3L()
        {
            /*
                A  -  -
                A  -  -
                A  A  A 
             */

            var board = CreateGenericBoard(3, 3);
            board[0][0].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
        }
        
        [Test]
        public void ShouldDetectedMatch3LInverse()
        {
            /*
                A  A  A
                A  -  -
                A  -  -
             */

            var board = CreateGenericBoard(3, 3);
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
        }
        
        [Test]
        public void ShouldDetectedMatch3LReflected()
        {
            /*
                -  -  A
                _  -  A
                A  A  A
             */

            var board = CreateGenericBoard(3, 3);
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
        }
        
        [Test]
        public void ShouldDetectedMatch3LInverseReflected()
        {
            /*
                A  A  A
                -  -  A
                -  -  A
             */

            var board = CreateGenericBoard(3, 3);
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,5);
        }
        
        [Test]
        public void ShouldDetectedMatch3X4L()
        {
            /*
                -  A  -  -
                -  A  -  -
                A  A  A  A 
             */

            var board = CreateGenericBoard(3, 4);
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[0][1].Type = 0;
            board[1][1].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,6);
        }
        
        [Test]
        public void ShouldDetectedMatch3X4LInverse()
        {
            /*
                A  -  -
                A  A  A
                A  -  -
                A  -  -
             */

            var board = CreateGenericBoard(4, 3);
            board[0][0].Type = 0;
            board[1][0].Type = 0;
            board[1][1].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[3][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,6);
        }
        
        [Test]
        public void ShouldDetectedMatch3X4LReflected()
        {
            /*
                -  -  A
                _  -  A
                A  A  A
                -  -  A
             */

            var board = CreateGenericBoard(4, 3);
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[3][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,6);
        }
        
        [Test]
        public void ShouldDetectedMatch3X4LInverseReflected()
        {
            /*
                A  A  A  A
                -  -  A  -
                -  -  A  -
             */

            var board = CreateGenericBoard(3, 4);
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[0][3].Type = 0;
            board[1][2].Type = 0;
            board[2][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,6);
        }
        
        [Test]
        public void ShouldDetectedMatch4X4L()
        {
            /*
                -  A  -  -
                -  A  -  -
                A  A  A  A 
                -  A  -  -
             */

            var board = CreateGenericBoard(4, 4);
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[0][1].Type = 0;
            board[1][1].Type = 0;
            board[3][1].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectedMatch4X4LInverse()
        {
            /*
                -  A  -  -
                A  A  A  A
                -  A  -  -
                -  A  -  -
             */

            var board = CreateGenericBoard(4, 4);
            board[0][1].Type = 0;
            board[1][0].Type = 0;
            board[1][1].Type = 0;
            board[1][2].Type = 0;
            board[1][3].Type = 0;
            board[2][1].Type = 0;
            board[3][1].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectedMatch4X4LReflected()
        {
            /*
                -  -  A  -
                -  -  A  -
                A  A  A  A
                -  -  A  -
             */

            var board = CreateGenericBoard(4, 4);
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[3][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectedMatch4X4LInverseReflected()
        {
            /*
                -  -  A  -
                A  A  A  A
                -  -  A  -
                -  -  A  -
             */

            var board = CreateGenericBoard(4, 4);
            board[0][2].Type = 0;
            board[1][0].Type = 0;
            board[1][1].Type = 0;
            board[1][2].Type = 0;
            board[1][3].Type = 0;
            board[2][2].Type = 0;
            board[3][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchL());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectedMatchT()
        {
            /*
                A  A  A  A  A
                -  -  A  -  -
                -  -  A  -  -
             */

            var board = CreateGenericBoard(3, 5);
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[0][3].Type = 0;
            board[0][4].Type = 0;
            board[1][2].Type = 0;
            board[2][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchT());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
        [Test]
        public void ShouldDetectedMatchTInverse()
        {
            /*
                -  -  A  -  -
                -  -  A  -  -
                A  A  A  A  A
             */

            var board = CreateGenericBoard(3, 5);
            board[0][2].Type = 0;
            board[1][2].Type = 0;
            board[2][0].Type = 0;
            board[2][1].Type = 0;
            board[2][2].Type = 0;
            board[2][3].Type = 0;
            board[2][4].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasComposeMatch());
            Assert.IsTrue(matchingService.HasComposeMatchT());
            Assert.AreEqual(matchingService.GetMatchedPositions().Count,7);
        }
        
    }
}