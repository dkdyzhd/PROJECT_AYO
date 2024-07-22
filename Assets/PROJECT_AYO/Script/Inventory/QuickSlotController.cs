using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class QuickSlotController : MonoBehaviour
    {
        [SerializeField] private SlotUI[] quickSlots;   //������6��
        [SerializeField] private Transform quick_parent;    //�������� �θ� ������Ʈ

        private int selectedSlot;   //���õ� �������� �ε���
        [SerializeField] private GameObject goSelectedImage;    //���õ� ������ �̹���

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
            selectedSlot = slotnum; //���õ� ����
        }

        private void UseItem()
        {
            if (quickSlots[selectedSlot].item != null)
            {
                if (quickSlots[selectedSlot].item.itemType == ItemType.Weapon)
                {
                    //To do : Holder�� ����ϱ�
                }
            }
            else
            {
                //To do : �Ǽ�
            }
        }

    }
}
