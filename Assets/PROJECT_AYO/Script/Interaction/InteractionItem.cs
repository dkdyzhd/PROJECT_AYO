using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

namespace AYO
{
    public class InteractionItem : MonoBehaviour, IInteractable
    {
        //이름을 아예 선언하는 것보다 아이템을 만들어 놓은것에서 itemData를 가져와서 그것을 이름으로 선언
        //코드 줄이기 & 일일히 다 스크립트를 파서 속성을 부여하는 것보다 편함
        public string Key => itemData.itemName + gameObject.GetHashCode();
        public string Message => "Pick Up";

        //Item 스크립트를 활용하여 itemData 선언
        public ItemData itemData;
        [Header("아이템")]
        [SerializeField] private ItemData itemdata;
        [Header("아이템 기능")]
        [SerializeField] private UltEvent onUse;

        public ItemData ItemData => itemdata;
        public UltEvent OnUse => onUse;


        public void Interact()
        {
            InteractionUI.Instance.RemoveInteractionData(this);

            //QuickSlotController.Instance.AddItem(itemData);
            QuickSlotController.Instance.AddItem(this);

            // 파괴를 제일 나중
            //Destroy(transform.root.gameObject);
            this.gameObject.SetActive(false);
        }

        public void Use()
        {
            onUse.Invoke();
            if (itemData.isExpendable)
            {
                QuickSlotController.Instance.RemoveItem(itemData, 1);
            }
        }

    }
}
