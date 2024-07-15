using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class RockSensor : MonoBehaviour
    {
        //Ž���ȵ��� ������ ����Ʈ ����
        public List<CollectableResource> detectedRock = new List<CollectableResource>();

        public CollectableResource GetClosedRock()  //����� ���� ã�� �Լ�
        {
            CollectableResource result = null;  //Ž���� ���� ������ null
            if (detectedRock.Count > 0) // Ž���� ���� �ִٸ�
            {
                float beforeDistance = float.MaxValue;  // �⺻�Ÿ� 9999..�� ����
                for (int i = 0; i < detectedRock.Count; i++)    // Ž���ȵ� ����ŭ for�� ����
                {
                    // �÷��̾�� ���� ������ �Ÿ� ���ϱ�
                    float distance = Vector3.Distance(transform.position, detectedRock[i].transform.position);
                    if (distance < beforeDistance)  //�Ÿ��� �� �۴ٸ�(���� ���Ŀ��� Ž���Ǿ��� ������ �Ÿ��� ��)
                    {
                        result = detectedRock[i];   //Ž���� ���� ����� ����
                        beforeDistance = distance;  //before �Ÿ��� �Ÿ����� �����ϰ� ����
                    }
                }
            }
            // Ž���� ������ ���� ����� ���� return
            return result;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Rock)   // Rock�� trigger�� ���Դٸ�
                {
                    collectableResource.OnResourceDestroy += OnResourceDestroyNotified; //�ݹ鿡 ü�ΰɱ�
                    detectedRock.Add(collectableResource);  //Ž���ȵ��� �߰�
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
                if (collectableResource.ResourceType == CollectResourceType.Rock)   // trigger���� ������ 
                {
                    collectableResource.OnResourceDestroy -= OnResourceDestroyNotified;
                    detectedRock.Remove(collectableResource);   // ����
                }
            }
        }
    }
}
