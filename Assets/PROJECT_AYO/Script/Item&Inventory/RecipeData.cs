using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting")]
    public class RecipeData : ScriptableObject
    {
        public ItemData outputItem; //제작 결과물
        public int outputQuantity;  //제작 결과물의 개수

        //필요한 재료 구조체
        [System.Serializable]
        public struct Ingredient
        {
            public ItemData item;   //재료가 되는 아이템
            public int quantity;    //필요한 개수
        }

        public Ingredient[] ingredients;    //제작에 필요한 재료 목록을 저장할 리스트

    }
}
