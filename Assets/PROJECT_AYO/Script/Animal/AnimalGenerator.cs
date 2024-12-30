using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AnimalGenerator : MonoBehaviour
    {
        public GameObject[] AnimalPrefab;

        float m_SpDelta = 0.0f;     //���� �ֱ� ���� ����
        float m_DiffSpawn = 1.0f;   //���̵��� ���� ���� ���� �ֱ� ����

        private int m_CurrentAnimalCount = 0; // ���� ������ ������ ��
        public int m_MaxAnimalCount = 5;    // ������ �� �ִ� �ִ� ������ ��

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {   //To do : ���� Day�� ����� Day�� �������� ���̵� ���
            m_SpDelta -= Time.deltaTime;
            if(m_SpDelta < 0.0f)
            {
                // ���� ���� ���� �ִ�ġ�� �ʰ��ϸ� ���� �ߴ�
                if (m_CurrentAnimalCount >= m_MaxAnimalCount)
                {
                    return; // �������� �ʰ� ����
                }

                m_SpDelta = m_DiffSpawn;

                int a_MinKd = 0;
                int a_MaxKd = AnimalPrefab.Length;

                int a_Monldx = Random.Range(a_MinKd, a_MaxKd);

                GameObject Go = Instantiate(AnimalPrefab[a_Monldx]);    // ���� ����
                m_CurrentAnimalCount++;                                 // ������ ���� �� ����

                Vector3 a_SpawnPos = Vector3.zero;
                int side = Random.Range(0, 4);      // �� , �� , �Ʒ� , ��
                switch(side)
                {
                    case 0: //����
                        a_SpawnPos = new Vector3(
                            Random.Range(-17,-20),
                            0.0f,
                            Random.Range(1,4)
                            );
                        break;

                    case 1: // ������
                        a_SpawnPos = new Vector3(
                            Random.Range(15,17),
                            0.0f,
                            Random.Range(1,4)
                            );
                        break;

                    case 2: // �Ʒ�
                        a_SpawnPos = new Vector3(
                            Random.Range(1, 4),
                            0.0f,
                            Random.Range(-13, -15)
                            );
                        break;

                    case 3: // ��
                        a_SpawnPos = new Vector3(
                            Random.Range(1, 4),
                            0.0f,
                            Random.Range(13, 15)
                            );
                        break;

                }

                Go.transform.position = a_SpawnPos;

                // �߰��� ������ ���ŵ� �� ���� ���� ����
                //Go.GetComponent<AnimalBehavior>().OnDestroyed += HandleAnimalDestroyed;
            }
        }
    }
}
