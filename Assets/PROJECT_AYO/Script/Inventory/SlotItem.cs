using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class SlotItem : MonoBehaviour
    {
        //Image Component를 담을 곳
        [SerializeField] Image image;

        private Item _item;
        public Item item
        {
            //슬롯의 item 정보를 넘겨줄 때 사용
            get { return _item; }
            set
            {
                //item에 들어오는 정보의 값은 _item에 저장
                _item = value;
                //Inventory 스크립트의 List<Item> items에 등록된 아이템이 있다면
                if (_item != null)
                {
                    //itemImage를 image에 저장 그리고 Image의 알파 값을 1로 하여 이미지를 표시
                    image.sprite = item.itemImage;
                    image.color = new Color(1, 1, 1, 1);
                }
                //item이 null 이면(빈슬롯 이면) Image의 알파 값 0을 주어 화면에 표시X
                else
                {
                    image.color = new Color(1, 1, 1, 0);
                }
            }
        }
    }
}
