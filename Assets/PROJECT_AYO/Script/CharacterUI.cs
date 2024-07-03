using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class CharacterUI : MonoBehaviour
    {
        public Image hpBar;

        public CharacterBase linkedCharacter;

        private void Start()
        {
            linkedCharacter.onDamageCallback += RefreshHpBar;
            linkedCharacter.onDamagedAction += RefreshHpBar;
        }

        public void RefreshHpBar(float currentHp, float maxHp)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
