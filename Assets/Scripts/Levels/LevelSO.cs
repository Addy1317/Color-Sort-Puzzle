using UnityEngine;

namespace SlowpokeStudio.Levels
{ 
    [System.Serializable]
    public class LevelDataSO
    {
        [Header("Level Information")]
        public string levelName;
        public int levelIndex;

        [Header("Prefab Reference")]
        public GameObject levelPrefab;

        [Header("Optional Settings")]
        public int coins = 0;
    }

    [CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/Level")]
    public class LevelSO : ScriptableObject
    {
        [Header("List of Levels")]
        [SerializeField] internal LevelDataSO[] levels;
    }
}
