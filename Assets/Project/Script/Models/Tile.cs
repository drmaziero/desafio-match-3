namespace Models
{
    public class Tile
    {
        public int Id { get; private set; }
        public TileType Type { get; private set; }
        public SpecialTileType SpecialType { get; private set; }
        
        public Tile(int id, TileType type, SpecialTileType specialType)
        {
            Id = id;
            Type = type;
            SpecialType = specialType;
        }

        public void ChangeId(int id) => Id = id;
        public void ChangeTileType(TileType type) => Type = type;
        public void ChangeSpecialType(SpecialTileType type) => SpecialType = type;
    }
}
