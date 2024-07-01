using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipHolder;

        private AyoPlayerController playerController;

        public static EquipManager instance { get; private set; } = null;

        private void Awake()
        {
            instance = this;
        }
    }
}
