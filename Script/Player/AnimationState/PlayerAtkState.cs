/**
    @file   PlayerAtkState.cs
    @class  PlayerAtkState
    @date   2019.01.02
    @author 황준성(hns17.tistory.com)
    @brief  Player 공격에 의한 상태 변화
*/
using UnityEngine;

public class PlayerAtkState : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //공격시 자세 고정, 상태 변경
        var info = PlayerInfo.Instance;
        info.IsLock = true;
        info.State = PlayerInfo.PlayerState.ATTACK;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //공격 종료시 대기상태로
        if (!animator.GetBool("IsBladeAttack")
            && !animator.GetBool("IsCubeAttack"))
        {
            var info = PlayerInfo.Instance;
            info.IsLock = false;
            info.State = PlayerInfo.PlayerState.IDLE;
        }
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
