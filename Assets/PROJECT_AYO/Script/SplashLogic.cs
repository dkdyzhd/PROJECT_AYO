using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class SplashLogic : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for(int i = 0; i <detectedObject.Count; i++)
            {
                Gizmos.DrawLine(transform.position, detectedObject[i].transform.position);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        public List<GameObject> detectedObject = new List<GameObject>();
        public float radius = 3f;
        public float splashAngle = 60f;
        public float distance = 10f;

        [ContextMenu("Detect_Explosion")]
        public void DetectObjectBySphere_Explosion()
        {
            detectedObject.Clear();
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- �ٽ�! ��ü�ȿ� �ִ���
            //��ü �� ���������� Ȯ��
            for(int i = 0;i < overlappedObjects.Length; i++)
            {
                //Rigbody�� ������ ����������
                if (overlappedObjects[i].attachedRigidbody != null)
                {
                    overlappedObjects[i].attachedRigidbody.AddExplosionForce(1000f, transform.position, radius);
                }

                detectedObject.Add(overlappedObjects[i].gameObject);
            }
        }
        [ContextMenu("Dectect")]
        public void DetectObjectBySphere()
        {
            detectedObject.Clear();
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- �ٽ�! ��ü�ȿ� �ִ���
            //��ü �� ���������� Ȯ��
            for (int i = 0; i < overlappedObjects.Length; i++)
            {
                Vector3 dir = transform.position - overlappedObjects[i].transform.position;
                Ray ray = new Ray(transform.position, dir.normalized);
                if (Physics.Raycast(ray, out RaycastHit hit, radius))    //2���˻�(��ֹ����� ���� ���� ��� �������� ���� �˻�)
                {
                    if (hit.transform == overlappedObjects[i].transform)
                    {
                        detectedObject.Add(overlappedObjects[i].gameObject);
                    }
                }
            }


        }
        [ContextMenu("Dectect3")]
        public void DetectObjectBySphere3()
        {
            detectedObject.Clear();
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- �ٽ�! ��ü�ȿ� �ִ���
            
            //��ä�� �ݰ�� & ��ü��
            for(int i = 0;i < overlappedObjects.Length; i++)
            {
                //�ޱ��� ���� ��� ������ƮA = pivot�� �ڽ� / ������ƮB =�˻��ؾ��ϴ� ������Ʈ
                // A�� Forward ����
                Vector3 forwardA = transform.forward;

                // A���� B�� ���ϴ� ����
                Vector3 directionToB = (overlappedObjects[i].transform.position - transform.position).normalized;

                // �� ���� ���� ���� ���
                float angle = Vector3.Angle(forwardA, directionToB);

                // ������ 60�� �̳����� Ȯ��
                if (angle <= splashAngle)
                {
                    Debug.Log("B�� A�� Forward �������� 60�� �̳��� �ֽ��ϴ�.");
                    detectedObject.Add(overlappedObjects[i].gameObject);
                }
                else
                {
                    Debug.Log("B�� A�� Forward �������� 60�� �ۿ� �ֽ��ϴ�.");
                }
            }

        }
        [ContextMenu("Dectect2")]
        public void DetectObjectBySphere2()
        {
            //�չ������� radius = ���� 
            detectedObject.Clear();
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hitObjects = Physics.SphereCastAll(ray, radius, distance);
            if (hitObjects != null)
            {
                for (int i = 0; i < hitObjects.Length; i++)
                {
                    detectedObject.Add(hitObjects[i].transform.gameObject);
                }
            }
        }
    }
}
