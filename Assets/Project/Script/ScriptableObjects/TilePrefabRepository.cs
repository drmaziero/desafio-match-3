using System;
using Models;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TilePrefabRepository", menuName = "Gameplay/TilePrefabRepository")]
    public class TilePrefabRepository : ScriptableObject
    {
        [SerializeField] private GameObject[] _tileTypePrefabList;

        public GameObject GetTilePrefab(TileType type)
        {
            if (type == TileType.None)
                throw new ArgumentOutOfRangeException($"Type {type} is not prefab valid");
                
            return _tileTypePrefabList[(int)type];
        }
    }
}
