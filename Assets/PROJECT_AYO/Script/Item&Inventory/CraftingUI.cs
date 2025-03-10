using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class CraftingUI : MonoBehaviour
    {
        public CraftingManager craftingManager;
        public GameObject quickSlotController;
        [HideInInspector]
        // 아이템을 선택하고 제작 버튼을 따로 만들었을때
        //public RecipeData selectRecipe;

        //public Transform itemListParent;    //UI아이템 목록이 들어갈 부모 오브젝트
        //public GameObject itemButtonPrefab;
        // 리스트형식으로 제작가능한 아이템 목록만들어서 아이템을 선택하면 제작이 되도록
        //public List<RecipeData> availableRecipes;   

        // Start is called before the first frame update
        void Start()
        {
            
        }

        //void CreateCraftingItem()   // CraftingUI에 제작가능한 아이템 목록 버튼 생성
        //{
        //    foreach (var recipe in availableRecipes)
        //    {
        //        GameObject itemButton = Instantiate(itemButtonPrefab, itemListParent);  // 버튼 생성
        //        // To do : 아이템 이미지 불러오기
        //        // 아이템 이름 불러오기
        //        Text buttonText = itemButton.GetComponentInChildren<Text>();
        //        buttonText.text = recipe.outputItem.itemName;

        //        Button button = itemButton.GetComponent<Button>();
        //        button.onClick.AddListener(() => OnItemClicked(recipe));

        //        // 재료가 부족하면 버튼 비활성화
        //        if (!craftingManager.CraftItem(recipe))
        //        {
        //            button.interactable = false;
        //            buttonText.color = Color.gray; // 비활성화 색상 변경
        //        }
        //    }
        //}
        
        //void OnItemClicked(RecipeData recipe)   // 클릭시 제작
        //{
        //    bool craftSuccess = craftingManager.CraftItem(recipe);
        //    if (craftSuccess)
        //    {   //CraftingManager 에서 제작이 되면
        //        Debug.Log($"{recipe.outputItem.itemName} 제작 성공!");
        //    }
        //}

        ////예비
        //public void OnClickCraftButton()
        //{
        //    TryCraftItem();
        //}
        ////예비
        //void TryCraftItem()
        //{
        //    if(selectRecipe != null)
        //    {
        //        bool craftSuccess = craftingManager.CraftItem(selectRecipe);
        //        if (craftSuccess)
        //        {
        //            Debug.Log($"{selectRecipe.outputItem.itemName} 제작 성공!");
        //        }
        //    }
        //}
    }
}
