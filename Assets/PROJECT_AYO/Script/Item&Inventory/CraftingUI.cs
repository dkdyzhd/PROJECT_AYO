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
        // �������� �����ϰ� ���� ��ư�� ���� ���������
        //public RecipeData selectRecipe;

        //public Transform itemListParent;    //UI������ ����� �� �θ� ������Ʈ
        //public GameObject itemButtonPrefab;
        // ����Ʈ�������� ���۰����� ������ ��ϸ��� �������� �����ϸ� ������ �ǵ���
        //public List<RecipeData> availableRecipes;   

        // Start is called before the first frame update
        void Start()
        {
            
        }

        //void CreateCraftingItem()   // CraftingUI�� ���۰����� ������ ��� ��ư ����
        //{
        //    foreach (var recipe in availableRecipes)
        //    {
        //        GameObject itemButton = Instantiate(itemButtonPrefab, itemListParent);  // ��ư ����
        //        // To do : ������ �̹��� �ҷ�����
        //        // ������ �̸� �ҷ�����
        //        Text buttonText = itemButton.GetComponentInChildren<Text>();
        //        buttonText.text = recipe.outputItem.itemName;

        //        Button button = itemButton.GetComponent<Button>();
        //        button.onClick.AddListener(() => OnItemClicked(recipe));

        //        // ��ᰡ �����ϸ� ��ư ��Ȱ��ȭ
        //        if (!craftingManager.CraftItem(recipe))
        //        {
        //            button.interactable = false;
        //            buttonText.color = Color.gray; // ��Ȱ��ȭ ���� ����
        //        }
        //    }
        //}
        
        //void OnItemClicked(RecipeData recipe)   // Ŭ���� ����
        //{
        //    bool craftSuccess = craftingManager.CraftItem(recipe);
        //    if (craftSuccess)
        //    {   //CraftingManager ���� ������ �Ǹ�
        //        Debug.Log($"{recipe.outputItem.itemName} ���� ����!");
        //    }
        //}

        ////����
        //public void OnClickCraftButton()
        //{
        //    TryCraftItem();
        //}
        ////����
        //void TryCraftItem()
        //{
        //    if(selectRecipe != null)
        //    {
        //        bool craftSuccess = craftingManager.CraftItem(selectRecipe);
        //        if (craftSuccess)
        //        {
        //            Debug.Log($"{selectRecipe.outputItem.itemName} ���� ����!");
        //        }
        //    }
        //}
    }
}
