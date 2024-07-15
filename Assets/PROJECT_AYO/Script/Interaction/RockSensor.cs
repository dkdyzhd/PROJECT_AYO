using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class RockSensor : MonoBehaviour
    {
        //탐지된돌을 저장할 리스트 선언
        public List<CollectableResource> detectedRock = new List<CollectableResource>();

        public CollectableResource GetClosedRock()  //가까운 돌을 찾는 함수
        {
            CollectableResource result = null;  //탐지된 것이 없으면 null
            if (detectedRock.Count > 0) // 탐지된 돌이 있다면
            {
                float beforeDistance = float.MaxValue;  // 기본거리 9999..로 설정
                for (int i = 0; i < detectedRock.Count; i++)    // 탐지된돌 수만큼 for문 돌기
                {
                    // 플레이어와 나무 사이의 거리 구하기
                    float distance = Vector3.Distance(transform.position, detectedRock[i].transform.position);
                    if (distance < beforeDistance)  //거리가 더 작다면(최초 이후에는 탐지되었던 나무와 거리를 비교)
                    {
                        result = detectedRock[i];   //탐지된 돌을 결과로 저장
                        beforeDistance = distance;  //before 거리를 거리값과 동일하게 저장
                    }
                }
            }
            // 탐지된 나무중 가장 가까운 돌을 return
            return result;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Rock)   // Rock이 trigger로 들어왔다면
                {
                    collectableResource.OnResourceDestroy += OnResourceDestroyNotified; //콜백에 체인걸기
                    detectedRock.Add(collectableResource);  //탐지된돌로 추가
                }
            }
        }

        private void OnResourceDestroyNotified(CollectableResource resource)
        {
            detectedRock.Remove(resource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Rock)   // trigger에서 나가면 
                {
                    collectableResource.OnResourceDestroy -= OnResourceDestroyNotified;
                    detectedRock.Remove(collectableResource);   // 제거
                }
            }
        }
    }
}
