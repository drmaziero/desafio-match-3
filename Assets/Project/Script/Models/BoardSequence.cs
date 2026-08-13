using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class BoardSequence
    {
        public List<MovedTileInfo> MovedTiles { get; set; }
        public List<AddedTileInfo> AddedTiles { get; set; }
        public List<AddedSpecialTileInfo> AddedSpecialTiles { get; set; }
        public IEnumerable<Vector2Int> MatchedPosition { get; set; }
    }
}
