using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class TreeSensor : MonoBehaviour
    {
        //Ž���ȳ����� ������ ����Ʈ ����
        public List<CollectableResource> detectedTree = new List<CollectableResource>();

        public CollectableResource GetClosedTree()  //�������� ã�� �Լ�
        {
            CollectableResource result = null;  //Ž���Ȱ��� ������ null
            if (detectedTree.Count > 0) // Ž���ȳ����� �ִٸ�
            {
                float beforeDistance = float.MaxValue;  // �⺻�Ÿ� 9999..�� ����
                for (int i = 0; i < detectedTree.Count; i++)    // Ž���ȳ��� ����ŭ for�� ����
                {
                    // �÷��̾�� ���� ������ �Ÿ� ���ϱ�
                    float distance = Vector3.Distance(transform.position, detectedTree[i].transform.position);
                    if (distance < beforeDistance)  //�Ÿ��� �� �۴ٸ�(���� ���Ŀ��� Ž���Ǿ��� ������ �Ÿ��� ��)
                    {
                        result = detectedTree[i];   //Ž���ȳ����� ����� ����
                        beforeDistance = distance;  //before �Ÿ��� �Ÿ����� �����ϰ� ����
                    }
                }
            }
            // Ž���� ������ ���� �������� return
            return result;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Tree)   // Tree�� trigger�� ���Դٸ�
                {
                    collectableResource.OnResourceDestroy += OnResourceDestroyNotified; //�ݹ鿡 ü�ΰɱ�
                    detectedTree.Add(collectableResource);  //Ž���ȳ����� �߰�
                }
            }
        }

        private void OnResourceDestroyNotified(CollectableResource resource)
        {
            detectedTree.Remove(resource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out CollectableResource collectableResource))
            {
                if (collectableResource.ResourceType == CollectResourceType.Tree)   // trigger���� ������ 
                {
                    collectableResource.OnResourceDestroy -= OnResourceDestroyNotified;
                    detectedTree.Remove(collectableResource);   // ����
                }
            }
        }
    }
}
