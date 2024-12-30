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
        AI_AggroTrace,      //���� ������ �� ��������
        AI_NormalTrace,     //�Ϲ� ��������
        AI_ReturnPos,       //������ ������ �� ���ڸ��� ���ƿ��� ����
        AI_Attack
    }

    public class AnimalCtrl : MonoBehaviour
    {
        protected AyoPlayerController player;
        protected Rigidbody rig;
        protected Animator anim;

        AnimalAIState animalAIState;      //���º���

        protected float maxHp = 100.0f;
        public float curHp;
        protected float speed = 2.0f;  //�̵��ӵ�
        protected Vector3 dirVec;      //�̵�����
        protected float attackSpeed = 1.4f;    //���ݼӵ�
        protected float attackDistance = 0.5f; //���ݰŸ�
        protected float shootCool;     //�ֱ� ���� ����
        protected float traceDistance = 7.0f;  //��׷� ��� ���� �Ÿ�

        //--- Patrol�� �ʿ��� ������
        Vector3 basePos = Vector3.zero;     //���� �ʱ� ���� ��ġ(������)
        Vector3 patrolTarget = Vector3.zero;    //Patrol�� �������� �� ���� ��ǥ ��ǥ
        Vector3 patrolMoveDir = Vector3.zero;   //Patrol�� �������� �� ���� ����
        Vector3 cacPtAngle = Vector3.zero;
        Vector3 vert;
        Quaternion cacptRot;

        bool movePtOnOff = false;

        float waitTime = 0.0f;      //Patrol �ÿ� ��ǥ���� �����ϸ� ��� ����ϱ� ���� ����
        int angleRan;
        int radiusRan;
        double addTimeCount = 0.0f;     //�̵� �� �����ð� ī��Ʈ�� ����
        double moveDurTime = 0.0f;      //��ǥ������ �����ϴµ� �ɸ��� �ð�

        //--- ���� ����
        Vector3 m_CacVec = Vector3.zero;
        float m_CacDist = 0.0f;

        //--- ���� ��ȯ ���� ����
        protected float stateChangeDelay = 1.0f;  // ���� ��ȯ �� ��� �ð�
        private float stateChangeTimer = 0.0f;   // ���� ��� �ð��� �����ϴ� Ÿ�̸�
        private bool isStateChanging = false;    // ���� ��ȯ ������ ����


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
            {   // ���� ��ȯ���̶��
                stateChangeTimer -= Time.deltaTime;
                if (stateChangeTimer <= 0.0f)
                {
                    isStateChanging = false; // ��� ����
                }
                else
                {
                    return; // ��� �߿��� �ൿ ����
                }
            }

            ChangeAnimation();
            AnimalAI();
            // �Ÿ� üũ
            //Debug.Log($"State: {animalAIState}, Distance to Player: {m_CacDist}, Direction Vector: {dirVec}");
        }

        public void AnimalAI()
        {
            //�÷��̾ �ִ� ���� ���
            m_CacVec = (player.transform.position - transform.position).normalized;
            m_CacVec.y = 0.0f;
            //�÷��̾�� �Ÿ� ���
            m_CacDist = Vector3.Distance(player.transform.position, transform.position);    //**����   

            if (player == null)     //�÷��̾� ��ü�� ��ȿ���� Ȯ��
            {
                //animalAIState = AnimalAIState.AI_Patrol;
                SetState(AnimalAIState.AI_Patrol);  // �ٷ� ���� ���� ���� ��ȯ �Լ��� ���� ���� ����
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
                    if (m_CacDist > traceDistance)      // ���� �������� ����� ������ ��ȯ
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
                        // To do : ���ݷ��� �����
                        MoveTowardsPlayer();
                    }
                    break;

            }

        }

        //���� ��ȯ ���� Ȯ�� �Լ�
        private void SetState(AnimalAIState newState)
        {
            if (animalAIState == newState) return; // ���� ���·� ��ȯ ����

            // ���� ��ȯ ó��
            animalAIState = newState;
            isStateChanging = true; // ��� ���� Ȱ��ȭ
            stateChangeTimer = stateChangeDelay;
        }

        //��ǥ ������ �������� ����ϴ� �Լ�
        private Vector3 CalculateNewPatrolTarget()
        {
            angleRan = Random.Range(30, 301);   // ���� ����
            radiusRan = Random.Range(3, 8);     // ���� �ݰ�
            Quaternion rotation = Quaternion.Euler(0, angleRan, 0); // ���� ������ ȸ���� ���� ����
            Vector3 offset = rotation * Vector3.forward * radiusRan; // ȸ�� ���� ���� * �ݰ� > �̵� ���� ���
            return basePos + offset;    // basePos�� �߽����� ���� �����ϵ��� ��ǥ ���� ����
        }

        public void AI_Patrol()
        {
            if (!movePtOnOff)  
            {   // ��ǥ ������ ���� or ������� �� 
                waitTime -= Time.deltaTime;
                if (waitTime > 0.0f)
                    return;
                //���ο� ��ǥ ����
                waitTime = Random.Range(0.2f, 2.0f);
                patrolTarget = CalculateNewPatrolTarget();
                moveDurTime = Vector3.Distance(transform.position, patrolTarget) / speed;   // �����ϴµ����� �ɸ��� �ð�
                // �ӵ� = �Ÿ� / �ð�     �ӵ� * �ð� = �Ÿ�        �ð� = �Ÿ� / �ӵ�
                addTimeCount = 0.0f;
                movePtOnOff = true;     // �̵� Ȱ��ȭ
            }
            else
            {    // patrol �̵� ���� ��
                addTimeCount += Time.deltaTime;
                if (moveDurTime <= addTimeCount)
                {
                    movePtOnOff = false;    // �̵� �Ϸ�
                }
                else
                {
                    // ��ǥ �������� �̵�
                    Vector3 moveDir = (patrolTarget - transform.position).normalized;
                    transform.Translate((moveDir * Time.deltaTime * speed), Space.World);

                    // ��ǥ ������ �ٶ󺸵���
                    Vector3 lookDir = patrolTarget - transform.position;
                    lookDir.y = 0.0f;
                    if(lookDir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5.0f); // ȸ�� �ӵ�
                    }
                }
            }
        }

        public void MoveTowardsPlayer()
        {
            dirVec = m_CacVec.normalized;   //���뺯���� �����̴� ������ ����
            dirVec.y = 0.0f;
            //�̵� > Patrol ������ �� ���� 1.5��
            rig.velocity = dirVec * speed * 1.5f;

            //�÷��̾� ������ �ٶ󺸵���
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            //TO do : ���ݷ��� �߰�
        }

        string curAnim = "";
        string beforeAnim = "";
        protected virtual void ChangeAnimation()
        {
            //To do : �ִϸ��̼� ����� ������ �� �ִϸ��̼� ����
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
        //    //To do: ���� ���� ���� �������� �ٸ���
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

            //To do : Hpbar UI �����
            //if (HpBarUI != null)
            //    HpBarUI.fillAmount = curHp / maxHp;

            if (curHp <= 0.0f)  //���� ��� ó��
            {
                Die();  //���� ����
                        //Destroy(gameObject);
            }
        }

        public void Die()
        {
            //To do: ������ Die Animation
            //anim.Play("Die");
            Destroy(gameObject, 2.0f); // 2�� �� �ı�
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
