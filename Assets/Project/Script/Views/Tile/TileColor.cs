using System;
using Models;
using UnityEngine;

namespace Views.Tile
{
    [Serializable]
    public struct TileColor
    {
        public TileType type;
        public Color color;
    }
}