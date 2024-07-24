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
            tpsCamera.m_Lens.FieldOfView = Mathf.Lerp(tpsCamera.m_Lens.FieldOfView, TargetFOV, zoomSpeed * Time.deltaTime);
            //fpsCamera.transform.forward = fpsCamera.Follow.transform.forward;
        }
    }
}
