using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSubOne : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        p_underwaterSub._instance.transform.localRotation = p_underwaterSub._instance._subOne.transform.rotation;
        p_underwaterSub._instance.transform.localPosition = new Vector3(0, 0.7360001f, 0.5599999f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        p_underwaterSub._instance.transform.localRotation = p_underwaterSub._instance._subOne.transform.rotation;
        p_underwaterSub._instance.transform.localPosition = new Vector3(0, 0.7360001f, 0.5599999f);

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
