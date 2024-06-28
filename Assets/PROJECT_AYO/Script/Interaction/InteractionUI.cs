using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class InteractionUI : MonoBehaviour
    {
        public static InteractionUI Instance { get; private set;} =null;

        public Transform root;
        public InteractionUI_ListItem itemPrefab;

        public List<InteractionUI_ListItem> createditems = new List<InteractionUI_ListItem>();
        public int selectedIndex = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void AddInteractionData(IInteractable interactableData)
        {
            var newitemList = Instantiate(itemPrefab, root);
            newitemList.gameObject.SetActive(true);

            newitemList.DataKey = interactableData.Key;
            newitemList.Message = interactableData.Message;
            newitemList.InteractionData = interactableData;
            newitemList.IsSelected = false;

            createditems.Add(newitemList);
        }

        public void RemoveInteractionData(IInteractable interactableData)
        {
            var targetitem = createditems.Find(x => x.DataKey.Equals(interactableData.Key));
            if (targetitem != null)
            {
                createditems.Remove(targetitem);
                Destroy(targetitem.gameObject);
            }

        }

        public void SelectPrev()
        {
            if (createditems.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < createditems.Count)
                {
                    createditems[selectedIndex].IsSelected = false;
                }

                selectedIndex--;
                selectedIndex = Mathf.Clamp(selectedIndex, 0, createditems.Count - 1);
                createditems[selectedIndex].IsSelected = true;
            }

        }

        public void SelectNext()
        {
            if (createditems.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < createditems.Count)
                {
                    createditems[selectedIndex].IsSelected = false;
                }

                selectedIndex++;
                selectedIndex = Mathf.Clamp(selectedIndex, 0, createditems.Count - 1);
                createditems[selectedIndex].IsSelected = true;
            }
        }

        public void DoInteract()
        {
            if (createditems.Count > 0 && selectedIndex >= 0)
            {
                createditems[selectedIndex].interactionData.Interact();
                //AyoPlayerController.Instance.animator.SetTrigger("Trigger_ItemPick");
            }
        }
    }
}
