using System;
using Models;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TilePrefabRepository", menuName = "Gameplay/TilePrefabRepository")]
    public class TilePrefabRepository : ScriptableObject
    {
        [SerializeField] private GameObject _tileTypePrefab;
        [SerializeField] private GameObject[] _effectTypePrefabList;

        public GameObject GetTilePrefab()
        {
            return _tileTypePrefab;
        }

        public GameObject GetEffectTilePrefab(SpecialTileType type)
        {
            if (type == SpecialTileType.None)
                throw new ArgumentOutOfRangeException($"Type {type} is not prefab valid");

            return _effectTypePrefabList[(int)type];
        }
    }
}
