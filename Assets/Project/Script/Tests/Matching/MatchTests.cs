using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;

namespace Project.Script.Tests.Matching
{
    public abstract class MatchTests
    {
         protected List<List<Tile>> CreateGenericBoard(int lines, int columns)
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
        
    }
}