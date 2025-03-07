using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class CraftingUI : MonoBehaviour
    {
        public CraftingManager craftingManager;
        public GameObject quickSlotController;
        [HideInInspector]
        public RecipeData selectRecipe;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void OnClickCraftButton()
        {
            TryCraftItem();
        }

        void TryCraftItem()
        {
            if(selectRecipe != null)
            {
                bool craftSuccess = craftingManager.CraftItem(selectRecipe);
                if (craftSuccess)
                {
                    Debug.Log($"{selectRecipe.outputItem.itemName} 제작 성공!");
                }
            }
        }
    }
}
