/**
    @file   FollowTarget
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  지정된 타겟을 따라간다.
*/
using UnityEngine;

/**
    @class  FollowTarget
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  지정된 타겟의 현재 위치를 목표로 이동
*/
public class FollowTarget : MonoBehaviour {
    //목표
    [SerializeField] private Transform target;

    //이동 속도
    [SerializeField] private float smoothStep = 2f;

    //타겟과 오브젝트간 최초 거리 유지
    [SerializeField] private bool isInterval = true;

    //타겟의 최초 위치
    private Vector3 originPos;

    private void Start()
    {
        originPos = target.position;
        
        if(isInterval)
            originPos = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = target.position + target.rotation * originPos;
        transform.position = Vector3.Lerp(transform.position,targetPos, Time.deltaTime * smoothStep);
    }
}
