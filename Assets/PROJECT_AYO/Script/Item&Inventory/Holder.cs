using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class Holder : SingletonBase<Holder>
    {
        public List<ItemData> ItemDatas = new List<ItemData>();

        public WeaponItemData GetWeaponItemData(ItemType type)
        {
            var targetItemData = ItemDatas.Find(x => x.itemType == type);
            if(targetItemData is WeaponItemData)
            {
                return targetItemData as WeaponItemData;
            }

            return null;
        }
    }
}
