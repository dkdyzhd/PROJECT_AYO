using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float cameraHorizontalSpeed = 2.0f;
        public float cameraVerticalSpeed = 2.0f;

        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;

        [Header("Weapon Holder")]
        public GameObject weaponHolder;

        [Header("Weapon FOV")]
        //public float defaultFOV;

        [Header("Camera Clamping")]
        public float topClamp = 70.0f;
        public float bottomClamp = -30.0f;
        public GameObject cinemachineCameraTarget;
        public float cameraAngleOverride = 0.0f;

        [Header("Animation Rigging")]
        public Transform aimTarget;
        public LayerMask aimingLayer;
        public UnityEngine.Animations.Rigging.Rig aimingRig; //Ǯ���� : �����ϰ� ������ using�� UnityEngine.Animations.Rigging�ִ´�

        public float aimingIKBlendCurrent;
        public float aimingIKBlendTarget;

        // InventoryUI �Լ����� static �Ӽ����� Instance�� �����س��ұ� ������ ���� ���ص���
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
        private Vector2 move;
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

        //ī�޶���ũ
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
            //currentWeapon = weaponGameObject.GetComponent<Weapon>(); //������ �̿��ؼ� ã��(�������������������� ã��� ����, InChild�� �ϳ��� ���������� ã����)
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
            //CameraSystem.Instance.TargetFOV = defaultFOV;

            //����ĳ��Ʈ
            /*if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            }*/

            if (Input.GetKeyDown(KeyCode.Tab))  // Down���� �ؾ� �ѹ����� �Է��� �� > GetKey�� ������ �Է�
            {
                InventoryUI.Instance.OnInventory();
            }


            //player move
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            move = new Vector2(horizontal, vertical);

            //---Mouse �̵��� ���� camera ������
            //float hMouse = Input.GetAxis("Mouse X");
            //float vMouse = Input.GetAxis("Mouse Y") * -1;  // ���Ϲ���
            //look = new Vector2(hMouse, vMouse);

            isSprint = Input.GetKey(KeyCode.LeftShift);
            Move();

            animator.SetFloat("Speed", animationBlend);
            //animator.SetFloat("Speed", isSprint ? 5.0f : 3.0f);
            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);

            if(Input.GetKeyDown(KeyCode.E))
            {
                InteractionUI.Instance.DoInteract();
                
                animator.SetTrigger("Trigger_ItemPick");
                //To do : Ŭ���ϸ� �Դ¸�� -> ���� UI ����?
            }

            //***Shooting***
            if (Input.GetMouseButton(0))
            {
                if (QuickSlotController.Instance.isFPSMode)
                {
                    //Gun
                    var weaponGameObject = TransformUtility.FindGameObjectWithTag(weaponHolder, "Weapon");
                    currentWeapon = weaponGameObject.GetComponent<Weapon>(); //������ �̿��ؼ� ã��(�������������������� ã��� ����, InChild�� �ϳ��� ���������� ã����)

                    if (currentWeapon != null)
                    {
                        currentWeapon.Shoot();
                    }

                    //***�� �ڵ�� ���� �ڵ�(�ֽŹ������� ����)***
                    //currentWeapon?.Shoot(); 

                    //***���� �� �� ���� �����ϱ� ***
                    //var cameraForward = Camera.main.transform.forward.normalized;
                    //cameraForward.y = 0;
                    //transform.forward = cameraForward;
                }
            }

            //***�ڿ�ĳ��***
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

                        Camera_Ctrl.Instance.ShakeCamera(2f, 0.5f);   //���� 0.2, ���� �ð� 1��
                    }
                    //***ī�޶� ��ũ��Ʈ �������� �ּ�ó��***
                    //Vector3 velocity = recoilShakePattern[currentRecoilIndex];
                    //currentRecoilIndex++;
                    //if (currentRecoilIndex >= recoilShakePattern.Count)
                    //{
                    //    currentRecoilIndex = currentRecoilIndex = 0;
                    //}

                    //CameraSystem.Instance.ShakeCamera(velocity,0.2f,1f);
                    //***ī�޶� ��ũ��Ʈ �������� �ּ�ó��***

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

                        Camera_Ctrl.Instance.ShakeCamera(2f, 0.5f);   //���� 0.2, ���� �ð� 1��
                    }
                }
                //To do : �ڿ��� ���� ���⸦ �ٸ���
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                InteractionUI.Instance.SelectNext();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                InteractionUI.Instance.SelectPrev();
            }

        }

        private void LateUpdate()
        {
            CameraRotation();
        }


        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (look.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;

                cinemachineTargetYaw += look.x * deltaTimeMultiplier * cameraHorizontalSpeed;
                cinemachineTargetPitch += look.y * deltaTimeMultiplier * cameraVerticalSpeed;
            }

            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            //if (!isEnableMovement)
                //return;

            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = move.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                //�߷��ۿ�?
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
                //�߷��ۿ�?
                //rigidbody.velocity = new Vector3(currentHorizontalSpeed, rigidbody.velocity.y, 0.0f);

                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                //if (!isStrafe){}
                
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            // ����Ƽ ��ü�� ĳ���� ��Ʈ�ѷ��� �̿�- �ݶ��̴� ����� ž��Ǿ�����
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);   //verticalVelocity => �߷¿� ������ �ִ� ����
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
