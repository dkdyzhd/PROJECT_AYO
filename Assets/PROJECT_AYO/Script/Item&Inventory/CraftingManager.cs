using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static AYO.RecipeData;

namespace AYO
{
    public class CraftingManager : MonoBehaviour
    {
        //public List<RecipeData> recipes;    //배열에서 리스트로 변경_25.03.10
        public QuickSlotController quickSlotController;

        public List<RecipeData> availableRecipes;
        public CraftingUI craftingUI;
        public Transform itemListParent;    //UI아이템 목록이 들어갈 부모 오브젝트
        public GameObject itemButtonPrefab;

        private QuickSlotData quickSlotData;

        //Test
        public bool testCanCraftItem = false;

        private void Start()
        {
            CreateUICraftingItem();
        }

        private void Update()
        {

        }
        // 제작가능한지 판별
        public bool TestCanCraftItem(RecipeData recipe)
        {
            if (quickSlotController == null)
            {
                Debug.LogError(" quickSlotController가 null입니다! 연결이 안 됨!");
                //return false;
            }   // 안뜸 > null상태는 아니다

            if (quickSlotController.quickSlotItems == null)
            {
                // quickSlotItems 가 제대로 생성 안 된 경우
                Debug.LogError(" quickSlotItems가 null입니다! 초기화되지 않음!");
                //return false;
            }

            if (quickSlotController.quickSlotItems.Count == 0)
            {
                // 비어있는 경우
                Debug.LogError(" quickSlotItems에 아이템이 없습니다! 아이템 추가가 안 되었음!");
                //return false;
            }  

            foreach (var item in quickSlotController.quickSlotItems)    //딕셔너리에 아이템이 추가되었는지 확인
            {
                Debug.Log($"퀵슬롯 아이템: {item.Key.itemName}, 개수: {item.Value}");
            }

            foreach (RecipeData.Ingredient ingredient in recipe.ingredients)
            {
                //weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;

                if (!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
                {
                    Debug.Log("재료가 부족합니다!");
                    return false;
                }
            }

            // 필요한 아이템이 있는 경우
            foreach (var ingredient in recipe.ingredients)
            {
                // 재료 차감
                // quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
                Debug.Log("제작 가능!");
            }
            return true;
        }

        // 제작
        public void TestCraftItem(RecipeData recipe)
        {
            // 퀵슬롯에 제작결과물 추가
            foreach (var ingredient in recipe.ingredients)
            {
                quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
                Debug.Log($"재료 차감 {ingredient.item}, {ingredient.quantity}개");
            }
            quickSlotController.AddItem(recipe.outputItem);
            Debug.Log($"제작완료 {recipe.outputItem.itemName}");
        }

        void OnItemButtonClick(RecipeData recipe)
        {
            Debug.Log($"{recipe.outputItem.itemName} 생성 버튼클릭!");
            if (TestCanCraftItem(recipe))
            {
                TestCraftItem(recipe);
            }

            else
            {
                Debug.Log($"{recipe.outputItem.itemName} 생성 실패!");
                return;
            }
        }

        // -------------------------------------------------------
        void CreateUICraftingItem()   // CraftingUI에 제작가능한 아이템 목록 버튼 생성
        {
            foreach (var recipe in availableRecipes)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemListParent);  // 버튼 생성
                // To do : 아이템 이미지 불러오기
                // 아이템 이름 불러오기
                Text buttonText = itemButton.GetComponentInChildren<Text>();
                buttonText.text = recipe.outputItem.itemName;

                Button button = itemButton.GetComponent<Button>();
                Debug.Log("버튼생성완료!");
                button.onClick.AddListener(() => OnItemButtonClick(recipe));

                // 재료가 부족하면 버튼 비활성화
                //if (!TestCanCraftItem(recipe))
                //{
                //    button.interactable = false;
                //    buttonText.color = Color.gray; // 비활성화 색상 변경
                //}
            }
        }
        //-------------------------------------------------------------------------------------
        //void OnItemClicked(RecipeData recipe)   // 클릭시 제작
        //{
        //    bool craftSuccess = CraftItem(recipe);
        //    if (craftSuccess)
        //    {   //CraftingManager 에서 제작이 되면
        //        Debug.Log($"{recipe.outputItem.itemName} 제작 성공!");
        //    }
        //}

        //public bool CraftItem(RecipeData recipe) // 인벤토리에 아이템이 있는지 검사
        //{
        //    //foreach문 사용이유 = for문을 간단하게 사용 & Dictionary는 for문을 사용하지 못함
        //    // 필요한 아이템이 없을 경우
        //    foreach (RecipeData.Ingredient ingredient in recipe.ingredients)    
        //    {
        //        if(!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
        //        {
        //            Debug.Log("재료가 부족합니다!");
        //            return false;
        //        }
        //    }

        //    // 필요한 아이템이 있는 경우
        //    foreach(var ingredient in recipe.ingredients)
        //    {
        //        // 재료 차감
        //        quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
        //    }

        //    // 퀵슬롯에 제작결과물 추가
        //    quickSlotController.AddItem(recipe.outputItem);
        //    Debug.Log($"제작완료 {recipe.outputItem.itemName}");

        //    return true;
        //}

    }
}
