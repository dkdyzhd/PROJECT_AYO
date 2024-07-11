using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    [System.Serializable]
    public class Condition
    {
        [HideInInspector]
        public float curValue;  // 현재값
        [HideInInspector]
        public int intcurValue; // 현재값 정수형
        public float maxValue;  // 최댓값
        public float startValue;    // 시작값
        public float regenRate; // 회복률
        public float decayRate; // 감소률
        public Image uiBar;
        public TMPro.TextMeshProUGUI valueText;

        public void Add(float amount)
        {
            curValue = Mathf.Min(curValue + amount, maxValue);
        }

        public void Subtract(float amount)
        {
            curValue = Mathf.Max(curValue - amount, 0.0f);
        }

        // 퍼센트값들은 대부분 0과 1을 사용한다.
        public float GetPercentage()
        {
            return curValue / maxValue;
        }

        public string ValueText()
        {
            intcurValue = Mathf.FloorToInt(curValue);
            return intcurValue.ToString();
        }

    }
}
