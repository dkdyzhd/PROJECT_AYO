using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AYO
{
    public class AyoPlayerController : MonoBehaviour
    {
        public static AyoPlayerController Instance { get; private set; } = null;

        public bool IsEnableMovement
        {
            set => isEnableMovement = value;
        }

        [Header("Character Setting")]
        public float moveSpeed = 3.0f;
        public float sprintSpeed = 5.0f;
        public float speedChangeRate = 10.0f;

        [Header("Camera Setting")]
        //***CinemachineCamera 사용 안함으로 주석처리
        //public float cameraHorizontalSpeed = 2.0f;
        //public float cameraVerticalSpeed = 2.0f;

        //***총사용 할 때 CameraSetting을 위한 변수
        public Transform weaponTransform; // 총구 Transform
        public LayerMask aimLayer; // 레이캐스트를 감지할 레이어 (지면, 벽 등)
        public float aimRange = 100f;

        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;

        [Header("Weapon Holder")]
        public GameObject weaponHolder;

        [Header("Weapon FOV")]
        //public float defaultFOV;

        [Header("Camera Clamping")]
        //***CinemachineCamera 사용 안함으로 주석처리
        //public float topClamp = 70.0f;
        //public float bottomClamp = -30.0f;
        //public GameObject cinemachineCameraTarget;
        //public float cameraAngleOverride = 0.0f;

        [Header("Animation Rigging")]
        public Transform aimTarget;
        public LayerMask aimingLayer;
        public UnityEngine.Animations.Rigging.Rig aimingRig; //풀네임 : 생략하고 싶으면 using에 UnityEngine.Animations.Rigging넣는다

        public float aimingIKBlendCurrent;
        public float aimingIKBlendTarget;

        // InventoryUI 함수에서 static 속성으로 Instance를 선언해놓았기 때문에 따로 안해도됨
        // [Header("Inventory")]
        // public InventoryUI inventoryUI;
        [Header("Inventory Bag")]
        public GameObject inventoryBag;

        [HideInInspector]
        public Animator animator;
        private CharacterController controller;
        private Camera mainCamera;
        private InteractionSensor interactionSensor;
        private TreeSensor treeSensor;
        private RockSensor rockSensor;
        private AnimationEventListener animationEventListener;
        private Weapon currentWeapon;

        private bool isSprint = false;
        private Vector2 moveInput;
        private Vector3 moveDirection;
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        public float verticalVelocity;

        private Vector2 look;
        private const float _threshold = 0.01f;
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        //public Cinemachine.CinemachineImpulseSource impulseSource;

        private float axingDamage = 20.0f;

        private bool isInventoryBag = false;
        private bool isEnableMovement = true;

        private bool isAxing = false;
        private bool isPickAxing = false;

        //카메라쉐이크
        //public List<Vector3> recoilShakePattern = new List<Vector3>();
        //private int currentRecoilIndex = 0;

        //private bool isStrafe = false;

        private void Awake()
        {
            Instance = this;
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            interactionSensor = GetComponentInChildren<InteractionSensor>();
            treeSensor = GetComponentInChildren<TreeSensor>();
            rockSensor = GetComponentInChildren<RockSensor>();
            animationEventListener = GetComponentInChildren<AnimationEventListener>();
            animationEventListener.OnTakenAnimationEvent += OnTakenAnimationEvent;

            //Gun
            //var weaponGameObject = TransformUtility.FindGameObjectWithTag(weaponHolder, "Weapon");
            //currentWeapon = weaponGameObject.GetComponent<Weapon>(); //루프를 이용해서 찾기(하위에하위에하위에를 찾기는 힘듬, InChild는 하나의 하위까지만 찾아줌)
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        private void OnEnable()
        {
            interactionSensor.Ondected += OnDectedInteraction;
            interactionSensor.OnLost += OnLostInteraction;
        }

        private void OnDectedInteraction(IInteractable interactable)
        {
            InteractionUI.Instance.AddInteractionData(interactable);
        }

        private void OnLostInteraction(IInteractable interactable)
        {
            InteractionUI.Instance.RemoveInteractionData(interactable);
        }

        private void Update()
        {
            HandleMovement();
            HandleAiming();

            //CameraSystem.Instance.TargetFOV = defaultFOV;

            if (Input.GetKeyDown(KeyCode.Tab))  // Down으로 해야 한번으로 입력이 됨 > GetKey는 여러번 입력
            {
                InventoryUI.Instance.OnInventory();
            }


            //player move
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            moveInput = new Vector2(horizontal, vertical);

            //---Mouse 이동에 따른 camera 움직임
            //float hMouse = Input.GetAxis("Mouse X");
            //float vMouse = Input.GetAxis("Mouse Y") * -1;  // 상하반전
            //look = new Vector2(hMouse, vMouse);

            isSprint = Input.GetKey(KeyCode.LeftShift);
            Move();

            animator.SetFloat("Speed", animationBlend);
            //animator.SetFloat("Speed", isSprint ? 5.0f : 3.0f);
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);

            if(Input.GetKeyDown(KeyCode.E))
            {
                InteractionUI.Instance.DoInteract();
                
                animator.SetTrigger("Trigger_ItemPick");
                //To do : 클릭하면 먹는모션 -> 따로 UI 구현?
            }

            //***Shooting***
            if (Input.GetMouseButton(0))
            {
                if (QuickSlotController.Instance.isFPSMode)
                {
                    //Gun
                    var weaponGameObject = TransformUtility.FindGameObjectWithTag(weaponHolder, "Weapon");
                    currentWeapon = weaponGameObject.GetComponent<Weapon>(); //루프를 이용해서 찾기(하위에하위에하위에를 찾기는 힘듬, InChild는 하나의 하위까지만 찾아줌)

                    if (currentWeapon != null)
                    {
                        currentWeapon.Shoot();
                    }
                    //***위 코드와 같은 코드(최신버전에서 가능)***
                    //currentWeapon?.Shoot(); 


                    //***총을 쏠 때 앞을 보게하기 ***
                    //var cameraForward = Camera.main.transform.forward.normalized;
                    //cameraForward.y = 0;
                    //transform.forward = cameraForward;
                }
            }

            //***자원캐기***
            if (Input.GetMouseButtonDown(0))
            {
                CollectableResource tree = treeSensor.GetClosedTree();
                CollectableResource rock = rockSensor.GetClosedRock();

                // ***Axing***
                //if (tree != null && !isAxing && QuickSlotController.Instance.isFPSMode == false)
                if (!isAxing && QuickSlotController.Instance.isFPSMode == false)
                {
                    animator.SetTrigger("Trigger_Axe");
                    if (tree != null)
                    {
                        Vector3 dir = tree.transform.position - transform.position;
                        transform.forward = dir.normalized;
                        animator.SetTrigger("Trigger_Axe");
                        isAxing = true;

                        tree.Damage(axingDamage);

                        Camera_Ctrl.Instance.ShakeCamera(2f, 0.5f);   //강도 0.2, 지속 시간 1초
                    }
                    //***카메라 스크립트 변경으로 주석처리***
                    //Vector3 velocity = recoilShakePattern[currentRecoilIndex];
                    //currentRecoilIndex++;
                    //if (currentRecoilIndex >= recoilShakePattern.Count)
                    //{
                    //    currentRecoilIndex = currentRecoilIndex = 0;
                    //}

                    //CameraSystem.Instance.ShakeCamera(velocity,0.2f,1f);
                    //***카메라 스크립트 변경으로 주석처리***

                    //float currentHp = tree.resourceHp - axingDamage;
                    //tree.resourceHp = currentHp;
                    //Debug.Log(currentHp);
                    //if (currentHp == 0)
                    //{
                    //    CollectableResource.Instance.OnDestroy();
                    //}
                }

                //***PickAxing***
                //if (rock != null && !isAxing && QuickSlotController.Instance.isFPSMode == false)
                if (!isAxing && QuickSlotController.Instance.isFPSMode == false)
                {
                    animator.SetTrigger("Trigger_Axe");
                    if ((rock != null))
                    {
                        Vector3 dir = rock.transform.position - transform.position;
                        transform.forward = dir.normalized;
                        animator.SetTrigger("Trigger_Axe");
                        isAxing = true;

                        rock.Damage(axingDamage);

                        Camera_Ctrl.Instance.ShakeCamera(2f, 0.5f);   //강도 0.2, 지속 시간 1초
                    }
                }
                //To do : 자원에 따라서 무기를 다르게
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                InteractionUI.Instance.SelectNext();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                InteractionUI.Instance.SelectPrev();
            }

        }//*** Update

        private void HandleMovement()
        {

        }

        private void HandleAiming()
        {
            if(mainCamera == null || weaponTransform == null)
                return;

            //마우스 포인터에서 레이 발사
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, aimRange, aimLayer))
            {
                Vector3 targetPosition = hit.point;

                // 총구 방향 조정
                Vector3 aimDirection = targetPosition - weaponTransform.position;
                aimDirection.y = 0;    //수평 방향만 회전
                weaponTransform.rotation = Quaternion.LookRotation(aimDirection);

                //캐릭터의 기본 회전은 움직임 방향에 맞추되, 조준 방향과 독립적
                if(moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
                }
            }
        }
        private void LateUpdate()
        {
            //CameraRotation();
        }

        //private void CameraRotation()
        //{
        //    // if there is an input and camera position is not fixed
        //    if (look.sqrMagnitude >= _threshold)
        //    {
        //        //Don't multiply mouse input by Time.deltaTime;
        //        float deltaTimeMultiplier = 1.0f;

        //        cinemachineTargetYaw += look.x * deltaTimeMultiplier * cameraHorizontalSpeed;
        //        cinemachineTargetPitch += look.y * deltaTimeMultiplier * cameraVerticalSpeed;
        //    }

        //    // clamp our rotations so our values are limited 360 degrees
        //    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        //    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        //    // Cinemachine will follow this target
        //    cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
        //        cinemachineTargetYaw, 0.0f);
        //}

        private void Move()
        {
            //if (!isEnableMovement)
                //return;

            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (moveInput == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = moveInput.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                //중력작용?
                //rigidbody.velocity = new Vector3(currentHorizontalSpeed, rigidbody.velocity.y, 0.0f);

                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                //중력작용?
                //rigidbody.velocity = new Vector3(currentHorizontalSpeed, rigidbody.velocity.y, 0.0f);

                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            moveDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (moveInput != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                //if (!isStrafe){}
                
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            // 유니티 자체의 캐릭터 컨트롤러를 이용- 콜라이더 기능이 탑재되어있음
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);   //verticalVelocity => 중력에 영향을 주는 변수
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void OnTakenAnimationEvent(string eventName)
        {
            if (eventName.Equals("Axe"))
            {
                isAxing = false;
            }
        }

    }
}
