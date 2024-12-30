using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYO
{
    public class DmgTxtCtrl : MonoBehaviour
    {
        public Text DamageText = null;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void InitDamage(float _Damage, Color _Color)
        {
            if(DamageText == null)
                DamageText = GetComponentInChildren<Text>();

            if(DamageText == null)
                return;

            if(_Damage < 0.0f)
            {
                int Dmg = (int)Mathf.Abs(_Damage);
                DamageText.text = "- " + Dmg;
            }
        }
    }
}
