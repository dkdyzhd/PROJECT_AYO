using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionItem : MonoBehaviour, IInteractable
    {
        //�̸��� �ƿ� �����ϴ� �ͺ��� �������� ����� �����Ϳ��� itemData�� �����ͼ� �װ��� �̸����� ����
        //�ڵ� ���̱� & ������ �� ��ũ��Ʈ�� �ļ� �Ӽ��� �ο��ϴ� �ͺ��� ����
        public string Key => itemData.itemName + gameObject.GetHashCode();
        public string Message => "Pick Up";

        //Item ��ũ��Ʈ�� Ȱ���Ͽ� itemData ����
        public Item itemData;

        public void Interact()
        {
            InteractionUI.Instance.RemoveInteractionData(this);

            // To do : Add Inventory
            // var weaponItem = itemData as WeaponItem; //ĳ����

            InventoryUI.Instance.AddItem(itemData.itemName);
            // InventoryUI.Instance.AddItem(itemData);

            // �ı��� ���� ����
            Destroy(gameObject);
        }

    }
}
