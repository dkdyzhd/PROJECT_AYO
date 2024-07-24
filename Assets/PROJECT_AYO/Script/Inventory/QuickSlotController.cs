using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class QuickSlotController : MonoBehaviour
    {
        [SerializeField] private SlotUI[] quickSlots;   //퀵슬롯6개
        [SerializeField] private Transform quick_parent;    //퀵슬롯의 부모 오브젝트

        private int selectedSlot;   //선택된 퀵슬롯의 인덱스
        [SerializeField] private GameObject goSelectedImage;    //선택된 퀵슬롯 이미지

        [SerializeField] private GameObject holder;

        private WeaponItemData weaponData;
        private GameObject currentEquipWeapon = null;
        private WeaponItemData currentEquipWeaponData = null;


        private void Start () 
        { 
            quickSlots = quick_parent.GetComponentsInChildren<SlotUI>();
            selectedSlot = 0;
        }

        private void Update ()
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
            goSelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
        }
        private void ChangeWeaponItemData()
        {
            weaponData = quickSlots[selectedSlot].item as WeaponItemData;    //아이템데이터 검사 Weapon으로 바꿔주기
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
        }

        private void UseItem()
        {
            if (quickSlots[selectedSlot].item != null)  //퀵슬롯에 아이템이 있다면
            {
                //아이템타입이 Weapon이라면
                if (quickSlots[selectedSlot].item.itemType == ItemType.Weapon)  
                {
                    ChangeWeaponItemData();
                    if (currentEquipWeaponData != null) //현재 무기데이터가 있고
                    {
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
                        OnHolder();
                    }
                }
                //아이템타입이 Food라면
                else if (quickSlots[selectedSlot].item.itemType == ItemType.Food)
                {
                    //To do : 맨손 & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat();

                }
                else
                {
                    //To do : 맨손 -> (Holder SetActive(false))
                }
            }
            else
            {
                //To do : 맨손
                RemoveWeapon();
            }
        }

    }
}
