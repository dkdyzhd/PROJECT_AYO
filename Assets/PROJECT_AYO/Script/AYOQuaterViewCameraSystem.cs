using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AYOQuaterViewCameraSystem : MonoBehaviour
    {
        public float cameraRotationSpeed = 3f;
        public float cameraMoveSpeed = 3f;

        public float cameraMoveSprintSpeed = 5f;

        public bool isSprint = false;

        public Camera mainCamera;
        public Transform cameraPivot;
        public Cinemachine.CinemachineVirtualCamera quaterViewCamera;

        private void Update()
        {
            float rotateDirection = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                rotateDirection = -1;
                Vector3 originalRot = quaterViewCamera.transform.localRotation.eulerAngles;
                quaterViewCamera.transform.localRotation = Quaternion.Euler(originalRot + 
                    (Vector3.up * rotateDirection * cameraRotationSpeed * Time.deltaTime));
            }

            if(Input.GetKey(KeyCode.E))
            {
                rotateDirection = 1;
                Vector3 originalRot = quaterViewCamera.transform.localRotation.eulerAngles;
                quaterViewCamera.transform.localRotation = Quaternion.Euler(originalRot +
                    (Vector3.up * rotateDirection * cameraRotationSpeed * Time.deltaTime));
            }

            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprint = true;
                cameraMoveSpeed = cameraMoveSprintSpeed;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprint = false;
                cameraMoveSpeed = 3f;
            }
            //To do : isSprint일 경우에 대한 코드 수정 -> 좀 더 깔끔하게되도록 targetSpeed 변수 만들어 볼 것
            // 실제 플레이를 해보면 플레이어가 가운데 딱 있지 않음
            // -> 지형에 따라 오르락 내리락 할 때 딜레이 되는 느낌?
            // 수정하기

            if (Input.GetKey(KeyCode.W))
            {
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.S))
            {
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.A))
            {
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.D))
            {
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
