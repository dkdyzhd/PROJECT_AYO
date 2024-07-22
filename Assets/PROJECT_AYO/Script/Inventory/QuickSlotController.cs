using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class QuickSlotController : MonoBehaviour
    {
        [SerializeField] private SlotUI[] quickSlots;   //Äü½½·Ô6°³
        [SerializeField] private Transform quick_parent;    //Äü½½·ÔÀÇ ºÎ¸ð ¿ÀºêÁ§Æ®

        private int selectedSlot;   //¼±ÅÃµÈ Äü½½·ÔÀÇ ÀÎµ¦½º
        [SerializeField] private GameObject goSelectedImage;    //¼±ÅÃµÈ Äü½½·Ô ÀÌ¹ÌÁö

        [SerializeField] private Holder holder;

        private void Start () 
        { 
            quickSlots = quick_parent.GetComponentsInChildren<SlotUI>();
            selectedSlot = 0;
        }

        private void Update ()
        {

        }

        private void TryInputNumber()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                
            }
        }

        private void ChangeSlot(int slotnum)
        {
            SelectedSlot(slotnum);
            UseItem();
        }

        private void SelectedSlot(int slotnum)
        {
            selectedSlot = slotnum; //¼±ÅÃµÈ ½½·Ô
        }

        private void UseItem()
        {
            if (quickSlots[selectedSlot].item != null)
            {
                if (quickSlots[selectedSlot].item.itemType == ItemType.Weapon)
                {
                    //To do : Holder¿¡ µé°ÔÇÏ±â
                }
            }
            else
            {
                //To do : ¸Ç¼Õ
            }
        }

    }
}
