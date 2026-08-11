using UnityEngine;

namespace ScriptableObjects.Score
{
    [System.Serializable]
    public class CascadeMultiplierEntry
    {
        [Min(1)] public int cascadeCount;

        [Min(1)] public int multiplier;

    }
}