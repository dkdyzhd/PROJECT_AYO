using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class InventoryUI : UIBase
    {
        public static InventoryUI Instance { get; private set; } = null;

        public Image Slot1 => inventorySlots[0];
        public Image Slot2 => inventorySlots[1];
        public Image Slot3 => inventorySlots[2];
        public Image Slot4 => inventorySlots[3];
        public Image Slot5 => inventorySlots[4];
        public Image Slot6 => inventorySlots[5];


        public List<Image> inventorySlots = new List<Image>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AddItem(string itemName)
        {
            var targetItemData = GameDataManager.Instance.ItemDataList.Find(x => x.itemName.Equals(itemName));
            Slot1.sprite = targetItemData.itemImage;

            //if (itemName.Equals("Fish"))
            //{
            //    var targetSprite = Resources.Load<Sprite>("UI/Sprite/Fish");
            //}

            //To do : for문을 이용하여 아이템이 빈자리가 있으면 순서대로 담기도록

        }
    }
}
