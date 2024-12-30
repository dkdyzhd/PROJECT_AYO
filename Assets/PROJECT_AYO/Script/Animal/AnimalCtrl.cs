using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public enum AnimalAIState
    {
        AI_Idle,
        AI_Patrol,
        AI_AggroTrace,      //공격 당했을 때 추적상태
        AI_NormalTrace,     //일반 추적상태
        AI_ReturnPos,       //추적을 놓쳤을 때 제자리로 돌아오는 상태
        AI_Attack
    }

    public class AnimalCtrl : MonoBehaviour
    {
        protected AyoPlayerController player;
        protected Rigidbody rig;
        protected Animator anim;

        AnimalAIState animalAIState;      //상태변수

        protected float maxHp = 100.0f;
        public float curHp;
        protected float speed = 2.0f;  //이동속도
        protected Vector3 dirVec;      //이동방향
        protected float attackSpeed = 1.4f;    //공격속도
        protected float attackDistance = 0.5f; //공격거리
        protected float shootCool;     //주기 계산용 변수
        protected float traceDistance = 7.0f;  //어그로 대상 추적 거리

        //--- Patrol에 필요한 변수들
        Vector3 basePos = Vector3.zero;     //몬스터 초기 스폰 위치(기준점)
        Vector3 patrolTarget = Vector3.zero;    //Patrol시 움직여야 될 다음 목표 좌표
        Vector3 patrolMoveDir = Vector3.zero;   //Patrol시 움직여야 될 방향 벡터
        Vector3 cacPtAngle = Vector3.zero;
        Vector3 vert;
        Quaternion cacptRot;

        bool movePtOnOff = false;

        float waitTime = 0.0f;      //Patrol 시에 목표점에 도달하면 잠시 대기하기 위한 변수
        int angleRan;
        int radiusRan;
        double addTimeCount = 0.0f;     //이동 총 누적시간 카운트용 변수
        double moveDurTime = 0.0f;      //목표점까지 도착하는데 걸리는 시간

        //--- 계산용 변수
        Vector3 m_CacVec = Vector3.zero;
        float m_CacDist = 0.0f;

        //--- 상태 전환 대기용 변수
        protected float stateChangeDelay = 1.0f;  // 상태 전환 후 대기 시간
        private float stateChangeTimer = 0.0f;   // 현재 대기 시간을 추적하는 타이머
        private bool isStateChanging = false;    // 상태 전환 중인지 여부


        // Start is called before the first frame update
        protected virtual void Start()
        {
            player = GameObject.FindObjectOfType<AyoPlayerController>();
            rig = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();

            animalAIState = AnimalAIState.AI_Patrol;
            basePos = transform.position;
            movePtOnOff = false;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (isStateChanging)
            {   // 상태 변환중이라면
                stateChangeTimer -= Time.deltaTime;
                if (stateChangeTimer <= 0.0f)
                {
                    isStateChanging = false; // 대기 종료
                }
                else
                {
                    return; // 대기 중에는 행동 중지
                }
            }

            ChangeAnimation();
            AnimalAI();
            // 거리 체크
            //Debug.Log($"State: {animalAIState}, Distance to Player: {m_CacDist}, Direction Vector: {dirVec}");
        }

        public void AnimalAI()
        {
            //플레이어가 있는 방향 계산
            m_CacVec = (player.transform.position - transform.position).normalized;
            m_CacVec.y = 0.0f;
            //플레이어와 거리 계산
            m_CacDist = Vector3.Distance(player.transform.position, transform.position);    //**수정   

            if (player == null)     //플레이어 객체가 유효한지 확인
            {
                //animalAIState = AnimalAIState.AI_Patrol;
                SetState(AnimalAIState.AI_Patrol);  // 바로 지정 말고 상태 변환 함수를 통해 상태 변경
                return;
            }

            switch(animalAIState)
            {
                case AnimalAIState.AI_Patrol:
                    if (m_CacDist <= traceDistance)
                    {
                        SetState(AnimalAIState.AI_AggroTrace);
                    }
                    else
                    {
                        AI_Patrol();
                    }
                    break;

                case AnimalAIState.AI_AggroTrace:
                    if (m_CacDist > traceDistance)      // 추적 범위에서 벗어나면 순찰로 전환
                    {
                        SetState(AnimalAIState.AI_Patrol);
                    }
                    else if(m_CacDist <= attackDistance)
                    {
                        SetState(AnimalAIState.AI_Attack);
                    }
                    else
                    {
                        MoveTowardsPlayer();
                    }
                    break;

                case AnimalAIState.AI_Attack:
                    if(m_CacDist > attackDistance)
                    {
                        SetState(AnimalAIState.AI_AggroTrace);
                    }
                    else
                    {
                        // To do : 공격로직 만들기
                        MoveTowardsPlayer();
                    }
                    break;

            }

        }

        //상태 변환 여부 확인 함수
        private void SetState(AnimalAIState newState)
        {
            if (animalAIState == newState) return; // 동일 상태로 전환 방지

            // 상태 전환 처리
            animalAIState = newState;
            isStateChanging = true; // 대기 상태 활성화
            stateChangeTimer = stateChangeDelay;
        }

        //목표 지점을 랜덤으로 계산하는 함수
        private Vector3 CalculateNewPatrolTarget()
        {
            angleRan = Random.Range(30, 301);   // 랜덤 각도
            radiusRan = Random.Range(3, 8);     // 랜덤 반경
            Quaternion rotation = Quaternion.Euler(0, angleRan, 0); // 랜덤 각도로 회전한 방향 벡터
            Vector3 offset = rotation * Vector3.forward * radiusRan; // 회전 방향 벡터 * 반경 > 이동 벡터 계산
            return basePos + offset;    // basePos를 중심으로 원형 순찰하도록 목표 지점 설정
        }

        public void AI_Patrol()
        {
            if (!movePtOnOff)  
            {   // 목표 지점에 도달 or 대기중일 때 
                waitTime -= Time.deltaTime;
                if (waitTime > 0.0f)
                    return;
                //새로운 목표 설정
                waitTime = Random.Range(0.2f, 2.0f);
                patrolTarget = CalculateNewPatrolTarget();
                moveDurTime = Vector3.Distance(transform.position, patrolTarget) / speed;   // 도착하는데까지 걸리는 시간
                // 속도 = 거리 / 시간     속도 * 시간 = 거리        시간 = 거리 / 속도
                addTimeCount = 0.0f;
                movePtOnOff = true;     // 이동 활성화
            }
            else
            {    // patrol 이동 중일 때
                addTimeCount += Time.deltaTime;
                if (moveDurTime <= addTimeCount)
                {
                    movePtOnOff = false;    // 이동 완료
                }
                else
                {
                    // 목표 지점으로 이동
                    Vector3 moveDir = (patrolTarget - transform.position).normalized;
                    transform.Translate((moveDir * Time.deltaTime * speed), Space.World);

                    // 목표 지점을 바라보도록
                    Vector3 lookDir = patrolTarget - transform.position;
                    lookDir.y = 0.0f;
                    if(lookDir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5.0f); // 회전 속도
                    }
                }
            }
        }

        public void MoveTowardsPlayer()
        {
            dirVec = m_CacVec.normalized;   //계산용변수를 움직이는 변수에 대입
            dirVec.y = 0.0f;
            //이동 > Patrol 상태일 때 보다 1.5배
            rig.velocity = dirVec * speed * 1.5f;

            //플레이어 쪽으로 바라보도록
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            //TO do : 공격로직 추가
        }

        string curAnim = "";
        string beforeAnim = "";
        protected virtual void ChangeAnimation()
        {
            //To do : 애니메이션 만들고 움직일 때 애니메이션 적용
            if(0.01f < dirVec.magnitude)
            {
                curAnim = "Animal_Walk";
            }
            else
            {
                curAnim = "Animal_Idle";
            }

            if(beforeAnim != curAnim)
            {
                anim.Play(curAnim);
                beforeAnim = curAnim;
            }
        }

        //protected virtual void OnCollisionEnter(Collision collision)
        //{
        //    //To do: 무기 종류 별로 데미지를 다르게
        //    if (collision.gameObject.CompareTag("Weapon"))
        //    {
        //        TakeDamage(20);
        //    }
        //}

        public void TakeDamage(float a_Value)
        {
            if (curHp <= 0.0f)
                return;

            curHp -= a_Value;
            if (curHp < 0.0f)
                curHp = 0.0f;

            //To do : Hpbar UI 만들기
            //if (HpBarUI != null)
            //    HpBarUI.fillAmount = curHp / maxHp;

            if (curHp <= 0.0f)  //동물 사망 처리
            {
                Die();  //동물 제거
                        //Destroy(gameObject);
            }
        }

        public void Die()
        {
            //To do: 동물의 Die Animation
            //anim.Play("Die");
            Destroy(gameObject, 2.0f); // 2초 후 파괴
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
