using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Ctrl : MonoBehaviour
{
    public static Camera_Ctrl Instance { get; private set; } = null;

    public GameObject m_Player;

    //---���콺 ������ ������ �ʿ��� ����
    public LayerMask aimLayer;                  //���콺 �����Ͱ� ������ ���̾� ����
    public float aimRange = 100f;               //����ĳ��Ʈ �ִ� �Ÿ�
    private Vector3 aimPoint = Vector3.zero;     //���콺 ������ ���� �浹 ����
    public Vector3 AimPoint => aimPoint;        //AimPoint�� �ܺο��� ���� �����ϵ��� ����
    //--- ���콺 ������ ������ �ʿ��� ����

    Vector3 m_TargetPos = Vector3.zero;

    //--- ī�޶� ��ġ ���� ����
    float m_RotH = 0.0f;        //���콺 �¿� ���۰� ���� ����
    float m_RotV = 0.0f;        //���콺 ���� ���۰� ���� ����
    float hSpeed = 5.0f;        //���콺 �¿� ȸ���� ���� ī�޶� ȸ�� ���ǵ� ������
    float vSpeed = 2.4f;        //���콺 ���� ȸ���� ���� ī�޶� ȸ�� ���ǵ� ������
    float vMinLimit = -7.0f;    //�� �Ʒ� ���� ����
    float vMaxLimit = 80.0f;    //�� �Ʒ� ���� ����
    float zoomSpeed = 1.0f;     //���콺 �� ���ۿ� ���� ���ξƿ� ���ǵ� ������
    float minDist = 3.0f;       //���콺 �� �� �ּ� �Ÿ� ���Ѱ�
    float maxDist = 50.0f;      //���콺 �� �ƿ� �ִ� �Ÿ� ���Ѱ�
    //--- ī�޶� ��ġ ���� ����

    //--- ���ΰ��� �������� �� ������� ����ǥ�� ������ �ʱⰪ
    float m_DefaultRotH = -40.8f;   //0.0f;    //���� ������ ȸ�� ����
    float m_DefaultRotV = 41.26f;  //25.0f;   //���� ������ ȸ�� ����
    float m_DefaltDist = 12.0f; //5.2f;     //Ÿ�ٿ��� ī�޶������ �Ÿ�
    //--- ���ΰ��� �������� �� ������� ����ǥ�� ������ �ʱⰪ

    //--- ��꿡 �ʿ��� ������...
    float m_CurDistance = 17.0f;            // ���� ���ΰ����� ī�޶������ �Ÿ�
    //Quaternion m_BuffRot;
    private float m_TargetDistance;         // ��ǥ ���ΰ����� ī�޶������ �Ÿ�
    private Quaternion m_CurrentRotation;   // ���� ȸ����
    private Quaternion m_TargetRotation;    // ��ǥ ȸ���� ����
    Vector3 m_BasicPos = Vector3.zero;
    Vector3 m_BuffPos = Vector3.zero;
    private float rotationSmoothTime = 0.08f; //0.1f;   // ȸ�� �ε巴�� ��ȭ��Ű�� �ð�
    private float zoomSmoothTime = 0.1f;       // �� �ε巴�� ��ȭ��Ű�� �ð�
    private Vector3 rotationVelocity = Vector3.zero; //ȸ�� �ӵ�
    private float zoomVelocity = 0.0f;               //�� �ӵ�
    //--- ��꿡 �ʿ��� ������...

    //*** ī�޶� ����ŷ ȿ���� �ʿ��� ������
    private float shakeDuration = 0f;   //����ŷ ���� �ð�
    private float shakeIntensity = 0f;  //����ŷ ����
    private Vector3 currentShakeOffset = Vector3.zero;  //���� �������� ��鸲 ��
    private Vector3 targetShakeOffset = Vector3.zero;   //��ǥ ��鸲 ��
    private float shakeDampSpeed = 0.1f;    //��鸲 ���� �ӵ�(0.1�� ����)
    //*** ī�޶� ����ŷ ȿ���� �ʿ��� ������

    private void Awake()
    {
        Instance = this;
    }

    public void InitCamera(GameObject a_Player)
    {
        //m_Player = a_Player;
    }

    //�ܺο��� ī�޶� ����ŷ�� ������ �� �ִ� �Լ�
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

        //--- ī�޶� ��ġ ��� ���� (����ǥ�踦 ������ǥ��� ȯ���ϴ� �κ�)
        m_RotH = m_DefaultRotH; //���� ������ ȸ�� ����
        m_RotV = m_DefaultRotV; //���� ������ ȸ�� ����
        m_CurDistance = m_DefaltDist; //�ʱ� ī�޶� �Ÿ� ����
        m_TargetDistance = m_CurDistance; //������ ��ǥ �Ÿ��� ���� �����Ѵ�.

        m_CurrentRotation = Quaternion.Euler(m_RotV, m_RotH, 0.0f); //���� ȸ����
        m_BasicPos.x = 0.0f;
        m_BasicPos.y = 0.0f;
        m_BasicPos.z = -m_CurDistance;

        m_BuffPos = m_TargetPos + (m_CurrentRotation * m_BasicPos);

        transform.position = m_BuffPos;  //<-- ī�޶��� ������ǥ�� ������ ��ġ
        transform.LookAt(m_TargetPos);
        //--- ī�޶� ��ġ ��� ���� (����ǥ�踦 ������ǥ��� ȯ���ϴ� �κ�)
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

        //--- ī�޶� ȸ��
        //if (Input.GetMouseButton(1) == true) //���콺 ���� ��ư�� ������ �ִ� ����
        //{
        //    //���콺�� �¿�� �������� �� ��
        //    m_RotH += Input.GetAxis("Mouse X") * hSpeed;
        //    //���콺�� ���Ʒ��� �������� �� ��
        //    m_RotV -= Input.GetAxis("Mouse Y") * vSpeed;

        //    m_RotV = ClampAngle(m_RotV, vMinLimit, vMaxLimit);
        //}//if(Input.GetMouseButton(1) == true) //���콺 ���� ��ư�� ������ �ִ� ����

        //ȸ�� �ε巴�� ���� 
        m_TargetRotation = Quaternion.Euler(m_RotV, m_RotH, 0.0f);
        m_CurrentRotation = Quaternion.Slerp(m_CurrentRotation, m_TargetRotation,
                                                Time.deltaTime / rotationSmoothTime);
        //Time.deltaTime / rotationSmoothTime�� ��ü ȸ�� �ð� ���
        //���� �����ӿ��� �����ؾ� �� ������ ����մϴ�.
        //���� ����
        //ȸ���� �ε巴�� �ϱ� ���ؼ��� ��ü ȸ�� �ð��� �������� �� �����Ӹ���
        //������ ������ ȸ���ؾ� �մϴ�.
        //���� ���, rotationSmoothTime�� 0.5�ʶ��, ȸ���� 0.5�ʿ� ���� �Ϸ�Ǿ�� �մϴ�.
        //�� �����Ӹ��� �����ؾ� �� ȸ�� ���� t�� Time.deltaTime�� ��ü ȸ�� �ð����� ���� ���Դϴ�.

        // ���콺 �� �Է� ó��
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0.0f)
        {
            // * 20f ���� : ���콺 �� �Է� ���� �������� �����Ͽ� �� ��/ �ƿ��� ������ �������ϴ�.
            m_TargetDistance -= scrollInput * zoomSpeed * 20f;
            m_TargetDistance = Mathf.Clamp(m_TargetDistance, minDist, maxDist);
        }

        //Debug.Log(m_CurDistance);

        // �� �ε巴�� ����
        m_CurDistance = Mathf.SmoothDamp(m_CurDistance, m_TargetDistance, ref zoomVelocity, zoomSmoothTime);
        //Mathf.SmoothDamp �Լ��� ����Ͽ� ���� �Ÿ����� ��ǥ �Ÿ��� �ε巴�� ��ȭ��ų �� �ֽ��ϴ�.

        m_BasicPos.x = 0.0f;
        m_BasicPos.y = 0.0f;
        m_BasicPos.z = -m_CurDistance;

        m_BuffPos = m_TargetPos + (m_CurrentRotation * m_BasicPos);

        UpdateAimPoint();   //AimPoint ���

        //***CameraShaking Logic : �� ���� ��鸲 & �ε巯�� ��鸲
        if (shakeDuration > 0.0f)
        {
            //��ǥ ��鸲 ���� �������� ���� (���Ʒ��θ�)
            targetShakeOffset = Vector3.up * (Random.Range(-1f,1f) * shakeIntensity);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            //��鸲 ����� ��ǥ�� �������� ����
            targetShakeOffset = Vector3.zero;
        }

        //���� ��鸲 ���� ��ǥ ������ �ε巴�� ����
        currentShakeOffset = Vector3.Lerp(currentShakeOffset,targetShakeOffset, Time.deltaTime);    

        //***CameraShaking Logic

        //***����ŷ�� ������ ���� ��ġ
        transform.position = m_BuffPos + currentShakeOffset; //<--- ī�޶��� ���� ��ǥ�� ������ ��ġ
        transform.LookAt(m_TargetPos);

    }//void LateUpdate()

    private void UpdateAimPoint()
    {
        //���콺 ������ ��ġ���� ���� �߻�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ����� �������� ���
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);  //y=0 ��� ����
        if(groundPlane.Raycast(ray, out float distance))
        {
            aimPoint = ray.GetPoint(distance); //���� ������ aimPoint�� ����
        }
        else
        {
            aimPoint = Vector3.zero;    //�������� ���� ��� �⺻�� ����
        }

        // �̵����� ĳ������ ��ġ�� �������� ����
        if(m_Player != null)
        {
            Vector3 playerPosition = m_Player.transform.position;
            aimPoint = new Vector3(aimPoint.x, playerPosition.y, aimPoint.z);   //ĳ���� ���̷�
        }

        //����� : AimPoint Ȯ��
        Debug.DrawLine(ray.origin, aimPoint, Color.red);
        //����� : ĳ���Ͱ� �ٶ󺸴� ���� Ȯ��
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
