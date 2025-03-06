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

        public bool CraftItem(RecipeData recipe) // �κ��丮�� �������� �ִ��� �˻�
        {
            // �ʿ��� �������� ���� ���
            foreach (var ingredient in recipe.ingredients)
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
