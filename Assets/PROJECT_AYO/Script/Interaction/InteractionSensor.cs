using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionSensor : MonoBehaviour
    {
        public List<IInteractable> interactables = new List<IInteractable>();

        public System.Action<IInteractable> Ondected;
        public System.Action<IInteractable> OnLost;

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                interactables.Remove(interactable);
            }
        }
    }
}
