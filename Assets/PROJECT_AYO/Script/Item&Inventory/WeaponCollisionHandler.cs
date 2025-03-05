using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class WeaponCollisionHandler : MonoBehaviour
    {
        public float damage; // 무기가 입히는 데미지

        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 객체가 동물인지 확인
            if (other.CompareTag("Animal"))
            {
                // AnimalCtrl 스크립트를 가져와 데미지 적용
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
