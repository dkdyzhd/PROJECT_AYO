using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionFood : MonoBehaviour, IInteractable
    {
        public string Key => "Fish" + gameObject.GetHashCode();

        public string Message => "Eat";

        public void Interact()
        {
            Destroy(gameObject);

            InteractionUI.Instance.RemoveInteractionData(this);

            //Add Inventory
        }
    }
}
