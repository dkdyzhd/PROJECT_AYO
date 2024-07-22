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

        private void UseItem()
        {
            if (quickSlots[selectedSlot].item != null)
            {
                if (quickSlots[selectedSlot].item.itemType == ItemType.Weapon)
                {
                    var weaponData = quickSlots[selectedSlot].item as WeaponItemData;

                    //To do : Holder에 들게하기
                    GameObject newWeapon = Instantiate(weaponData.itemPrefab);
                    newWeapon.transform.parent = holder.transform;
                    newWeapon.transform.localPosition = Vector3.zero;
                    newWeapon.transform.localRotation = Quaternion.identity;
                }
                else if (quickSlots[selectedSlot].item.itemType == ItemType.Food)
                {
                    //To do : 맨손 & animation settrigger Eat
                    if (holder.activeInHierarchy)
                    {
                        holder.SetActive(false);
                    }
                }
                else
                {
                    //To do : 맨손 -> (Holder SetActive(false))
                }
            }
            else
            {
                //To do : 맨손
            }
        }

    }
}
