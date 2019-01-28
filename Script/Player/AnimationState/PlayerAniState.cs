/**
    @file   PlayerAniState.cs
    @class  PlayerAniState
    @date   2019.12.23
    @author 황준성(hns17.tistory.com)
    @brief  Player 기본 애니메이션 상태 이벤트
*/
using UnityEngine;

public class PlayerAniState : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var info = PlayerInfo.Instance;

        //애니메이션 시작시 플레이어 상태 변경
        if (stateInfo.IsName("Idle"))
            info.State = PlayerInfo.PlayerState.IDLE;
        else if (stateInfo.IsName("Jump"))
            info.State = PlayerInfo.PlayerState.JUMP;
        else if (stateInfo.IsName("Fall"))
            info.State = PlayerInfo.PlayerState.FALL;
        else if (stateInfo.IsName("Run"))
            info.State = PlayerInfo.PlayerState.RUN;
        else if (stateInfo.IsName("Walk"))
            info.State = PlayerInfo.PlayerState.WALK;
        else if (stateInfo.IsName("WakeUp"))
            info.State = PlayerInfo.PlayerState.WAKEUP;
        else if (stateInfo.IsName("Roll"))
            info.State = PlayerInfo.PlayerState.ROLL;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        var info = PlayerInfo.Instance;

        //캐릭터는 일정 높이에서 낙하하면 착지시 구른다.
        //rollingHeight : 구르기 착지 낙하 높이, 클수록 더 높은 곳에서 낙하해야 실행된다.
        float rollingHeight = 2.0f;

        bool isGround = info.IsGround(rollingHeight, 0.15f);
        animator.SetBool("IsGround", isGround);
        
        //점프 애니메이션에 맞게 충돌체 크기 및 위치 조정
        if (stateInfo.IsName("Jump") || stateInfo.IsName("Fall"))
        {
            if(stateInfo.IsName("Jump") && !isGround)
                animator.Play("Fall");

            var coll = PlayerInfo.Instance.Collider;
            coll.center = new Vector3(0, animator.GetFloat("ColliderPosition"), 0);
            coll.height = animator.GetFloat("ColliderHeight");
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //점프 상태면 점프 종료
        if (stateInfo.IsName("Jump"))
            animator.SetBool("IsJump", false);
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
