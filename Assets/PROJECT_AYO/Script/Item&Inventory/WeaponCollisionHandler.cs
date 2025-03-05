using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class WeaponCollisionHandler : MonoBehaviour
    {
        public float damage; // ���Ⱑ ������ ������

        private void OnTriggerEnter(Collider other)
        {
            // �浹�� ��ü�� �������� Ȯ��
            if (other.CompareTag("Animal"))
            {
                // AnimalCtrl ��ũ��Ʈ�� ������ ������ ����
                var animalCtrl = other.GetComponent<AYO.AnimalCtrl>();
                if (animalCtrl != null)
                {
                    animalCtrl.TakeDamage(damage);
                    Debug.Log(animalCtrl.curHp);
                }
            }
        }
    }
}
