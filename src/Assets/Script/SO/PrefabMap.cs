using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace GrowTopia.Data
{

    [CreateAssetMenu(fileName = "New Prefab Map", menuName = "SO/Prefabs/Prefab Map")]
    public class PrefabMap : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<string, PrefabPack> _prefabMap = new SerializedDictionary<string, PrefabPack>();
        public IReadOnlyDictionary<string, PrefabPack> Prefabs => _prefabMap;
    }

    [System.Serializable]
    public struct PrefabPack
    {
        public string PrefabName;
        public GameObject Prefab;
    }
}
