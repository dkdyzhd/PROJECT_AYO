using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Ctrl : MonoBehaviour
{
    public static Camera_Ctrl Instance { get; private set; } = null;

    public GameObject m_Player;

    //---마우스 포인터 감지에 필요한 변수
    public LayerMask aimLayer;                  //마우스 포인터가 감지할 레이어 설정
    public float aimRange = 100f;               //레이캐스트 최대 거리
    private Vector3 aimPoint = Vector3.zero;     //마우스 포인터 기준 충돌 지점
    public Vector3 AimPoint => aimPoint;        //AimPoint를 외부에서 접근 가능하도록 공개
    //--- 마우스 포인터 감지에 필요한 변수

    Vector3 m_TargetPos = Vector3.zero;

    //--- 카메라 위치 계산용 변수
    float m_RotH = 0.0f;        //마우스 좌우 조작값 계산용 변수
    float m_RotV = 0.0f;        //마우스 상하 조작값 계산용 변수
    float hSpeed = 5.0f;        //마우스 좌우 회전에 대한 카메라 회전 스피드 설정값
    float vSpeed = 2.4f;        //마우스 상하 회전에 대한 카메라 회전 스피드 설정값
    float vMinLimit = -7.0f;    //위 아래 각도 제한
    float vMaxLimit = 80.0f;    //위 아래 각도 제한
    float zoomSpeed = 1.0f;     //마우스 휠 조작에 대한 줌인아웃 스피드 설정값
    float minDist = 3.0f;       //마우스 줌 인 최소 거리 제한값
    float maxDist = 50.0f;      //마우스 줌 아웃 최대 거리 제한값
    //--- 카메라 위치 계산용 변수

    //--- 주인공을 기준으로 한 상대적인 구좌표계 기준의 초기값
    float m_DefaultRotH = -40.8f;   //0.0f;    //수평 기준의 회전 각도
    float m_DefaultRotV = 41.26f;  //25.0f;   //수직 기준의 회전 각도
    float m_DefaltDist = 12.0f; //5.2f;     //타겟에서 카메라까지의 거리
    //--- 주인공을 기준으로 한 상대적인 구좌표계 기준의 초기값

    //--- 계산에 필요한 변수들...
    float m_CurDistance = 17.0f;            // 현재 주인공에서 카메라까지의 거리
    //Quaternion m_BuffRot;
    private float m_TargetDistance;         // 목표 주인공에서 카메라까지의 거리
    private Quaternion m_CurrentRotation;   // 현재 회전값
    private Quaternion m_TargetRotation;    // 목표 회전값 관리
    Vector3 m_BasicPos = Vector3.zero;
    Vector3 m_BuffPos = Vector3.zero;
    private float rotationSmoothTime = 0.08f; //0.1f;   // 회전 부드럽게 변화시키는 시간
    private float zoomSmoothTime = 0.1f;       // 줌 부드럽게 변화시키는 시간
    private Vector3 rotationVelocity = Vector3.zero; //회전 속도
    private float zoomVelocity = 0.0f;               //줌 속도
    //--- 계산에 필요한 변수들...

    //*** 카메라 쉐이킹 효과에 필요한 변수들
    private float shakeDuration = 0f;   //쉐이킹 유지 시간
    private float shakeIntensity = 0f;  //쉐이킹 강도
    private Vector3 currentShakeOffset = Vector3.zero;  //현재 적용중인 흔들림 값
    private Vector3 targetShakeOffset = Vector3.zero;   //목표 흔들림 값
    private float shakeDampSpeed = 0.1f;    //흔들림 보간 속도(0.1초 기준)
    //*** 카메라 쉐이킹 효과에 필요한 변수들

    private void Awake()
    {
        Instance = this;
    }

    public void InitCamera(GameObject a_Player)
    {
        //m_Player = a_Player;
    }

    //외부에서 카메라 쉐이킹을 시작할 수 있는 함수
    public void ShakeCamera(float intensity, float duration)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_Player == null)
            return;

        m_TargetPos = m_Player.transform.position;
        m_TargetPos.y += 1.4f;

        //--- 카메라 위치 계산 공식 (구좌표계를 직각좌표계로 환산하는 부분)
        m_RotH = m_DefaultRotH; //수평 기준의 회전 각도
        m_RotV = m_DefaultRotV; //수직 기준의 회전 각도
        m_CurDistance = m_DefaltDist; //초기 카메라 거리 적용
        m_TargetDistance = m_CurDistance; //시작은 목표 거리도 같게 시작한다.

        m_CurrentRotation = Quaternion.Euler(m_RotV, m_RotH, 0.0f); //현재 회전값
        m_BasicPos.x = 0.0f;
        m_BasicPos.y = 0.0f;
        m_BasicPos.z = -m_CurDistance;

        m_BuffPos = m_TargetPos + (m_CurrentRotation * m_BasicPos);

        transform.position = m_BuffPos;  //<-- 카메라의 직각좌표계 기준의 위치
        transform.LookAt(m_TargetPos);
        //--- 카메라 위치 계산 공식 (구좌표계를 직각좌표계로 환산하는 부분)
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_Player == null)
            return;

        m_TargetPos = m_Player.transform.position;
        m_TargetPos.y += 0.5f;

        //--- 카메라 회전
        //if (Input.GetMouseButton(1) == true) //마우스 우측 버튼을 누르고 있는 동안
        //{
        //    //마우스를 좌우로 움직였을 때 값
        //    m_RotH += Input.GetAxis("Mouse X") * hSpeed;
        //    //마우스를 위아래로 움직였을 때 값
        //    m_RotV -= Input.GetAxis("Mouse Y") * vSpeed;

        //    m_RotV = ClampAngle(m_RotV, vMinLimit, vMaxLimit);
        //}//if(Input.GetMouseButton(1) == true) //마우스 우측 버튼을 누르고 있는 동안

        //회전 부드럽게 보간 
        m_TargetRotation = Quaternion.Euler(m_RotV, m_RotH, 0.0f);
        m_CurrentRotation = Quaternion.Slerp(m_CurrentRotation, m_TargetRotation,
                                                Time.deltaTime / rotationSmoothTime);
        //Time.deltaTime / rotationSmoothTime은 전체 회전 시간 대비
        //현재 프레임에서 진행해야 할 비율을 계산합니다.
        //나눈 이유
        //회전을 부드럽게 하기 위해서는 전체 회전 시간을 기준으로 매 프레임마다
        //일정한 비율로 회전해야 합니다.
        //예를 들어, rotationSmoothTime이 0.5초라면, 회전은 0.5초에 걸쳐 완료되어야 합니다.
        //매 프레임마다 진행해야 할 회전 비율 t는 Time.deltaTime을 전체 회전 시간으로 나눈 값입니다.

        // 마우스 휠 입력 처리
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0.0f)
        {
            // * 20f 이유 : 마우스 휠 입력 값에 스케일을 조정하여 줌 인/ 아웃의 감도를 높였습니다.
            m_TargetDistance -= scrollInput * zoomSpeed * 20f;
            m_TargetDistance = Mathf.Clamp(m_TargetDistance, minDist, maxDist);
        }

        //Debug.Log(m_CurDistance);

        // 줌 부드럽게 보간
        m_CurDistance = Mathf.SmoothDamp(m_CurDistance, m_TargetDistance, ref zoomVelocity, zoomSmoothTime);
        //Mathf.SmoothDamp 함수를 사용하여 현재 거리에서 목표 거리로 부드럽게 변화시킬 수 있습니다.

        m_BasicPos.x = 0.0f;
        m_BasicPos.y = 0.0f;
        m_BasicPos.z = -m_CurDistance;

        m_BuffPos = m_TargetPos + (m_CurrentRotation * m_BasicPos);

        UpdateAimPoint();   //AimPoint 계산

        //***CameraShaking Logic : 한 번만 흔들림 & 부드러운 흔들림
        if (shakeDuration > 0.0f)
        {
            //목표 흔들림 값을 랜덤으로 설정 (위아래로만)
            targetShakeOffset = Vector3.up * (Random.Range(-1f,1f) * shakeIntensity);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            //흔들림 종료시 목표를 원점으로 설정
            targetShakeOffset = Vector3.zero;
        }

        //현재 흔들림 값을 목표 값으로 부드럽게 보간
        currentShakeOffset = Vector3.Lerp(currentShakeOffset,targetShakeOffset, Time.deltaTime);    

        //***CameraShaking Logic

        //***쉐이킹을 적용한 최종 위치
        transform.position = m_BuffPos + currentShakeOffset; //<--- 카메라의 직각 좌표계 기준의 위치
        transform.LookAt(m_TargetPos);

    }//void LateUpdate()

    private void UpdateAimPoint()
    {
        //마우스 포인터 위치에서 레이 발사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 지면과 교차점을 계산
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);  //y=0 평면 생성
        if(groundPlane.Raycast(ray, out float distance))
        {
            aimPoint = ray.GetPoint(distance); //교차 지점을 aimPoint로 설정
        }
        else
        {
            aimPoint = Vector3.zero;    //교차점이 없을 경우 기본값 설정
        }

        // 이동중인 캐릭터의 위치를 기준으로 보정
        if(m_Player != null)
        {
            Vector3 playerPosition = m_Player.transform.position;
            aimPoint = new Vector3(aimPoint.x, playerPosition.y, aimPoint.z);   //캐릭터 높이로
        }

        //디버깅 : AimPoint 확인
        Debug.DrawLine(ray.origin, aimPoint, Color.red);
        //디버깅 : 캐릭터가 바라보는 방향 확인
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.green);
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max);
    }
}
