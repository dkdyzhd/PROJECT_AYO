using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class AnimationEventListener : MonoBehaviour
    {
        public System.Action<string> OnTakeAnimationEvent;

        public void OnAnimationEvent(string eventName)
        {
           //if(eventName.Equals(""))
        }
    }
}
