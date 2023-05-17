using UnityEngine;

namespace Game
{
    public class SmbEventsLogger : StateMachineBehaviour
    {
        [SerializeField] private string _marker;


        private void Log(string msg)
        {
            Debug.Log($"SMB {_marker}: {msg}. Frame: {Time.frameCount}");
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log(nameof(OnStateEnter));
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log(nameof(OnStateUpdate));
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log(nameof(OnStateExit));
        }


        // OnStateMove is called right after Animator.OnAnimatorMove()
        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Implement code that processes and affects root motion
            Log(nameof(OnStateMove));
        }

        // OnStateIK is called right after Animator.OnAnimatorIK()
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Implement code that sets up animation IK (inverse kinematics)
            Log(nameof(OnStateIK));
        }
    }
}
