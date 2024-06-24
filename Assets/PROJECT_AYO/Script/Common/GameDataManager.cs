using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; } = null;

        public List<Item> ItemDataList = new List<Item>();



        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
