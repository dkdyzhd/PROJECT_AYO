using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class SlotUI : MonoBehaviour
    {
        public static SlotUI Instance { get; private set; } = null;

        [HideInInspector]
        public Item item;
        public Button button;
        public Image slotItemImage;

        public int index;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ImageColor()
        {
            
        }
        public void OnItemButtonClick()
        {
            InventoryUI.Instance.SelectItem(index);
        }
    }
}
