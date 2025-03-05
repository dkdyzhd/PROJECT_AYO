using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting")]
    public class RecipeData : ScriptableObject
    {
        public ItemData outputItem; //���� �����
        public int outputQuantity;  //���� ������� ����

        //�ʿ��� ��� ����ü
        [System.Serializable]
        public struct Ingredient
        {
            public ItemData item;   //��ᰡ �Ǵ� ������
            public int quantity;    //�ʿ��� ����
        }

        public Ingredient[] ingredients;    //���ۿ� �ʿ��� ��� ����� ������ ����Ʈ

    }
}
