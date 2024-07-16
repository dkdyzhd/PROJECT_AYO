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
            inventoryUI.SetActive(false);   // ó������ �κ��丮 off
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

        public void FreshSlot() // Update �� �������� üũ�ϴ� ���̱� ������ AddItem �Լ� �������� ȣ��
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

            //To do : for���� �̿��Ͽ� �������� ���ڸ��� ������ ������� ��⵵��
            var targetItemData = GameDataManager.Instance.ItemDataList.Find(x => x.itemName.Equals(itemName));
            Debug.Log("ItemDataList���� ã��");
            /*
            pickupCount++;
            
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].sprite == null && i < pickupCount )
                { 
                    inventorySlots[i].sprite = targetItemData.itemImage;
                    Debug.Log("�κ��丮�� ���");
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
                Debug.Log("������ ���� ���ֽ��ϴ�");
            }

        }

        //Inventory���� Item�� �����ϸ� ��� UI ����
        public void SelectItem(int index)
        {
            if (slots[index] == null)
                return;
            //To do : �������� Ŭ���ϸ� �������̸�/�̹���/���� ����
            selectedItem = slots[index];
            selectedItemName.text = selectedItem.item.itemName;
        }
    }
}
