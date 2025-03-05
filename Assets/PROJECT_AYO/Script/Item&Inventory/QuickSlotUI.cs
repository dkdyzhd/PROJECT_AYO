using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class QuickSlotUI : MonoBehaviour
    {
        public static QuickSlotUI Instance { get; private set; } = null;

        [HideInInspector]
        private ItemData _item;
        public ItemData Item
        {
            get { return _item; }    //아이템 정보를 넘겨줄때 사용
            set
            {
                _item = value;  //아이템에 들어오는 정보를 _item에 저장
                if (_item != null)
                {
                    slotItemImage.sprite = Item.itemImage;
                    slotItemImage.color = new Color(1, 1, 1, 1);    //
                    Count = 1;
                }
                else
                {
                    slotItemImage.sprite = null;
                    slotItemImage.color = new Color(1, 1, 1, 0);    //
                    countText.text = string.Empty;
                }

            }
        }

        public int Count
        {
            set
            {
                if(value > 1)
                {
                    countText.text = value.ToString();
                }
                else
                {
                    countText.text = string.Empty;
                }
            } 
        }

        public Button button;
        public Image slotItemImage;
        public TMPro.TextMeshProUGUI countText;


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
            InventoryUI.Instance.SelectItem(QuickSlotController.Instance.selectedSlot);
        }
    }
}
