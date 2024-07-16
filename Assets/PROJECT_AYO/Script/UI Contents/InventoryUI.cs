using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace AYO
{
    public class InventoryUI : UIBase
    {
        public static InventoryUI Instance { get; private set; } = null;

        public GameObject inventoryUI;
        
        //public Image Slot1 => slots[0].item.itemImage;
        /*
        public Image Slot2 => inventorySlots[1];
        public Image Slot3 => inventorySlots[2];
        public Image Slot4 => inventorySlots[3];
        public Image Slot5 => inventorySlots[4];
        public Image Slot6 => inventorySlots[5];
        */

        //public List<Image> inventorySlots = new List<Image>();
        //test
        public List<Item> items;
        [SerializeField]
        public SlotUI[] slots;
       
        //test
        [Header("Selected Item")]
        private SlotUI selectedItem;
        public TextMeshProUGUI selectedItemName;

        public int pickupCount = 0;

#if UNITY_EDITOR
        private void OnValidate()
        {
            slots = GetComponentsInChildren<SlotUI>();
        }
#endif

        private void Awake()
        {
            Instance = this;
            
        }

        private void Start()
        {
            inventoryUI.SetActive(false);   // 처음에는 인벤토리 off
            FreshSlot();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void OnInventory()
        {
            if(inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
            }

            else
            {
                inventoryUI.SetActive(true);
            }
        }

        public void FreshSlot() // Update 는 매프레임 체크하는 것이기 때문에 AddItem 함수 마지막에 호출
        {
            /*for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].sprite != null )
                {
                    inventorySlots[i].color = new Color(1, 1, 1, 1);
                }
                else
                {
                    inventorySlots[i].color = new Color(1, 1, 1, 0);
                }
            }*/

            int i = 0;
            for (; i < items.Count && i < slots.Length; i++)
            {
                slots[i].item = items[i];
            }
            for (; i < slots.Length; i++)
            {
                slots[i].item = null;
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
            /*
            pickupCount++;
            
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].sprite == null && i < pickupCount )
                { 
                    inventorySlots[i].sprite = targetItemData.itemImage;
                    Debug.Log("인벤토리에 담김");
                }
            }*/
            //test
            if(items.Count < slots.Length)
            {
                items.Add(targetItemData);
                FreshSlot();
            }
            else
            {
                Debug.Log("슬롯이 가득 차있습니다");
            }

        }

        //Inventory에서 Item을 선택하면 띄울 UI 셋팅
        public void SelectItem(int index)
        {
            if (slots[index] == null)
                return;
            //To do : 아이템을 클릭하면 아이템이름/이미지/정보 띄우기
            selectedItem = slots[index];
            selectedItemName.text = selectedItem.item.itemName;
        }
    }
}
