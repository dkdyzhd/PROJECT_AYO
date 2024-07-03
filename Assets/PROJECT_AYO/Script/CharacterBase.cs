using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class CharacterBase : MonoBehaviour
    {
        public float currentHp;
        public float maxHp;

        public delegate void OnDamage(float currentHp, float maxHp);    //delegate => �ݹ� �Լ��� ������ ����� (parameter�� �ְ� ������ ���)
        public delegate void OnCharacterDead(); 

        public OnDamage onDamageCallback;   //�Լ��� ����ó�� ����ϴ°�
        public OnCharacterDead onCharacterDead;

        //delegate ���� �ѹ��� �Ҽ��ִ� ���?
        public System.Action<float, float> onDamagedAction;
        public Action OnCharacterDeadAction;    //parameter�� ���� �ݹ��� �� ���

        //delegate�� ���� �̺�Ʈ���ӿ��� => �Ϲ� delegate�� �ܺο��� null�� �����Ҽ�������
        //�̺�Ʈ�� �پ������� +=, -= �� ����
        public event OnDamage onDamagedCallBackEvent;

        public void Damage(float damage)
        {
            currentHp -= damage;

            onDamageCallback(currentHp, maxHp);

            onDamagedAction(currentHp, maxHp);

            if(currentHp <= 0)
            {
                onCharacterDead();
                Destroy(gameObject);
            }
        }
        //Damage Ű �ϳ� ���Ƿ� ���� Ȯ���غ���
        [ContextMenu("Damage Debug")]
        public void DamageDebugButton() // ���� ������ �޴� ������ ������ ��
        {
            Damage(20);
        }
    }
}
