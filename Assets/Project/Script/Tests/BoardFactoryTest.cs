using System.Collections.Generic;
using Models;

namespace Tests
{
    public abstract class BoardFactoryTest
    {
         protected List<List<Tile>> CreateGenericBoard(int lines, int columns)
        {
            var newboard = new List<List<Tile>>();
            int counter = -1;
            TileType[] types =
            {
                TileType.Green,
                TileType.Orange,
                TileType.Yellow,
                TileType.Pink,
                TileType.Purple,
                TileType.Red
            };
            
            for (int i = 0; i < lines; i++)
            {
                newboard.Add(new List<Tile>());
                for (int j = 0; j < columns; j++)
                {
                    counter++;
                    newboard[i].Add(new Tile(counter,types[(i + j) % types.Length],SpecialTileType.None));
                }
            }

            return newboard;
        }
    }
}