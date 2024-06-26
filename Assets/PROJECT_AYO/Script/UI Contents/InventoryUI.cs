using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

        public int pickupCount = 0;
      
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void CheckItem() // Update 는 매프레임 체크하는 것이기 때문에 AddItem 함수 마지막에 호출
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].sprite != null )
                {
                    inventorySlots[i].color = new Color(1, 1, 1, 1);
                }
                else
                {
                    inventorySlots[i].color = new Color(1, 1, 1, 0);
                }
            }
        }
       
        public void AddItem(string itemName)
        {
            //var targetItemData = GameDataManager.Instance.ItemDataList.Find(x => x.itemName.Equals(itemName));
            //Slot1.sprite = targetItemData.itemImage;

            //if (itemName.Equals("Fish"))
            //{
            //    var targetSprite = Resources.Load<Sprite>("UI/Sprite/Fish");
            //}

            //To do : for문을 이용하여 아이템이 빈자리가 있으면 순서대로 담기도록
            var targetItemData = GameDataManager.Instance.ItemDataList.Find(x => x.itemName.Equals(itemName));
            Debug.Log("ItemDataList에서 찾음");

            pickupCount++;

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].sprite == null && i < pickupCount )
                { 
                    inventorySlots[i].sprite = targetItemData.itemImage;
                    Debug.Log("인벤토리에 담김");
                }
            }
            
            CheckItem();
        }
    }
}
