using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class SelectableCharacter : MonoBehaviour, ISelectable
    {
        public static List<SelectableCharacter> SpawnedCharacters = new List<SelectableCharacter>();
        public GameObject Selection => selectStateUI;

        public GameObject selectStateUI;
        public UnityEngine.AI.NavMeshAgent navMeshAgent;

        public void Awake()
        {
            SpawnedCharacters.Add(this);
        }

        private void OnDestroy()
        {
            SpawnedCharacters.Remove(this);
        }
        public void Deselect()
        {
            Selection.gameObject.SetActive(false);
        }

        public void Select()
        {
            Selection.gameObject.SetActive(true);
        }

        public void SetDestination(Vector3 targetDestination)
        {
            navMeshAgent.SetDestination(targetDestination);
        }
    }
}
