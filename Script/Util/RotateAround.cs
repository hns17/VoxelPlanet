/**
    @file   RotateAround.cs
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;


/**
    @class  RotateAround
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  Target의 주위를 회전
*/
public class RotateAround : MonoBehaviour {
    //중심이 될 오브젝트
    public Transform target = null;

    //회전 정보
    public Vector3 aixs;

    //회전 속도
    public float speed;

    //중심을 바라 볼 것인가?
    public bool isLookAtTarget = false;


    private void FixedUpdate()
    {
        if (target == null)
            return;
        
        if (isLookAtTarget)
            transform.LookAt(target.position);

        transform.RotateAround(target.position, aixs, speed * Time.fixedDeltaTime);
    }
}
