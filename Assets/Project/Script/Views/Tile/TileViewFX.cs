using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Tile
{
    [Serializable]
    public struct TileViewFX
    {
        public TileFX type;
        public Image image;
        public Animator animator;
        public GameObject root;
        public string animationName;
    }

    public enum TileFX
    {
        None,
        ExplodeTile,
        CreateSpecialTile
    }
}