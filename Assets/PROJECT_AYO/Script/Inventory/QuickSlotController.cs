using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    [System.Serializable]
    public class QuickSlotData
    {
        public ItemData itemData;
        public int count;
    }

    public class QuickSlotController : MonoBehaviour
    {
        public static QuickSlotController Instance { get; private set; } = null;

        private int selectedSlot;   //선택된 퀵슬롯의 인덱스
        [SerializeField] private GameObject holder;

        private WeaponItemData weaponData;
        private GameObject currentEquipWeapon = null;
        private WeaponItemData currentEquipWeaponData = null;

        public List<QuickSlotData> quickSlotDatas = new List<QuickSlotData>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            selectedSlot = 0;
            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }

        private void Update()
        {
            TryInputNumber();
        }

        private void TryInputNumber()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeSlot(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeSlot(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeSlot(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeSlot(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChangeSlot(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChangeSlot(5);
            }
        }

        private void ChangeSlot(int slotnum)
        {
            SelectedSlot(slotnum);
            UseItem();
        }

        private void SelectedSlot(int slotnum)
        {
            selectedSlot = slotnum; //선택된 슬롯
            InventoryUI.Instance.SelectItem(slotnum);
        }

        private void OnHolder()
        {
            currentEquipWeaponData = weaponData;    //새로 받아온 데이터 입력

            //To do : Holder에 들게하기
            GameObject newWeapon = Instantiate(weaponData.itemPrefab);  //새로 받아온데이터의 itemPrefab 생성
            newWeapon.transform.parent = holder.transform;
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.Euler(weaponData.rotation);

            currentEquipWeapon = newWeapon;
        }
        private void RemoveWeapon()
        {
            if (currentEquipWeapon != null)     //들고있는 무기가 있다면
            {
                Destroy(currentEquipWeapon);    //무기 삭제
                currentEquipWeapon = null;  //null값으로 만들기
            }
            currentEquipWeaponData = null;
        }

        private void Eat()
        {
            AyoPlayerController.Instance.animator.SetTrigger("Trigger_Eat");
            quickSlotDatas[selectedSlot].itemData = null;

            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }

        private void UseItem()
        {
            if (selectedSlot >= quickSlotDatas.Count)
            {
                if (currentEquipWeaponData != null) //현재 무기데이터가 있고
                {
                    RemoveWeapon();
                }
                return;
            }

            bool isExistItemData = quickSlotDatas[selectedSlot].itemData != null;
            if (isExistItemData)  //퀵슬롯에 아이템이 있다면
            {
                //아이템타입이 Weapon이라면
                if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Weapon)
                {
                    if (currentEquipWeaponData != null) //현재 무기데이터가 있고
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        if (currentEquipWeaponData != weaponData)   //새로 받아온 무기아이템데이터와 같지 않고
                        {
                            RemoveWeapon();
                            OnHolder();
                        }
                        else    //새로 받아온 무기아이템데이터가 같다면
                        {
                            RemoveWeapon(); //무기 내려놓기

                            //currentEquipWeaponData = null;
                        }
                    }
                    else    //현재 무기데이터가 없다면
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        OnHolder();
                    }
                }
                //아이템타입이 Food라면
                else if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Food)
                {
                    //To do : 맨손 & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat();

                }
                else
                {
                    //To do : 맨손 -> (Holder SetActive(false))
                    RemoveWeapon();
                }
            }
            else
            {
                //To do : 맨손
                RemoveWeapon();
            }
        }

        public void AddItem(ItemData itemData)
        {
            for (int i = 0; i < quickSlotDatas.Count; i++)
            {
                if (quickSlotDatas[i].itemData == null)
                {
                    quickSlotDatas[i].itemData = itemData;
                    InventoryUI.Instance.RefreshSlot(quickSlotDatas);
                    return;
                }
            }

            var newQuickSlotData = new QuickSlotData() { itemData = itemData, count = 1, };
            quickSlotDatas.Add(newQuickSlotData);
            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }
    }
}
