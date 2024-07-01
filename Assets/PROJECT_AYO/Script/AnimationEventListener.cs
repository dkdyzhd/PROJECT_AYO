using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AnimationEventListener : MonoBehaviour
    {
        public System.Action<string> OnTakenAnimationEvent;

        public void OnAnimationEvent(string eventName)
        {
            //if(eventName.Equals(""))
            OnTakenAnimationEvent?.Invoke(eventName);
        }
    }
}
