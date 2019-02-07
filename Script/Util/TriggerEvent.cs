/**
    @file   TriggerEvent.cs
    @class  TriggerEvent
    @date   2018.12.19
    @author 황준성(hns17.tistory.com)
    @brief  트리거 이벤트 발생시 등록된 오브젝트 활성화, 비활성화, 삭제 수행
*/
using UnityEngine;
using System.Collections.Generic;

public class TriggerEvent : MonoBehaviour {
    //Condition
    enum Event { ENTER, STAY, EXIT}
    enum Behaviour { TARGET_ENABLE, TARGET_DISABLE, TARGET_REMOVE}
    enum FinishBehaviour { NONE, REMOVE, DISABLLE}

    [SerializeField] private Event triggerEvent;
    [SerializeField] private Behaviour behaviour;
    [SerializeField] private FinishBehaviour finshBehaviour;

    //작업이 수행될 오브젝트들
    [SerializeField] private List<GameObject> targets;


    private void OnTriggerEnter(Collider other)
    {
        if (triggerEvent != Event.ENTER)
            return;

        PlayBehaviour();
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerEvent != Event.STAY)
            return;

        PlayBehaviour();
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerEvent != Event.EXIT)
            return;

        PlayBehaviour();
    }


    /**
        @brief  지정된 작업을 수행한다.
    */
    private void PlayBehaviour()
    {
        if (behaviour== Behaviour.TARGET_ENABLE)
            SetTargetActiveState(true);
        else if (behaviour == Behaviour.TARGET_ENABLE)
            SetTargetActiveState(false);
        else
            RemoveTarget();
    }

    /**
        @brief  파라미터에 대응해 대상을 활성화, 비활성화 한다. 
        @param  state : 활성화 / 비활성화 플래그
    */
    private void SetTargetActiveState(bool state)
    {
        if (targets == null)
            return;

        foreach(var target in targets)
        {
            if (target != null)
                target.SetActive(state);
        }
    }
    

    /**
        @brief  대상을 삭제한다.
    */
    private void RemoveTarget()
    {
        if (targets == null)
            return;

        foreach (var target in targets)
        {
            if(target!=null)
                GameObject.Destroy(target);
        }
    }
}
