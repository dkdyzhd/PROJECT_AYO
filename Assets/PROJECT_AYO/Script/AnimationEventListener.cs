using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AnimationEventListener : MonoBehaviour
    {
        public System.Action<string> OnTakenAnimationEvent;

        public void OnAnimationEvent(string eventName)  //애니메이터 기능 이름
        {
            OnTakenAnimationEvent?.Invoke(eventName);
        }

        void EnableWeaponCollision()
        {
            QuickSlotController.Instance.EnableWeaponCollision();
        }

        void DisableWeaponCollision()
        {
            QuickSlotController.Instance.DisableWeaponCollision();
        }
    }
}
