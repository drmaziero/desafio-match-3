using System;
using System.Collections.Generic;
using Models;

namespace ScriptableObjects.Level
{
    [Serializable]
    public class TargetLevelSettings
    {
        public int score;
        public List<TileCounterSettings> tileSettings;
        public List<SpecialCounterSettings> specialSettings;
    }

    [Serializable]
    public class TileCounterSettings
    {
        public TileType type;
        public int count;
    }

    [Serializable]
    public class SpecialCounterSettings
    {
        public SpecialTileType type;
        public int count;
    }
}