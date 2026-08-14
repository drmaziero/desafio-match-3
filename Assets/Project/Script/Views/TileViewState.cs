using Models;

namespace Views
{
    public struct TileViewState
    {
        public TileType Type { get; private set; }
        public SpecialTileType SpecialType { get; private set; }
        public bool IsEmpty { get; private set; }

        public TileViewState(TileType type, SpecialTileType specialType)
        {
            Type = type;
            SpecialType = specialType;
            IsEmpty = Type == TileType.None && SpecialType == SpecialTileType.None;
        }
    }
}