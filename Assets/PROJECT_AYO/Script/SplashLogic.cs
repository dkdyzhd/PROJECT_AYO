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
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- 핵심! 구체안에 있는지
            //구체 안 반지름으로 확인
            for(int i = 0;i < overlappedObjects.Length; i++)
            {
                //Rigbody가 있으면 폭발힘영향
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
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- 핵심! 구체안에 있는지
            //구체 안 반지름으로 확인
            for (int i = 0; i < overlappedObjects.Length; i++)
            {
                Vector3 dir = transform.position - overlappedObjects[i].transform.position;
                Ray ray = new Ray(transform.position, dir.normalized);
                if (Physics.Raycast(ray, out RaycastHit hit, radius))    //2차검사(장애물같은 것이 있을 경우 레이저를 쏴서 검사)
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
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);   //<- 핵심! 구체안에 있는지
            
            //부채꼴 반경안 & 구체안
            for(int i = 0;i < overlappedObjects.Length; i++)
            {
                //앵글을 쓰는 방법 오브젝트A = pivot나 자신 / 오브젝트B =검사해야하는 오브젝트
                // A의 Forward 벡터
                Vector3 forwardA = transform.forward;

                // A에서 B로 향하는 벡터
                Vector3 directionToB = (overlappedObjects[i].transform.position - transform.position).normalized;

                // 두 벡터 간의 각도 계산
                float angle = Vector3.Angle(forwardA, directionToB);

                // 각도가 60도 이내인지 확인
                if (angle <= splashAngle)
                {
                    Debug.Log("B는 A의 Forward 기준으로 60도 이내에 있습니다.");
                    detectedObject.Add(overlappedObjects[i].gameObject);
                }
                else
                {
                    Debug.Log("B는 A의 Forward 기준으로 60도 밖에 있습니다.");
                }
            }

        }
        [ContextMenu("Dectect2")]
        public void DetectObjectBySphere2()
        {
            //앞방향으로 radius = 간격 
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
