using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AYO
{
    public class CraftingManager : MonoBehaviour
    {
        public RecipeData[] recipes;
        public QuickSlotController quickSlotController;

        public bool CraftItem(RecipeData recipe) // 인벤토리에 아이템이 있는지 검사
        {
            //foreach문 사용이유 = for문을 간단하게 사용 & Dictionary는 for문을 사용하지 못함
            // 필요한 아이템이 없을 경우
            foreach (RecipeData.Ingredient ingredient in recipe.ingredients)    
            {
                if(!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
                {
                    Debug.Log("재료가 부족합니다!");
                    return false;
                }
            }

            // 필요한 아이템이 있는 경우
            foreach(var ingredient in recipe.ingredients)
            {
                // 재료 차감
                quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
            }

            // 퀵슬롯에 제작결과물 추가
            quickSlotController.AddItem(recipe.outputItem);
            Debug.Log($"제작완료 {recipe.outputItem.itemName}");

            return true;
        }

    }
}
