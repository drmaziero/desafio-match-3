using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class BoardSequence
    {
        public IEnumerable<Vector2Int> MatchedPosition { get; set; }
        public List<MovedTileInfo> MovedTiles { get; set; }
        public List<AddedTileInfo> AddedTiles { get; set; }
        public List<AddedSpecialTileInfo> AddedSpecialTiles { get; set; }

        public List<Vector2Int> PendingSpecials { get; set; }
        public Vector2Int? ActivatedSpecial { get; set; }

        public BoardSequence( IEnumerable<Vector2Int> matchedPosition, 
            List<MovedTileInfo> movedTiles, 
            List<AddedTileInfo> addedTiles,
            List<AddedSpecialTileInfo> addedSpecialTiles,
            List<Vector2Int> pendingSpecials, 
            Vector2Int? activatedSpecial)
        {
            MovedTiles = movedTiles;
            AddedTiles = addedTiles;
            AddedSpecialTiles = addedSpecialTiles;
            MatchedPosition = matchedPosition;
            PendingSpecials = pendingSpecials;
            ActivatedSpecial = activatedSpecial;
        }
        
    }
}
