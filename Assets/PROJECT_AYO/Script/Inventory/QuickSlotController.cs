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
        public int selectedSlot;   //���õ� �������� �ε���
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
            if (currentEquipWeaponData.weaponType == WeaponType.Gun)
            {
                ToggleFPSMode(true);
            }

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
            int index = quickSlotDatas.IndexOf(slotData);   //�迭�� �ε����� �̾Ƴ��� �Լ�
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
            if (selectedSlot >= quickSlotDatas.Count)   //�� �������� ����������
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
                bool isWeaponData = quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Weapon;
                //������Ÿ���� Weapon�̶��
                if (isWeaponData)
                {
                    if (currentEquipWeaponData != null) //���� ���ⵥ���Ͱ� �ְ�
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        if (currentEquipWeaponData != weaponData)   //���� �޾ƿ� ��������۵����Ϳ� ���� �ʴٸ�
                        {
                            RemoveWeapon();
                            OnHolder();
                        }
                        else    //���� �޾ƿ� ��������۵����Ͱ� ���ٸ�(�ٽ� ������ ���� ������)
                        {
                            RemoveWeapon(); //���� ��������
                        }
                    }
                    else    //���� ���ⵥ���Ͱ� ���ٸ�
                    {
                        weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;
                        OnHolder();
                    }
                }
                //������Ÿ���� Food���
                else if (quickSlotDatas[selectedSlot].itemData.itemType == ItemType.Food && !isWeaponData)
                {
                    AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                    //To do : �Ǽ� & animation settrigger Eat
                    RemoveWeapon();
                    //currentEquipWeaponData = null;
                    Eat(quickSlotDatas[selectedSlot]);

                }
                else
                {
                    //To do : �Ǽ� -> (Holder SetActive(false))
                    AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                    RemoveWeapon();
                }
            }
            else
            {
                //To do : �Ǽ�
                AyoPlayerController.Instance.animator.SetBool("isFPSMode", false);
                RemoveWeapon();
            }
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.canStack)  //������ �ִ� �������̶��
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
