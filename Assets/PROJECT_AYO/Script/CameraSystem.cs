using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; } = null;

        //public bool IsTPSMode { get {return isTPSMode;} }

        /// <summary>
        /// 현재 CameraSystem이 TPS모드로 작동하고 있는지 여부 값
        /// </summary>
        public bool IsTPSMode => isTPSMode;
        public float TargetFOV { get; set; } = 60.0f;

        public Cinemachine.CinemachineVirtualCamera tpsCamera;
        //public Cinemachine.CinemachineVirtualCamera fpsCamera;
        public Cinemachine.CinemachineImpulseSource impulseSource;

        public void ShakeCamera(Vector3 velocity, float duration, float force)
        {
            impulseSource.m_DefaultVelocity = velocity;
            impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
            impulseSource.GenerateImpulseWithForce(force);
        }

        public float zoomSpeed = 5.0f;

        private bool isTPSMode = true;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            isTPSMode = tpsCamera.gameObject.activeSelf;
        }

        private void OnDestroy()
        {
            Instance = null;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                isTPSMode = !isTPSMode;
                tpsCamera.gameObject.SetActive(isTPSMode);
                //fpsCamera.gameObject.SetActive(!isTPSMode);
            }
        }

        private void LateUpdate()
        {
            UpdateAimPoint();

            tpsCamera.m_Lens.FieldOfView = Mathf.Lerp(tpsCamera.m_Lens.FieldOfView, TargetFOV, zoomSpeed * Time.deltaTime);
            //fpsCamera.transform.forward = fpsCamera.Follow.transform.forward;
        }
        public Vector3 AimPoint { get; private set; }

        private void UpdateAimPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground", "Default")))
            {
                AimPoint = hit.point;

                // 디버그 시 확인
                Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f);
            }
            else
            {
                // 충돌하지 않을 경우 카메라 방향으로 일정 거리로 설정
                AimPoint = ray.origin + ray.direction * 100f;
            }
        }
    }
}
