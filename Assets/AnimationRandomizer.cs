using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEditor;
using UnityEngine;

public class AnimationRandomizer : StateMachineBehaviour {
    [SerializeField] AnimationClip[] animationClips = null;

    float minimumRandom = 3f;
    // float minimumLimit = 15f;
    float maximumRandom = 15f;
    // float maximumLimit = 40f;
    float clipLengthInterval;

    ThirdPersonCharacterController player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        player = FindObjectOfType<ThirdPersonCharacterController> ();

        int random = Random.Range (0, animationClips.Length);
        animator.SetFloat ("idleMotion", random);
        AnimationClip animationClip = animationClips[random];

        clipLengthInterval = Random.Range (minimumRandom, maximumRandom);
        player.SetIdleInterveral (animationClip.length + clipLengthInterval);
    }

    // private void OnGUI () {
    //     EditorGUILayout.LabelField ("Min Ran Value to add to clip length interval: ", minimumRandom.ToString ());
    //     EditorGUILayout.LabelField ("Max Ran Value to add to clip length interval: ", maximumRandom.ToString ());
    //     EditorGUILayout.MinMaxSlider (ref minimumRandom, ref maximumRandom, minimumLimit, maximumLimit);
    // }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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