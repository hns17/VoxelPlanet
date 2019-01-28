/**
    @file       SunClockInfo.cs
    @date       2018.12.27
    @author     황준성(hns17.tistory.com)
    @brief      Temple Scene Room1, Room2, Room3에 사용된 컴포넌트
                지정된 조건을 클리어 하면 문이 열린다.
    @detail     원래는 Room1의 문 Open 해시계 트랩으로 만들었지만 다른 곳에서도 쓰고 있다.
                클래스나 변수 이름을 바꾸는게 좋을 것 같다.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
    @class      SunClockInfo
    @date       2018.12.27
    @author     황준성(hns17.tistory.com)
    @brief      Door Open Condition
*/
[System.Serializable]
public class SunClockInfo
{
    //Open 조건
    public bool hourEnable = false;
    public bool minuteEnable = false;

    //현재 상태를 표시할 Obj
    public GameObject tfHour = null;
    public GameObject tfMinute = null;
    
    /**
        @brief  조건과 Obj 상태가 같은지 체크
    */
    public bool CheckCondition()
    {
        if (hourEnable != tfHour.activeSelf)
            return false;
        if (minuteEnable != tfMinute.activeSelf)
            return false;

        return true;
    }
}

/**
    @file       SunClockEvent
    @class      2018.12.27
    @author     황준성(hns17.tistory.com)
    @brief      조건을 검사하고 문을 연다.
*/
public class SunClockEvent : MonoBehaviour {
    //조건 리스트
    [SerializeField] public List<SunClockInfo> infos = null;

    //열려야 하는 문
    public DoorEvent door = null;

    //조건을 충족하면 표시
    public SunClockLightEvent centerLightEvent = null;


    private void Start()
    {
        StartCoroutine(ReleaseEvent());
    }

    /**
        @brief  Door Open Event
    */
    IEnumerator ReleaseEvent()
    {
        //Open Condition Check
        while (!CheckReleaseCondition())
            yield return null;

        //정답을 맞추면 불을 켜 표시하고 문의 잠금을 해제 한 후 메시지를 표시한다.
        centerLightEvent.TurnLight();
        door.isLock = false;

        NotifyMessageEvent.Instance.PrintMainText("문을 조사해 보자", 2f);
    }

    /**
        @brief  정답을 맞췄는지 검사한다.
    */
    bool CheckReleaseCondition()
    {
        for (int i = 0; i < infos.Count; i++)
        {
            if (!infos[i].CheckCondition())
                return false;
        }

        return true;
    }
}
