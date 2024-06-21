using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AYO
{
    public class InteractionUI_ListItem : MonoBehaviour
    {
        public string DataKey
        {
            set => key = value;
            get => key;
        }

        public string Message 
        {  
            set => text.text = value; 
        }

        public bool IsSelected
        {
            set => selection.SetActive(value);
        }

        public IInteractable InteractionData
        {
            set => interactionData = value;
            get => interactionData;
        }


        public GameObject selection;
        public TMPro.TextMeshPro text;
        public IInteractable interactionData;

        private string key;

    }
}
