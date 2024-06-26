using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionAXE : MonoBehaviour, IInteractable
    {
        public string Key => itemData.itemName + gameObject.GetHashCode();

        public string Message => "Pick Up";

        //Item 스크립트를 활용하여 itemData 선언
        public Item itemData;

        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}
