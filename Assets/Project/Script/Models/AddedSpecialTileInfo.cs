using UnityEngine;

namespace Models
{
    public struct AddedSpecialTileInfo
    {
        public Vector2Int Position { get; set; }
        public TileType Type { get; set; }
        public SpecialTileType SpecialTileType { get; set; }
    }
}