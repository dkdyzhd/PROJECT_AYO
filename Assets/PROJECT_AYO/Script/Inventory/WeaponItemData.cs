using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public enum WeaponType
    {
        None = 0,
        Axe = 1,
        PickAxe = 2,
    }

    [CreateAssetMenu]
    public class WeaponItemData : ItemData
    {
        public WeaponType weaponType;
    }
}
