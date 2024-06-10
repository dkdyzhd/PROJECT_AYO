using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AYO
{
    public class SelectionUI : MonoBehaviour
    {
        public RectTransform selectionBox;

        private Vector2 startClickPosition = Vector3.zero;
        private List<SelectableCharacter> selectedCharacters = new List<SelectableCharacter>();

        private void Start()
        {
            selectionBox.sizeDelta = Vector3.zero;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    for (int i = 0; i < selectedCharacters.Count; i++)
                    {
                        selectedCharacters[i].SetDestination(hit.point);
                    }
                }

            }

            //Selection Box Start
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                startClickPosition = Input.mousePosition;
            }

            //Selection Box Update - ( Draging )
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector2 currentPosition = Input.mousePosition;
                float width = currentPosition.x - startClickPosition.x;
                float height = currentPosition.y - startClickPosition.y;

                selectionBox.anchoredPosition =
                    new Vector2(startClickPosition.x, startClickPosition.y) + new Vector2(width, height) / 2f;

                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            }

            //Execute - Selection Order
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                // To do : Select Order !
                Select();

                startClickPosition = Vector3.zero;
                selectionBox.sizeDelta = Vector2.zero;
            }
        }

        public void Select()
        {
            Vector2 min = selectionBox.anchoredPosition -(selectionBox.sizeDelta / 2);
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

            selectedCharacters.Clear();

            //1. 피라미드 형태로 Casting 을 만들어서 바꿔보기
            // 2. Physics.Cast 된 오브젝트들에게서 ISelectable 인터페이스 컴포넌트를 찾아서 가져오고
            // 3. 기존 Select/Deslect 함수를 수행하기
            foreach (var character in SelectableCharacter.SpawnedCharacters)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(character.transform.position);

                if (screenPosition.x > min.x && screenPosition.x < max.x &&
                    screenPosition.y > min.y && screenPosition.y < max.y)
                {
                    character.Select();
                    selectedCharacters.Add(character);
                }
                else
                {
                    character.Deselect();
                }
            }
        }
    }
}
