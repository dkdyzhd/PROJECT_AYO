using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class Weapon : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePosition.position, firePosition.position + firePosition.forward * range);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
        }

        public GameObject MuzzlePrefab;
        public GameObject bulletPrefab;
        public Transform firePosition;

        public float fireRate = 0.1f;
        public float range = 100f;

        private float lastShootTime = 0f;

        private void Update()
        {
            
        }

        public void Shoot()
        {
            if (Time.time > lastShootTime + fireRate)
            {
                //Possible Shoot

                lastShootTime = Time.time;

                //create Muzzle
                var newMuzzle = Instantiate(MuzzlePrefab);
                newMuzzle.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                newMuzzle.gameObject.SetActive(true);
                Destroy(newMuzzle, 1f);

                // Create Bullet
                var newBullet = Instantiate(bulletPrefab);
                //�ѿ��� �Ѿ��� ������ ������ ����(ī�޶� -> �ѱ�)
                newBullet.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                newBullet.gameObject.SetActive(true);

                //�� ������Ʈ�� ���� �浹ó���� ���� �ʰ� Unity Physics Engine���� �����ϵ��� ����
                Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
            }
        }
    }
}
