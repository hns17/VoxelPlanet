/**
    @file   PlayerMove.cs
    @class  PlayerMove
    @date   2018.08.04
    @author 황준성(hns17.tistory.com)
    @brief  플레이어의 이동 관련 이벤트
*/
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    //회전속도
	[SerializeField] private float turnSpeed = 10.0f;

    //이동방향이 카메라 기준인가?
	[SerializeField] private bool isCamBaseDirection = true;
   
    //이동 방향
    private Vector3 moveDirection;
   
    
	void Update() {
        var info = PlayerInfo.Instance;

        //움직일수 없는 상태이면...
        if (info.IsLock) {
            moveDirection = Vector3.zero;

            //회피 상태인경우 전방으로 도약
            if (info.State == PlayerInfo.PlayerState.ROLL)
                moveDirection = info.Model.transform.forward * 5f;

            return;
        }

        //이동 이벤트
        UpdateMoveEvent();

        //점프 이벤트
        if (info.IsGround(1.0f, 0.005f))
            UpdateJumpEvent();
        //플레이어가 OffGround 상태면 낙하 움직임 적용
        else
            UpdateFloatingEvent();
    }

    /*
        @brief : 캐릭터의 이동 및 회전을 업데이트
    **/
    private void FixedUpdate()
    {

        var info = PlayerInfo.Instance;
        var model = info.Model;
        var rigid = info.Rigid;

        if (info.State == PlayerInfo.PlayerState.JUMP)
            rigid.AddForce(transform.up * info.Anim.GetFloat("JumpForce"));

        if (moveDirection == Vector3.zero)
            return;
        
        //캐릭터 이동
        transform.position += moveDirection * Time.fixedDeltaTime;

        //캐릭터 모델 회전
        //이동방향으로 회전
        model.rotation = Quaternion.Slerp(
            Quaternion.LookRotation(model.forward, rigid.transform.up), 
            Quaternion.LookRotation(moveDirection.normalized, rigid.transform.up),
            Time.fixedDeltaTime * turnSpeed);
        
    }


    /*
        @brief : 캐릭터의 이동 이벤트
    **/
    void UpdateMoveEvent() {
        var spd = PlayerInfo.Instance.BaseSpeed;

        var inputVertical = Input.GetAxis("Vertical");
        var inputHorizontal = Input.GetAxis("Horizontal");

        
        moveDirection = Vector3.zero;
        
        //이동하지 않으면 반환
        if (Mathf.Abs(inputVertical) + Mathf.Abs(inputHorizontal) <= 0)
        {
            PlayerAnimation.Instance.Idle();
            return;
        }

        

        //캐릭터 방향 기준으로 이동
        if (!isCamBaseDirection) {
            moveDirection = (transform.forward * inputVertical + transform.right * inputHorizontal).normalized;
        }
        //카메라 기준 이동
        else {
            Transform tfCam = Camera.main.transform;
            //카메라의 upVec를 player의 upVec에 맞춘다.
            //y축이 up인 경우 y를 0으로 하면 되지만, 아닐 경우(중력방향이 다른)를 위해
            tfCam.rotation = Quaternion.FromToRotation(tfCam.up, transform.up) * tfCam.rotation;
            moveDirection = (tfCam.forward * inputVertical + tfCam.right * inputHorizontal).normalized;
        }

        //달리기 상태로 전환
        bool isRun = false;
        if (Input.GetKey(KeyCode.LeftShift)) {
            spd *= 3f;
            isRun = true;
        }

        moveDirection *= spd;

        if (isRun)
            PlayerAnimation.Instance.Run();
        else
            PlayerAnimation.Instance.Walk();
    }

   
    /*
        @brief : Jump Event
    **/
    void UpdateJumpEvent()
    {

        var state = PlayerInfo.Instance.State;

        if (state.Equals(PlayerInfo.PlayerState.JUMP) ||
            state.Equals(PlayerInfo.PlayerState.LANDING))
        {
            moveDirection = Vector3.zero;
            return;
        }
        
        if (Input.GetKey(KeyCode.Space))
            PlayerAnimation.Instance.Jump();
        
    }
    
    /**
        @brief  공중에서 Player의 움직임 
    */
    void UpdateFloatingEvent()
    {
        var spd = PlayerInfo.Instance.BaseSpeed;
        moveDirection = moveDirection.normalized * spd * 2.5f;

    }

}


