using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionSensor : MonoBehaviour
    {
        public bool HasInteractable => interactables.Count > 0;

        public List<IInteractable> interactables = new List<IInteractable>();

        public System.Action<IInteractable> Ondected;
        public System.Action<IInteractable> OnLost;

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out IInteractable interactable))
            {
                Debug.Log("�浹");
                interactables.Add(interactable);

                //�ݹ�
                Ondected?.Invoke(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out IInteractable interactable))
            {
                Debug.Log("����ħ");
                interactables.Remove(interactable);

                //�ݹ�
                OnLost?.Invoke(interactable);
            }
        }
    }
}
