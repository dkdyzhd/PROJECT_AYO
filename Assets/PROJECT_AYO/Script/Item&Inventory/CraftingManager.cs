using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AYO.RecipeData;

namespace AYO
{
    public class CraftingManager : MonoBehaviour
    {
        //public List<RecipeData> recipes;    //�迭���� ����Ʈ�� ����_25.03.10
        public QuickSlotController quickSlotController;

        public List<RecipeData> availableRecipes;
        public CraftingUI craftingUI;

        // ���۰������� �Ǻ�
        public bool TestCanCraftItem(RecipeData recipe)
        {
            foreach (RecipeData.Ingredient ingredient in recipe.ingredients)
            {
                if (!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
                {
                    return false;
                }
            }

            // �ʿ��� �������� �ִ� ���
            foreach (var ingredient in recipe.ingredients)
            {
                TestCraftItem(recipe);
            }
            return true;
        }

        // ����
        public void TestCraftItem(RecipeData recipe)
        {
            // �����Կ� ���۰���� �߰�
            quickSlotController.AddItem(recipe.outputItem);
            Debug.Log($"���ۿϷ� {recipe.outputItem.itemName}");
        }

        // -------------------------------------------------------
        public bool CraftItem(RecipeData recipe) // �κ��丮�� �������� �ִ��� �˻�
        {
            //foreach�� ������� = for���� �����ϰ� ��� & Dictionary�� for���� ������� ����
            // �ʿ��� �������� ���� ���
            foreach (RecipeData.Ingredient ingredient in recipe.ingredients)    
            {
                if(!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
                {
                    Debug.Log("��ᰡ �����մϴ�!");
                    return false;
                }
            }

            // �ʿ��� �������� �ִ� ���
            foreach(var ingredient in recipe.ingredients)
            {
                // ��� ����
                quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
            }

            // �����Կ� ���۰���� �߰�
            quickSlotController.AddItem(recipe.outputItem);
            Debug.Log($"���ۿϷ� {recipe.outputItem.itemName}");

            return true;
        }

    }
}
