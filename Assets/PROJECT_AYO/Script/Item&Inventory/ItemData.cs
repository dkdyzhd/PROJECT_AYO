using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public enum ItemType
    {
        None = 0,
        Resource = 1,
        Food = 2,
        Weapon = 3,
        Heal = 4,
        Build = 5,

    }

    public enum FoodType
    {
        None = 0,
        Health = 1,
        Thirst = 2,
        Hunger = 3,
        Variable = 4,
    }

    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        [Header("Info")]
        public string itemName;
        public string itemInfo;
        public ItemType itemType;
        public Sprite itemImage;
        public GameObject itemPrefab;
        //public Image image;

        [Header("Stacking")]
        public bool canStack;
        public int maxStackAmount;
    }
}

