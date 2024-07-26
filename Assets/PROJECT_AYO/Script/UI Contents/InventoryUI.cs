using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class InventoryUI : UIBase
    {
        public static InventoryUI Instance { get; private set; } = null;

        public GameObject inventoryUI;

        [SerializeField] private GameObject goSelectedImage;    //선택된 퀵슬롯 이미지
        [SerializeField] public SlotUI[] slots;
       
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

        public void RefreshSlot(List<QuickSlotData> quickslotItems) // Update 는 매프레임 체크하는 것이기 때문에 AddItem 함수 마지막에 호출
        {
            int i = 0;
            for (; i < quickslotItems.Count && i < slots.Length; i++)
            {
                slots[i].Item = quickslotItems[i].itemData;
            }
            for (; i < slots.Length; i++)
            {
                slots[i].Item = null;
            }
        }

        //Inventory에서 Item을 선택하면 띄울 UI 셋팅
        public void SelectItem(int index)
        {
            //if (slots[index] == null)
                //return;
            //To do : 아이템을 클릭하면 아이템이름/이미지/정보 띄우기
            //selectedItem = slots[index];
            //selectedItemName.text = selectedItem.Item.itemName;

            goSelectedImage.transform.position = slots[index].transform.position;
        }
    }
}
