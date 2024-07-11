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
        public float curValue;  // ���簪
        [HideInInspector]
        public int intcurValue; // ���簪 ������
        public float maxValue;  // �ִ�
        public float startValue;    // ���۰�
        public float regenRate; // ȸ����
        public float decayRate; // ���ҷ�
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

        // �ۼ�Ʈ������ ��κ� 0�� 1�� ����Ѵ�.
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
