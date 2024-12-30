using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AnimalGenerator : MonoBehaviour
    {
        public GameObject[] AnimalPrefab;

        float m_SpDelta = 0.0f;     //스폰 주기 계산용 변수
        float m_DiffSpawn = 1.0f;   //난이도에 따른 동물 스폰 주기 변수

        private int m_CurrentAnimalCount = 0; // 현재 스폰된 동물의 수
        public int m_MaxAnimalCount = 5;    // 스폰될 수 있는 최대 동물의 수

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {   //To do : 생존 Day를 만들어 Day가 지날수록 난이도 상승
            m_SpDelta -= Time.deltaTime;
            if(m_SpDelta < 0.0f)
            {
                // 현재 동물 수가 최대치를 초과하면 스폰 중단
                if (m_CurrentAnimalCount >= m_MaxAnimalCount)
                {
                    return; // 스폰하지 않고 종료
                }

                m_SpDelta = m_DiffSpawn;

                int a_MinKd = 0;
                int a_MaxKd = AnimalPrefab.Length;

                int a_Monldx = Random.Range(a_MinKd, a_MaxKd);

                GameObject Go = Instantiate(AnimalPrefab[a_Monldx]);    // 동물 스폰
                m_CurrentAnimalCount++;                                 // 스폰된 동물 수 증가

                Vector3 a_SpawnPos = Vector3.zero;
                int side = Random.Range(0, 4);      // 왼 , 오 , 아래 , 위
                switch(side)
                {
                    case 0: //왼쪽
                        a_SpawnPos = new Vector3(
                            Random.Range(-17,-20),
                            0.0f,
                            Random.Range(1,4)
                            );
                        break;

                    case 1: // 오른쪽
                        a_SpawnPos = new Vector3(
                            Random.Range(15,17),
                            0.0f,
                            Random.Range(1,4)
                            );
                        break;

                    case 2: // 아래
                        a_SpawnPos = new Vector3(
                            Random.Range(1, 4),
                            0.0f,
                            Random.Range(-13, -15)
                            );
                        break;

                    case 3: // 위
                        a_SpawnPos = new Vector3(
                            Random.Range(1, 4),
                            0.0f,
                            Random.Range(13, 15)
                            );
                        break;

                }

                Go.transform.position = a_SpawnPos;

                // 추가로 동물이 제거될 때 동물 수를 감소
                //Go.GetComponent<AnimalBehavior>().OnDestroyed += HandleAnimalDestroyed;
            }
        }
    }
}
