using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class LocomotionBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var controller = animator.transform.root.GetComponent<AyoPlayerController>();
            
        }
    }
}
