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
            thirst.Subtract(hunger.decayRate * Time.deltaTime); //주기적으로 닳음
            hunger.Subtract(hunger.decayRate * Time.deltaTime);

            if (hunger.curValue == 0.0f || thirst.curValue == 0.0f)    // 배고픔 또는 목마름이 0 ? 체력이 닳는다
                health.Subtract(noHungerHealthDecay * Time.deltaTime);

            if(health.curValue == 0.0f) //체력이 0 ? 사망
            {
                Die();
            }

            //UI화면 업데이트
            health.uiBar.fillAmount = health.GetPercentage();
            thirst.uiBar.fillAmount = thirst.GetPercentage();
            hunger.uiBar.fillAmount = hunger.GetPercentage();

            //Txt 업데이트
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
            Debug.Log("플레이어 사망");
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
