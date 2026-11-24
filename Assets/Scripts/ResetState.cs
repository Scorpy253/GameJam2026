using Unity.VisualScripting;
using UnityEngine;

public class ResetState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetInteger("State") == 0 || animator.GetInteger("State") == 5) //Orange
        {
            animator.SetInteger("Page", 1);
        }
        if (animator.GetInteger("State") == 2 || animator.GetInteger("State") == 4) //Blue  
        {
            animator.SetInteger("Page", 3);
        }
        if (animator.GetInteger("State") == 3 || animator.GetInteger("State") == 1) //Purple
        {
            animator.SetInteger("Page", 2);
        }
        SwitchPages switchPages = animator.GetComponent<SwitchPages>();
        if (switchPages != null)
        {
            switchPages.FixData(animator.GetInteger("Page"));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state

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
