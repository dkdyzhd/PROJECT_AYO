using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    [System.Serializable]
    public class PrefabData
    {
        public string Key;
        public GameObject Prefab;
    }

    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance = null;

       public List<PrefabData> Prefabs = new List<PrefabData>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null; 
        }

        public GameObject GetPrefab(string key)
        {
            var targetData = Prefabs.Find(x => x.Key.Equals(key));
            if (targetData != null)
            {
                return targetData.Prefab;
            }

             return null;
        }
    }
}
