/**
    @file       InvisibleEvent.cs
    @date       2018.12.16
    @author     황준성(hns17.tistory.com)
    @brief      Temple Scene Room3, Room5에 사용된 컴포넌트
                지정된 조건에 따라 지정된 오브젝트의 렌더와 충돌을 조정한다.
*/
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
    @class      InvisibleCondition
    @date       2018.12.16
    @author     황준성(hns17.tistory.com
    @brief      지정된 Obj의 Condition 정보를 기록하고 그에 맞게 업데이트한다.
*/
[Serializable]
public class InvisibleCondition
{
    public bool isDraw;         //Object Render On/Off
    public bool isCollision;    //Object Collision On/Off
    public bool isActive;

    /**
        @brief  지정된 조건에 맞춰 Object collider와 render 변경
        @param  colls : 충돌체 리스트, renders : renderer List
    */
    public void UpdateObject(List<Collider> colls, List<MeshRenderer> renders, List<GameObject> objs)
    {
        for(int i=0; i<colls.Count; i++)
            colls[i].enabled = isCollision;
        
        for (int i = 0; i < renders.Count; i++)
            renders[i].enabled = isDraw;

        for (int i = 0; i < objs.Count; i++)
            objs[i].SetActive(isActive);
    }
}


/**
    @class      InvisibleEvent
    @date       2018.12.16
    @author     황준성(hns17.tistory.com)
    @brief      Objects 의 render와 colls 상태를 조건에 따라 변경한다.
*/
public class InvisibleEvent : MonoBehaviour {
    //현재 on/off 산태
    public bool turnState = false;

    //시작할때 TurnState 상태에 맞춰 등록된 오브젝트 업데이트 할 것인가?
    public bool isStartUpdate = false;

    //on/off 대상이 되는 오브젝트
    public GameObject turnObject = null;
    
    
    [Header("[Trun on Condition]")]     //on condition
    [SerializeField] InvisibleCondition turnOnCondition;

    [Header("[Trun off Condition]")]    //off condition
    [SerializeField] InvisibleCondition turnOffCondition;


    //target colldier & render list
    [Header("[Target List]")]
    public List<Collider> colls = null;
    public List<MeshRenderer> renders = null;
    public List<GameObject> objs = null;

    //event courtine 
    private IEnumerator updateEvent;

    private void Start()
    {
        updateEvent = UpdateEvent();

        if (isStartUpdate)
            UpdateCondition();
    }

    private void OnTriggerEnter(Collider other)
    {
        //이벤트 오브젝트에 플레이어가 진입하면 이벤트 코루틴 활성화
        if (other.CompareTag("Player")) {
            NotifyText();
            StartCoroutine(updateEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //이벤트 오브젝트에서 플레이어가 멀어지면 이벤트 코루틴 비활성화
        if (other.CompareTag("Player")) {
            NotifyMessageEvent.Instance.MainTextDisable();
            StopCoroutine(updateEvent);
        }
    }
    

    /**
        @brief 사용자 입력이 들어오면 State 변경, 변경된 State에 맞게 오브젝트 변경
    */
    IEnumerator UpdateEvent()
    {
        while (true) {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E)) {
                //on / off 변경 후 지정된 오브젝트 상태 변경
                turnState = !turnState;
                UpdateCondition();
                NotifyText();
            }
        }
    }

    /**
        @brief  Object Condition Update 
    */
    void UpdateCondition()
    {
        if (turnState)
            turnOnCondition.UpdateObject(colls, renders, objs);
        else
            turnOffCondition.UpdateObject(colls, renders, objs);

        if(turnObject != null)
            turnObject.SetActive(turnState);
    }

    /**
        @brief  현재 조명 상태를 메시지로 보여준다. 
    */
    private void NotifyText()
    {
        if(turnState)
            NotifyMessageEvent.Instance.PrintMainText("조명을 끈다.");
        else
            NotifyMessageEvent.Instance.PrintMainText("조명을 켠다.");
    }
}
