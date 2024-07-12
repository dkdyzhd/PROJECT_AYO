using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class PlayerCondition : MonoBehaviour, IDamagable
    {
        public Condition health;
        public Condition thirst;
        public Condition hunger;

        public float noHungerHealthDecay;
        public float noThirstHealthDecay;

        private void Start()
        {
            health.curValue = health.startValue;
            thirst.curValue = thirst.startValue;
            hunger.curValue = hunger.startValue;
        }

        private void Update()
        {
            thirst.Subtract(hunger.decayRate * Time.deltaTime); //�ֱ������� ����
            hunger.Subtract(hunger.decayRate * Time.deltaTime);

            if (hunger.curValue == 0.0f || thirst.curValue == 0.0f)    // ����� �Ǵ� �񸶸��� 0 ? ü���� ��´�
                health.Subtract(noHungerHealthDecay * Time.deltaTime);

            if(health.curValue == 0.0f) //ü���� 0 ? ���
            {
                Die();
            }

            //UIȭ�� ������Ʈ
            health.uiBar.fillAmount = health.GetPercentage();
            thirst.uiBar.fillAmount = thirst.GetPercentage();
            hunger.uiBar.fillAmount = hunger.GetPercentage();

            //Txt ������Ʈ
            health.valueText.text = health.ValueText();
            thirst.valueText.text = thirst.ValueText();
            hunger.valueText.text = hunger.ValueText();
        }

        public void CurValueToInt()
        {
            health.intcurValue = Mathf.FloorToInt(health.curValue);
        }

        void Die()
        {
            Debug.Log("�÷��̾� ���");
        }

        public void Heal(float amount)
        {
            health.Add(amount);
        }

        public void Eat(float amount)
        {
            health.Add(amount);
            thirst.Add(amount);
            hunger.Add(amount);
        }


        public void OnTakePhysicalDamage()
        {
            
        }

        
    }
}
