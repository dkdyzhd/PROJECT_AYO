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

        private int selectedSlot;   //���õ� �������� �ε���
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
            selectedSlot = slotnum; //���õ� ����
            InventoryUI.Instance.SelectItem(slotnum);
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
            quickSlotDatas[selectedSlot].itemData = null;

            InventoryUI.Instance.RefreshSlot(quickSlotDatas);
        }

        private void UseItem()
        {
            if (selectedSlot >= quickSlotDatas.Count)
            {
                if (currentEquipWeaponData != null) //���� ���ⵥ���Ͱ� �ְ�
                {
                    RemoveWeapon();
                }
                return;
            }

            bool isExistItemData = quickSlotDatas[selectedSlot].itemData != null;
            if (isExistItemData)  //�����Կ� �������� �ִٸ�
            {
                //������Ÿ���� Weapon�̶��
                if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Weapon)
                {
                    if (currentEquipWeaponData != null) //���� ���ⵥ���Ͱ� �ְ�
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
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
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        OnHolder();
                    }
                }
                //������Ÿ���� Food���
                else if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Food)
                {
                    //To do : �Ǽ� & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat();

                }
                else
                {
                    //To do : �Ǽ� -> (Holder SetActive(false))
                    RemoveWeapon();
                }
            }
            else
            {
                //To do : �Ǽ�
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
