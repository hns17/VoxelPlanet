/**
    @file   GravityAttractor.cs
    @date   2018.07.28
    @author 황준성(hns17.tistory.com)
    @brief  Object를 중력체로 만든다.
*/
using UnityEngine;


/**
    @class   GravityAttractor
    @date   2018.07.28
    @author 황준성(hns17.tistory.com)
    @brief  Object를 대상으로 중력 값이 적용된다.
*/
public class GravityAttractor : MonoBehaviour {
    //중력크기
    [SerializeField] private float gravity = -9.8f;

    //엔진 디폴트 그라비티 사용
    [SerializeField] private bool isPhysicsGravity = false;


    /**
        @brief  지정된 오브젝트에 중력 적용 
    */
    public void Attract(Rigidbody rigid) {
        //역중력
		Vector3 gravityUp = (rigid.position - transform.position).normalized;
        //플레이어 업 벡터
		Vector3 localUp = rigid.transform.up;

        if(isPhysicsGravity)
            gravityUp = Physics.gravity;
        else
            gravityUp = (rigid.position - transform.position).normalized;
        

        //중력 적용
        Physics.gravity = gravityUp * gravity;
        //body.AddForce(gravityUp * gravity);

        //플레이어를 중력 위치에 맞게 회전
        rigid.rotation = Quaternion.FromToRotation(localUp, gravityUp) * rigid.rotation;
	}
}
