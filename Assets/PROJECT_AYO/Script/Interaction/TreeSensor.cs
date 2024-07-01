using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class TreeSensor : MonoBehaviour
    {
        //탐지된나무를 저장할 리스트 선언
        public List<CollectableResource> detectedTree = new List<CollectableResource>();

        public CollectableResource GetClosedTree()  //가까운나무를 찾는 함수
        {
            CollectableResource result = null;  //탐지된것이 없으면 null
            if (detectedTree.Count > 0) // 탐지된나무가 있다면
            {
                float beforeDistance = float.MaxValue;  // 기본거리 9999..로 설정
                for (int i = 0; i < detectedTree.Count; i++)    // 탐지된나무 수만큼 for문 돌기
                {
                    // 플레이어와 나무 사이의 거리 구하기
                    float distance = Vector3.Distance(transform.position, detectedTree[i].transform.position);
                    if (distance < beforeDistance)  //거리가 더 작다면(최초 이후에는 탐지되었던 나무와 거리를 비교)
                    {
                        result = detectedTree[i];   //탐지된나무를 결과로 저장
                        beforeDistance = distance;  //before 거리를 거리값과 동일하게 저장
                    }

                    if(CollectableResource.Instance == null)
                    {
                        detectedTree.RemoveAt(i);   //오류남->
                    }
                }
            }
            // 탐지된 나무중 가장 가까운나무를 return
            return result;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Tree)   // Tree가 trigger로 들어왔다면
                {
                    detectedTree.Add(collectableResource);  //탐지된나무로 추가
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Tree)   // trigger에서 나가면 
                {
                    detectedTree.Remove(collectableResource);   // 제거
                }
            }
        }
    }
}
