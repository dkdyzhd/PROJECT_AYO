using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

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
        [Header("플레이어 컨트롤러")]
        [SerializeField] private AyoPlayerController player;
        [Header("무기 데이터")]
        [SerializeField] private WeaponItemData weaponData;
        [Header("Holder")]
        [SerializeField] private GameObject holder;

        [Header("총알 발사 설정")]
        public float fireRate = 0.1f;
        public float range = 100f;
        private float lastShootTime = 0f;

        private bool onHolder = false;


        public void OnHolder()  // UltEvent: 무기가 홀더에 추가될 때 호출
        {
            if (onHolder) // 이미 홀더에 있으면 제거
            {
                RemoveHolder(); 
                return;
            }

            if (weaponData.weaponType == WeaponType.Gun)
            {
                player.ToggleFPSMode(true); // FPS 모드 활성화
            }

            onHolder = true; //홀더에 추가됨
            // 무기 위치 변경
            this.transform.SetParent(holder.transform);
            this.transform.localPosition = Vector3.zero;  //위치 초기화
            this.transform.localRotation = Quaternion.Euler(weaponData.rotation);  //회전 초기화
            this.gameObject.SetActive(true);  //활성화

            // 아이템 획득을 위해 켜놨던 콜라이더 끄기
            Collider weaponCollider = this.GetComponent<Collider>();
            weaponCollider.enabled = false;


            // 무기 충돌 기본적으로 비활성화 (공격중 충돌 활성화 할 것)
            var weaponHandler = this.GetComponent<WeaponCollisionHandler>();
            if (weaponHandler != null)
            {
                weaponHandler.enabled = false;  
            }
        }

        public void RemoveHolder()
        {
            if (!onHolder) 
            {
                return; //이미 홀더에 없는 경우
            }
            if (weaponData.weaponType == WeaponType.Gun)
            {
                player.ToggleFPSMode(false); // FPS 모드 비활성화
            }

            player.ToggleFPSMode(false); // FPS 모드 비활성화
            onHolder = false; //홀더에서 제거됨

            this.transform.SetParent(null);
            this.gameObject.SetActive(false);  //무기 비활성화

            // 아이템 획득을 위해 켜놨던 콜라이더 켜기
            Collider weaponCollider = this.GetComponent<Collider>();
            weaponCollider.enabled = true;

            // 무기 충돌 비활성화 설정
            var weaponHandler = this.GetComponent<WeaponCollisionHandler>();
            if (weaponHandler != null)
            {
                weaponHandler.enabled = false; 
            }
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
                //총에서 총알이 나가는 것으로 변경(카메라 -> 총구)
                newBullet.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                newBullet.gameObject.SetActive(true);

                //두 오브젝트가 서로 충돌처리가 되지 않게 Unity Physics Engine에게 무시하도록 지시
                Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
            }
        }
    }
}
