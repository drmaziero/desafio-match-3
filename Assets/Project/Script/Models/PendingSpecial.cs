namespace Models
{
    public class PendingSpecial
    {
        public int TileId { get; private set; }
        public SpecialTileType Type { get; private set; }

        public PendingSpecial(int tileId, SpecialTileType type)
        {
            TileId = tileId;
            Type = type;
        }
    }
}