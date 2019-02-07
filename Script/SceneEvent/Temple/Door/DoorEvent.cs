/**
    @file   DoorEvent.cs
    @date   2018.12.27
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;


/**
    @class  DoorEvent
    @date   2018.12.27
    @author 황준성(hns17.tistory.com)
    @brief  Temple scene에 사용된 문 open 이벤트
*/
public class DoorEvent : MonoBehaviour {
    public Animator anim;
    public bool isLock = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //문이 잠긴 경우 메시지를 보여준다.
        if (isLock) {
            NotifyMessageEvent.Instance.PrintMainText("어딘가 문을 여는 방법이...");
            return;
        }

        if (anim == null)
            return;
        
        //문을 연다
        anim.SetTrigger("IsOpen");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //메시지를 제거한다.
        NotifyMessageEvent.Instance.MainTextDisable();
    }
}
