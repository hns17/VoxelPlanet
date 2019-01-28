/**
    @file   TalkStorage.cs
    @date   2019.01.05
    @author 황준성(hns17.tistory.com)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
    @class   TalkStorage
    @date   2019.01.05
    @author 황준성(hns17.tistory.com)
    @brief  대화 이벤트를 위한 클래스, 조건을 만족하면 Talk Bax 이벤트가 진행된다.
*/
public class TalkStorage : MonoBehaviour {
    [SerializeField] private string title;          //box Title
    [SerializeField] private List<string> textList; //box Text

    private IEnumerator talkEvent;

    private void Start()
    {
        talkEvent = TalkEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(talkEvent);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            StopCoroutine(talkEvent);
    }

    /**
        @brief  대화 이벤트를 진행한다.
    */
    private IEnumerator TalkEvent()
    {
        var info = PlayerInfo.Instance;

        while (true) {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E) && !info.IsLock) {
                //대화 이벤트 진행 중 Player 움직임은 Lock
                //Cursor On, Talk Event 실행 
                MyUtil.EnableCursor();
                PlayerAnimation.Instance.Idle();
                info.IsLock = true;
                TalkMessageEvent.Instance.PlayTalk(title, textList);
            }
        }
    }
}
