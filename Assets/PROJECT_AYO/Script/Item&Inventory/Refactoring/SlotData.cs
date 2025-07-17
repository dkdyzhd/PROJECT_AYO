using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

namespace AYO
{
    [System.Serializable]
    public class SlotData
    {
        private int count;
        private InteractionItem item;

        //private ItemData slotItemData;
        //private UltEvent useItem;
        public void SetSlotItemCount(int i)
        {
            count += i;
        }
        public int GetItemCount()
        {
            return count;
        }

        public void SetSlotItem(InteractionItem interactionItem)
        {
            item = interactionItem;
        }

        public void GetItemUse()
        {

        }
        public InteractionItem GetItem()
        {
            return item;
        }
        public ItemData GetItemData()
        {
            return item.ItemData;
        }
    }
}
