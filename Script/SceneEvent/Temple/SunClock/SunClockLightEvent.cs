/**
    @file       SunClockLightEvent.cs
    @date       2018.12.27
    @author     황준성(hns17.tistory.com)
    @brief      Temple Scene Room1, Room2, Room3에 사용된 컴포넌트
                SunClockEvent에 사용된다. 
    @detail     SunClockEvent에서 SunClockLightEvent로 인한 오브젝트 상태를 검사하여 문을 연다.
                분리한 이유는 오브젝트의 트리거와 상태가 개별로 처리되어야 하기 때문
*/
using System.Collections;
using UnityEngine;

/**
    @class      SunClockLightEvent
    @date       2018.12.27
    @author     황준성(hns17.tistory.com)
    @brief      조건을 on / off한다.
*/
public class SunClockLightEvent : MonoBehaviour {
    //condition on / off에 따른 라이트 표시
    public Light clockLight = null;

    //현재 상태
    private bool turnOnLight = false;

    //조건 on / off 이벤트
    private IEnumerator coroutine;
    
    private void Start()
    {
        coroutine = TurnLightEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        //메시지 표시 후 이벤트 코루틴 시작
        if (other.CompareTag("Player"))
            NotifyMessageEvent.Instance.PrintMainText("눌러본다");

        StartCoroutine(coroutine);
    }

    private void OnTriggerExit(Collider other)
    {
        //메시지 해제 후 이벤트 코루틴 정지
        if (other.CompareTag("Player"))
            NotifyMessageEvent.Instance.MainTextDisable();

        StopCoroutine(coroutine);
    }
    
    /**
        @brief  키 입력에 따른 on / off
    */
    IEnumerator TurnLightEvent()
    {
        while (true) {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E)) {
                TurnLight();
            }
            
        }
    }
    
    /**
        @brief  On / Off 변경 후 Light 상태 변경
    */
    public void TurnLight()
    {
        //조건을 바꾼다.
        turnOnLight = !turnOnLight;

        //on인 경우 Light를 켠다.
        if (turnOnLight)
        {
            UpdateLight();
            iTween.MoveBy(clockLight.gameObject, iTween.Hash("y", 1.0f, "time", 1.0f));
        }
        //off인 경우 Light를 끈다.
        else
        {
            iTween.MoveBy(clockLight.gameObject, iTween.Hash("y", -1.0f, "time", 1.0f,
                                                             "oncomplete", "UpdateLight",
                                                             "oncompletetarget", this.gameObject));
        }
    }

    /**
        @brief  Light On / Off
    */
    private void UpdateLight()
    {
        clockLight.gameObject.SetActive(turnOnLight);
    }
}
