using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class QuickSlotController : MonoBehaviour
    {
        [SerializeField] private SlotUI[] quickSlots;   //������6��
        [SerializeField] private Transform quick_parent;    //�������� �θ� ������Ʈ

        private int selectedSlot;   //���õ� �������� �ε���
        [SerializeField] private GameObject goSelectedImage;    //���õ� ������ �̹���

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
            selectedSlot = slotnum; //���õ� ����
            goSelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
        }
        private void ChangeWeaponItemData()
        {
            weaponData = quickSlots[selectedSlot].item as WeaponItemData;    //�����۵����� �˻� Weapon���� �ٲ��ֱ�
        }
        private void OnHolder()
        {
            currentEquipWeaponData = weaponData;    //���� �޾ƿ� ������ �Է�

            //To do : Holder�� ����ϱ�
            GameObject newWeapon = Instantiate(weaponData.itemPrefab);  //���� �޾ƿµ������� itemPrefab ����
            newWeapon.transform.parent = holder.transform;
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.Euler(weaponData.rotation);

            currentEquipWeapon = newWeapon;
        }
        private void RemoveWeapon()
        {
            if (currentEquipWeapon != null)     //����ִ� ���Ⱑ �ִٸ�
            {
                Destroy(currentEquipWeapon);    //���� ����
                currentEquipWeapon = null;  //null������ �����
            }
            currentEquipWeaponData = null;
        }
        private void Eat()
        {
            AyoPlayerController.Instance.animator.SetTrigger("Trigger_Eat");
        }

        private void UseItem()
        {
            if (quickSlots[selectedSlot].item != null)  //�����Կ� �������� �ִٸ�
            {
                //������Ÿ���� Weapon�̶��
                if (quickSlots[selectedSlot].item.itemType == ItemType.Weapon)  
                {
                    ChangeWeaponItemData();
                    if (currentEquipWeaponData != null) //���� ���ⵥ���Ͱ� �ְ�
                    {
                        if (currentEquipWeaponData != weaponData)   //���� �޾ƿ� ��������۵����Ϳ� ���� �ʰ�
                        {
                            RemoveWeapon();
                            OnHolder();
                        }
                        else    //���� �޾ƿ� ��������۵����Ͱ� ���ٸ�
                        {
                            RemoveWeapon(); //���� ��������

                            //currentEquipWeaponData = null;
                        }
                    }
                    else    //���� ���ⵥ���Ͱ� ���ٸ�
                    {
                        OnHolder();
                    }
                }
                //������Ÿ���� Food���
                else if (quickSlots[selectedSlot].item.itemType == ItemType.Food)
                {
                    //To do : �Ǽ� & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat();

                }
                else
                {
                    //To do : �Ǽ� -> (Holder SetActive(false))
                }
            }
            else
            {
                //To do : �Ǽ�
                RemoveWeapon();
            }
        }

    }
}
