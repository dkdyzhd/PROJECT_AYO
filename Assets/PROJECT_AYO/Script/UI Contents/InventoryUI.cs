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

        public GameObject quickSlotUI;
        public GameObject inventoryBagUI;

        [SerializeField] private GameObject goSelectedImage;    //���õ� ������ �̹���
        [SerializeField] public QuickSlotUI[] slots;

        //test
        [Header("Selected Item")]
        private QuickSlotUI selectedItem;
        public TextMeshProUGUI selectedItemName;

        public int pickupCount = 0;

#if UNITY_EDITOR
        private void OnValidate()
        {
            slots = GetComponentsInChildren<QuickSlotUI>();
        }
#endif

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            inventoryBagUI.SetActive(false);   // ó������ �κ��丮Bag off
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void OnInventoryBag()
        {
            if (inventoryBagUI.activeInHierarchy)
            {
                inventoryBagUI.SetActive(false);
            }
            else
            {
                inventoryBagUI.SetActive(true);
            }
        }

        public void RefreshSlot(List<QuickSlotData> quickslotItems) // Update �� �������� üũ�ϴ� ���̱� ������ AddItem �Լ� �������� ȣ��
        {
            int i = 0;
            for (; i < quickslotItems.Count && i < slots.Length; i++)
            {
                slots[i].Item = quickslotItems[i].itemData;
                slots[i].Count = quickslotItems[i].count;
            }
            for (; i < slots.Length; i++)
            {
                slots[i].Item = null;
            }
        }

        //Inventory���� Item�� �����ϸ� ��� UI ����
        public void SelectItem(int index)
        {
            //if (slots[index] == null)
            //return;
            //To do : �������� Ŭ���ϸ� �������̸�/�̹���/���� ����
            //selectedItem = slots[index];
            //selectedItemName.text = selectedItem.Item.itemName;

            goSelectedImage.transform.position = slots[index].transform.position;
        }

        public void SetQuickSlotCount(int index, int count)
        {
            slots[index].Count = count;
        }
    }
}
