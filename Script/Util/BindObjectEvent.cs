/**
    @file   BindObjectEvent.cs
    @date   2018.08.18
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  BindObjectEvent
    @date   2018.08.18
    @author 황준성(hns17.tistory.com)
    @brief  충돌된 오브젝트를 연결(부모로 변경)해 오브젝트의 움직임을 적용한다.
    @detail Temple Scene 의 움직이는 계단 및 VoxelWorld의 회전하는 구름에 사용
*/
public class BindObjectEvent : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        //연결
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        //연결 해제
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = null;
    }
}
