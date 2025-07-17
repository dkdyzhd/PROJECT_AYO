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
        public int quantity;
    }

    public class QuickSlotController : MonoBehaviour
    {
        public static QuickSlotController Instance { get; private set; } = null;

        private List<SlotData> slotDataList = new List<SlotData>(); //리팩토링

        [HideInInspector]
        public int selectedSlot;   //선택된 퀵슬롯의 인덱스
        [SerializeField] private GameObject holder;

        private WeaponItemData weaponData;
        private GameObject currentEquipWeapon = null;
        private WeaponItemData currentEquipWeaponData = null;

        public List<QuickSlotData> quickSlotDatas = new List<QuickSlotData>();

        //아이템과 충분한 양이 있는지 확인하기 위함
        public Dictionary<ItemData, int> quickSlotItems = new Dictionary<ItemData, int>();

        public bool HasItem(ItemData item, int quantity)
        {
            foreach (SlotData slotData in slotDataList)
            {
                if (slotData.GetItemData() == item)
                {
                    if (slotData.GetItemCount() >= quantity)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool isFPSMode = false;

        public bool isDragging = false;

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
            InventoryUI.Instance.RefreshSlot(slotDataList);
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
            //UseItem();
        }

        // 퀵슬롯 선택 & 사용
        private void SelectedSlot(int slotnum)
        {
            selectedSlot = slotnum; //선택된 슬롯
            InventoryUI.Instance.SelectItem(slotnum);

            InteractionItem item = slotDataList[slotnum].GetItem();
            item.Use();
            // to do : 슬롯이 비어있을경우 들고 있던 무기를 내려놓기? 무기를 내려놓는건 어떻게 할지 고민
        }

        public void EnableWeaponCollision()
        {
            if (currentEquipWeapon != null)
            {
                var weaponHandler = currentEquipWeapon.GetComponent<WeaponCollisionHandler>();
                if (weaponHandler != null)
                {
                    weaponHandler.enabled = true; // 충돌 활성화
                }
            }
        }

        public void DisableWeaponCollision()
        {
            if (currentEquipWeapon != null)
            {
                var weaponHandler = currentEquipWeapon.GetComponent<WeaponCollisionHandler>();
                if (weaponHandler != null)
                {
                    weaponHandler.enabled = false; // 충돌 비활성화
                }
            }
        }

        private void Eat(QuickSlotData slotData)
        {
            AyoPlayerController.Instance.animator.SetTrigger("Trigger_Eat");
            PlayerCondition.Instance.Eat(5);

            slotData.quantity--;
            int index = quickSlotDatas.IndexOf(slotData);   //배열의 인덱스를 뽑아내는 함수
            InventoryUI.Instance.SetQuickSlotCount(index, slotData.quantity);
            if (slotData.quantity <= 0)
            {
                quickSlotDatas[selectedSlot].itemData = null;
            }

            InventoryUI.Instance.RefreshSlot(slotDataList);
        }

        // ------------------------ 아이템 획득한 후 인벤토리에 추가되는 로직 -----------------------------
        // 리팩토링된 AddItem 메서드
        public void AddItem(InteractionItem item)
        {
            // 스택가능한 아이템이라면
            if (item.ItemData.isStackable)
            {
                // GetExistItemStackable() 호출 -> 이미 존재하는 동일한 아이템찾기
                //index : 슬롯의 위치 / result : 해당 슬롯의 SlotData
                int index = GetExistItemStackable(item.ItemData, out SlotData result);
                if (result != null && index >= 0)
                {
                    // 기존 아이템이 있으면  슬롯 아이템 count++ 
                    result.SetSlotItemCount(1);
                    Debug.Log($" 기존 아이템 {item.ItemData.itemName} 개수 증가: {result.GetItemCount()}");

                    // To do :  인벤토리 count에도 적용
                    //invenUI.SetSlotUICount(index, result.GetItemCount());
                    InventoryUI.Instance.SetQuickSlotCount(index, result.GetItemCount());
                    return;
                }
            }

            // 빈 슬롯 찾기 & 새로운칸을 만들어야 할 때 그 인덱스 저장
            int i = 0;
            for (i = 0; i < slotDataList.Count; i++)      //데이터 리스트의 번호만 알려줌
            {
                if (slotDataList[i].GetItemData() == null)
                {
                    //slotDataList[i] = slotdata;       //*** 20250528_ 수정해야됨
                    break;
                }
            }

            // 새 아이템 추가
            SlotData slotdata = new SlotData();
            slotdata.SetSlotItem(item);     // *** 20250528_ 그냥 item 으로 아이템 자체를 넘겨주기
            slotdata.SetSlotItemCount(1);
            slotDataList.Add(slotdata);
            Debug.Log($" 새 아이템 추가: {item.ItemData.itemName}, 개수: 1");

            InventoryUI.Instance.RefreshSlot(slotDataList);
        }
        //----------------------------------------------------------------------------------------------
        // 기존
        public void AddItem(ItemData itemData)
        {
            if (itemData.isStackable)  //쌓을수 있는 아이템이라면
            {
                // GetExistItemStackable() 호출 -> 이미 존재하는 동일한 아이템찾기
                //index : 슬롯의 위치 / result : 해당 슬롯의 QuickSlotData
                int index = GetExistItemStackable(itemData, out QuickSlotData result);
                if (result != null && index >= 0)
                {// 기존 아이템이 있으면 개수 ++ & UI 갱신
                    result.quantity++;

                    // Dictionary에도 함께 반영
                    if (quickSlotItems.ContainsKey(itemData))
                        quickSlotItems[itemData]++;
                    else
                        quickSlotItems[itemData] = result.quantity;

                    InventoryUI.Instance.SetQuickSlotCount(index, result.quantity);
                    Debug.Log($" 기존 아이템 {itemData.itemName} 개수 증가: {result.quantity}");
                    return;
                }
            }
            Debug.Log($"현재 퀵 슬롯 칸 수 : {quickSlotDatas.Count}");
            // 순회하며 빈 슬롯 찾기
            for (int i = 0; i < quickSlotDatas.Count; i++)
            {   // 빈 슬롯 찾으면 새 아이템 추가
                if (quickSlotDatas[i].itemData == null)
                {
                    quickSlotDatas[i].itemData = itemData;
                    quickSlotDatas[i].quantity = 1;

                    // Dictionary에도 추가
                    quickSlotItems[itemData] = 1;


                    Debug.Log($" 새 아이템 추가: {itemData.itemName}, 개수: 1");
                    InventoryUI.Instance.RefreshSlot(slotDataList);
                    return;
                }
            }

            //빈슬롯이 없을 경우, 새로운 슬롯 데이터를 생성
            var newQuickSlotData = new QuickSlotData() { itemData = itemData, quantity = 1, };
            quickSlotDatas.Add(newQuickSlotData);

            // Dictionary에도 추가
            quickSlotItems[itemData] = 1;


            Debug.Log($" 새로운 슬롯에 아이템 추가: {itemData.itemName}, 개수: 1");
            InventoryUI.Instance.RefreshSlot(slotDataList);
        }
        //----------------------------------------------------------------------------------------------
        // 리팩토링된 GetExistItemStackable 메서드
        public int GetExistItemStackable(ItemData itemData, out SlotData resultData)
        {
            for (int i = 0; i < slotDataList.Count; i++)
            {
                if (slotDataList[i] != null && slotDataList[i].GetItemData() == itemData)
                {
                    resultData = slotDataList[i];
                    return i;
                }
            }
            resultData = null;
            return -1;  // 없으면 -1 반환
        }
        //----------------------------------------------------------------------------------------------
        // 기존
        private int GetExistItemStackable(ItemData itemData, out QuickSlotData resultData)
        {
            for (int i = 0; i < quickSlotDatas.Count; i++)
            {   // 같은 아이템이 있는 슬롯이 있으면서 && 최대 스택을 초과하지 않은 경우
                if (quickSlotDatas[i].itemData == itemData && quickSlotDatas[i].quantity < itemData.maxStackAmount)
                {
                    resultData = quickSlotDatas[i]; // 찾은 퀵슬롯데이터를 resultData에 저장
                    return i;   // 슬롯 인덱스를 반환
                }
            }

            resultData = null;
            return -1;      // 없으면 -1 반환
        }
        //----------------------------------------------------------------------------------------------
        // 기존
        public void RemoveItem(ItemData item, int quantity)
        {
            int index = GetExistItemStackable(item, out QuickSlotData result);
            int slotIndex = quickSlotDatas.IndexOf(result);   //배열의 인덱스를 뽑아내는 함수
            if (result != null && index > 0)
            {
                result.quantity -= quantity;
                if (result.quantity <= 0)
                {// 다쓰면 아이템데이터가 퀵슬롯에서 없어지도록
                    quickSlotDatas[slotIndex].itemData = null;
                }
            }

            // Dictionary에서도 차감
            quickSlotItems[item] -= quantity;
            if (quickSlotItems[item] <= 0)
            {
                quickSlotItems.Remove(item);
            }

            InventoryUI.Instance.RefreshSlot(slotDataList);
        }
        //----------------------------------------------------------------------------------------------
        // 리팩토링된 ItemRemove 메서드
        public void ItemRemove(ItemData item, int quantity)
        {
            int index = GetExistItemStackable(item, out SlotData result);   //result가 null이면 -1을 반환
            int slotIndex = slotDataList.IndexOf(result);   // 따로 리스트의 인덱스를 뽑아내는 함수를g= 활용하여 저장


            if (result != null && index >= 0)
            {
                result.SetSlotItemCount(-quantity);
                if (result.GetItemCount() <= 0)
                {   // 다쓰면 아이템데이터가 퀵슬롯에서 없어지도록
                    //slotDataList.Remove(result);    // => 뒤 아이템들이 다 땡겨짐
                    slotDataList[slotIndex].SetSlotItem(null);    // => 빈 칸이 그대로 남아있음
                }
            }

            //invenUI.RefreshUI(slotDataList);
            InventoryUI.Instance.RefreshSlot(slotDataList);
        }
    }
}
