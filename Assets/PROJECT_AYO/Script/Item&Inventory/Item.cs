using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class Item : MonoBehaviour
    {
        [Header("Weapon/Equip")]
        public ItemData axe;
        public ItemData pickaxe;
        public ItemData ak47;
        public ItemData knife;

        [Header("Resource")]
        public ItemData tree;
        public ItemData rock;
        public ItemData iron;

        [Header("Add Condition")]
        public ItemData water;
        public ItemData syringe;
        public ItemData fish;
    }
}