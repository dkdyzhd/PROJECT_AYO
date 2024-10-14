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
            get { return _item; }    //������ ������ �Ѱ��ٶ� ���
            set
            {
                _item = value;  //�����ۿ� ������ ������ _item�� ����
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
