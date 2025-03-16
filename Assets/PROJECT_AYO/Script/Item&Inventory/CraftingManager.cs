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
        //public List<RecipeData> recipes;    //�迭���� ����Ʈ�� ����_25.03.10
        public QuickSlotController quickSlotController;

        public List<RecipeData> availableRecipes;
        public CraftingUI craftingUI;
        public Transform itemListParent;    //UI������ ����� �� �θ� ������Ʈ
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
        // ���۰������� �Ǻ�
        public bool TestCanCraftItem(RecipeData recipe)
        {
            if (quickSlotController == null)
            {
                Debug.LogError(" quickSlotController�� null�Դϴ�! ������ �� ��!");
                //return false;
            }   // �ȶ� > null���´� �ƴϴ�

            if (quickSlotController.quickSlotItems == null)
            {
                // quickSlotItems �� ����� ���� �� �� ���
                Debug.LogError(" quickSlotItems�� null�Դϴ�! �ʱ�ȭ���� ����!");
                //return false;
            }

            if (quickSlotController.quickSlotItems.Count == 0)
            {
                // ����ִ� ���
                Debug.LogError(" quickSlotItems�� �������� �����ϴ�! ������ �߰��� �� �Ǿ���!");
                //return false;
            }  

            foreach (var item in quickSlotController.quickSlotItems)    //��ųʸ��� �������� �߰��Ǿ����� Ȯ��
            {
                Debug.Log($"������ ������: {item.Key.itemName}, ����: {item.Value}");
            }

            foreach (RecipeData.Ingredient ingredient in recipe.ingredients)
            {
                //weaponData = quickSlotDatas[selectedSlot].itemData as WeaponItemData;

                if (!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
                {
                    Debug.Log("��ᰡ �����մϴ�!");
                    return false;
                }
            }

            // �ʿ��� �������� �ִ� ���
            foreach (var ingredient in recipe.ingredients)
            {
                // ��� ����
                // quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
                Debug.Log("���� ����!");
            }
            return true;
        }

        // ����
        public void TestCraftItem(RecipeData recipe)
        {
            // �����Կ� ���۰���� �߰�
            foreach (var ingredient in recipe.ingredients)
            {
                quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
                Debug.Log($"��� ���� {ingredient.item}, {ingredient.quantity}��");
            }
            quickSlotController.AddItem(recipe.outputItem);
            Debug.Log($"���ۿϷ� {recipe.outputItem.itemName}");
        }

        void OnItemButtonClick(RecipeData recipe)
        {
            Debug.Log($"{recipe.outputItem.itemName} ���� ��ưŬ��!");
            if (TestCanCraftItem(recipe))
            {
                TestCraftItem(recipe);
            }

            else
            {
                Debug.Log($"{recipe.outputItem.itemName} ���� ����!");
                return;
            }
        }

        // -------------------------------------------------------
        void CreateUICraftingItem()   // CraftingUI�� ���۰����� ������ ��� ��ư ����
        {
            foreach (var recipe in availableRecipes)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemListParent);  // ��ư ����
                // To do : ������ �̹��� �ҷ�����
                // ������ �̸� �ҷ�����
                Text buttonText = itemButton.GetComponentInChildren<Text>();
                buttonText.text = recipe.outputItem.itemName;

                Button button = itemButton.GetComponent<Button>();
                Debug.Log("��ư�����Ϸ�!");
                button.onClick.AddListener(() => OnItemButtonClick(recipe));

                // ��ᰡ �����ϸ� ��ư ��Ȱ��ȭ
                //if (!TestCanCraftItem(recipe))
                //{
                //    button.interactable = false;
                //    buttonText.color = Color.gray; // ��Ȱ��ȭ ���� ����
                //}
            }
        }
        //-------------------------------------------------------------------------------------
        //void OnItemClicked(RecipeData recipe)   // Ŭ���� ����
        //{
        //    bool craftSuccess = CraftItem(recipe);
        //    if (craftSuccess)
        //    {   //CraftingManager ���� ������ �Ǹ�
        //        Debug.Log($"{recipe.outputItem.itemName} ���� ����!");
        //    }
        //}

        //public bool CraftItem(RecipeData recipe) // �κ��丮�� �������� �ִ��� �˻�
        //{
        //    //foreach�� ������� = for���� �����ϰ� ��� & Dictionary�� for���� ������� ����
        //    // �ʿ��� �������� ���� ���
        //    foreach (RecipeData.Ingredient ingredient in recipe.ingredients)    
        //    {
        //        if(!quickSlotController.HasItem(ingredient.item, ingredient.quantity))
        //        {
        //            Debug.Log("��ᰡ �����մϴ�!");
        //            return false;
        //        }
        //    }

        //    // �ʿ��� �������� �ִ� ���
        //    foreach(var ingredient in recipe.ingredients)
        //    {
        //        // ��� ����
        //        quickSlotController.RemoveItem(ingredient.item, ingredient.quantity);
        //    }

        //    // �����Կ� ���۰���� �߰�
        //    quickSlotController.AddItem(recipe.outputItem);
        //    Debug.Log($"���ۿϷ� {recipe.outputItem.itemName}");

        //    return true;
        //}

    }
}
