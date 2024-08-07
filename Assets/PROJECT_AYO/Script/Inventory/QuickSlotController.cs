using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [HideInInspector]
        public int selectedSlot;   //선택된 퀵슬롯의 인덱스
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
            if (currentEquipWeaponData.weaponType == WeaponType.Gun)
            {
                ToggleFPSMode(true);
            }

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

            if (currentEquipWeaponData != null && currentEquipWeaponData.weaponType == WeaponType.Gun)
            {
                ToggleFPSMode(false);
            }
            
            currentEquipWeaponData = null;
        }

        private void Eat(QuickSlotData slotData)
        {
            AyoPlayerController.Instance.animator.SetTrigger("Trigger_Eat");
            PlayerCondition.Instance.Eat(5);

            slotData.count--;
            int index = quickSlotDatas.IndexOf(slotData);   //배열의 인덱스를 뽑아내는 함수
            InventoryUI.Instance.SetQuickSlotCount(index, slotData.count);
            if (slotData.count <= 0)
            {
                quickSlotDatas[selectedSlot].itemData = null;
            }

            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }

        private void ToggleFPSMode(bool isOn)
        {
            AyoPlayerController.Instance.animator.SetBool("isFPSMode", isOn);


            bool isFPSMode = weaponData.weaponType == WeaponType.Gun;
            if (isFPSMode)
            {
            }
            else
            {
                AyoPlayerController.Instance.animator.SetBool("isFPSMode", isOn);
            }

        }

        private void UseItem()
        {
            if (selectedSlot >= quickSlotDatas.Count)   //빈 퀵슬롯을 선택했을때
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
                bool isWeaponData = quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Weapon;
                //아이템타입이 Weapon이라면
                if (isWeaponData)
                {
                    if (currentEquipWeaponData != null) //현재 무기데이터가 있고
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        if (currentEquipWeaponData != weaponData)   //새로 받아온 무기아이템데이터와 같지 않다면
                        {
                            RemoveWeapon();
                            OnHolder();
                        }
                        else    //새로 받아온 무기아이템데이터가 같다면(다시 눌러서 무기 내리기)
                        {
                            RemoveWeapon(); //무기 내려놓기
                        }
                    }
                    else    //현재 무기데이터가 없다면
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        OnHolder();
                    }
                }
                //아이템타입이 Food라면
                else if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Food && !isWeaponData)
                {
                    AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                    //To do : 맨손 & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat(quickSlotDatas[selectedSlot]);

                }
                else
                {
                    //To do : 맨손 -> (Holder SetActive(false))
                    AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                    RemoveWeapon();
                }
            }
            else
            {
                //To do : 맨손
                AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                RemoveWeapon();
            }
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.canStack)  //쌓을수 있는 아이템이라면
            {
                int index = GetExistItemStackable(itemData, out QuickSlotData result);
                if (result != null && index >= 0)
                {
                    result.count++;
                    InventoryUI.Instance.SetQuickSlotCount(index, result.count);
                    return;
                }
            }

            for (int i = 0; i < quickSlotDatas.Count; i++)
            {
                if (quickSlotDatas[i].itemData == null)
                {
                    quickSlotDatas[i].itemData = itemData;
                    quickSlotDatas[i].count = 1;
                    InventoryUI.Instance.RefreshSlot(quickSlotDatas);
                    return;
                }
            }

            var newQuickSlotData = new QuickSlotData() { itemData = itemData, count = 1, };
            quickSlotDatas.Add(newQuickSlotData);
            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }

        private int GetExistItemStackable(ItemData itemData, out QuickSlotData resultData)
        {
            for (int i = 0; i < quickSlotDatas.Count; i++)
            {
                if (quickSlotDatas[i].itemData == itemData && quickSlotDatas[i].count < itemData.maxStackAmount)
                {
                    resultData = quickSlotDatas[i];
                    return i;
                }
            }

            resultData = null;
            return -1;
        }
    }
}
