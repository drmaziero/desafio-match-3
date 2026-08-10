using System.Collections.Generic;
using Gazeus.DesafioMatch3.Core;
using Gazeus.DesafioMatch3.Models;
using NUnit.Framework;

namespace Project.Script.Tests.Matching
{
    public class BasicMatch3Test
    {
        private List<List<Tile>> CreateGenericBoard(int lines, int columns)
        {
            var newboard = new List<List<Tile>>();
            int counter = -1;
            int type = 2;
            
            for (int i = 0; i < lines; i++)
            {
                newboard.Add(new List<Tile>());
                for (int j = 0; j < columns; j++)
                {
                    counter++;
                    type++;
                    newboard[i].Add(new Tile{ Id = counter, Type = type});
                }
            }

            return newboard;
        }

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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
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
            board[0][0].Type = 0;
            board[0][1].Type = 0;
            board[0][2].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
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
            board[0][1].Type = 0;
            board[0][2].Type = 0;
            board[0][3].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
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
            board[0][2].Type = 0;
            board[0][3].Type = 0;
            board[0][4].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
       
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsTrue(matchingService.HasHorizontalMatch());
            Assert.IsFalse(matchingService.HasVerticalMatch());
        }
        
        [Test]
        public void ShouldDetectVerticalMatch()
        {
            /*
                Board
                A  -  -
                A  -  -
                A  -  -  
            */
            var board = CreateGenericBoard(3, 3);
            board[0][0].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
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
            board[0][0].Type = 0;
            board[1][0].Type = 0;
            board[2][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
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
            board[1][0].Type = 0;
            board[2][0].Type = 0;
            board[3][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
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
            board[2][0].Type = 0;
            board[3][0].Type = 0;
            board[4][0].Type = 0;

            var matchingService = new MatchingService();
            matchingService.Init();
            matchingService.FindMatches(board);
            
            Assert.IsTrue(matchingService.HasBasicMatch());
            Assert.IsFalse(matchingService.HasHorizontalMatch());
            Assert.IsTrue(matchingService.HasVerticalMatch());
        }
    }
}
