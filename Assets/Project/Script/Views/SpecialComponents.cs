using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    [Serializable]
    public struct SpecialComponents
    {
        public SpecialTileType type;
        public Image liquid;
        public GameObject root;
    }
}