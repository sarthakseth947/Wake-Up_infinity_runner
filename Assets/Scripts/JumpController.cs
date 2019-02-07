using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : StateMachineBehaviour {

    /*private GameObject player;

    private CharacterController playerController;
    private PlayerMotor playerMotorObject;*/


	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*player = animator.gameObject;

        playerController = player.GetComponent<CharacterController>();
        playerMotorObject = player.GetComponent<PlayerMotor>();

        player.transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z + 0.1f
        );*/
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.stabilizeFeet = true;

        //Debug.Log(animator.gameObject.transform.position);
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
