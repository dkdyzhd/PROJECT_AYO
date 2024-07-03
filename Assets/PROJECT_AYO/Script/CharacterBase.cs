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

        public delegate void OnDamage(float currentHp, float maxHp);    //delegate => 콜백 함수의 원형을 만들고 (parameter를 넣고 싶을때 사용)
        public delegate void OnCharacterDead(); 

        public OnDamage onDamageCallback;   //함수를 변수처럼 사용하는것
        public OnCharacterDead onCharacterDead;

        //delegate 말고 한번에 할수있는 방법?
        public System.Action<float, float> onDamagedAction;
        public Action OnCharacterDeadAction;    //parameter를 없이 콜백을 할 경우

        //delegate를 통한 이벤트ㅋㅣ워드 => 일반 delegate는 외부에서 null로 통제할수있지만
        //이벤트가 붙어있으면 +=, -= 만 가능
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
        //Damage 키 하나 임의로 만들어서 확인해보기
        [ContextMenu("Damage Debug")]
        public void DamageDebugButton() // 실제 데미지 받는 로직을 넣으면 됨
        {
            Damage(20);
        }
    }
}
