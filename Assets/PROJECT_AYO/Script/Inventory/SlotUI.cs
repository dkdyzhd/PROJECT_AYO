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
        private ItemData _item;
        public ItemData item 
        {  get { return _item; }    //아이템 정보를 넘겨줄때 사용
           set { 
                _item = value;  //아이템에 들어오는 정보를 _item에 저장
                if (_item != null)
                {
                    slotItemImage.sprite = item.itemImage;
                    slotItemImage.color = new Color(1, 1, 1, 1);    //
                }
                else
                {
                    slotItemImage.color = new Color(1, 1, 1, 0);    //
                }

            } 
        }
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
