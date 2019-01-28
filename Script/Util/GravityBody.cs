/**
    @file   GravityBody.cs
    @date   2018.07.28
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
/**
    @class  GravityBody
    @date   2018.07.28
    @author 황준성(hns17.tistory.com)
    @brief  지정된 중력체를 기준으로 오브젝트에 중력을 부여한다.
*/
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour {
    //obj rigidbody
	[SerializeField] private Rigidbody rigid;
    //중력체
	[SerializeField] private GravityAttractor attract;
	
	void Awake () {
        rigid = GetComponent<Rigidbody> ();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
    public GravityAttractor Attract
    {
        set { attract = value; }
    }

	void FixedUpdate () {
        if (attract == null) {
            Physics.gravity = -Vector3.up * 9.8f;
            return;
        }
        
        //플레이어의 중력 및 회전계산 후 적용
        attract.Attract(rigid);
	}
}