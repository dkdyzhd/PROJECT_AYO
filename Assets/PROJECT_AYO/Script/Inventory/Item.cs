using UnityEngine;

namespace AYO
{
    public enum ItemType
    {
        None = 0,
        Resource = 1,
        Consumable = 2,
        Equip = 3,

    }

    public enum ConsumableType
    {
        None = 0,
        Health = 1,
        Thirst = 2,
        Hunger = 3,
        Variable = 4,
    }

    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        [Header("Info")]
        public string itemName;
        public ItemType type;
        public Sprite itemImage;

        [Header("Stacking")]
        public bool canStack;
        public int maxStackAmount;
    }
}

