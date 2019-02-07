/**
    @file   SimpleDungeonExit.cs
    @date   2019.01.12
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @class  SimpleDungeonExit
    @date   2019.01.12
    @author 황준성(hns17.tistory.com)
    @brief  SimpleDungeon 씬에서 사용된 이벤트 
*/
public class SimpleDungeonExit : MonoBehaviour {
    //나가기 이벤트 
    private IEnumerator exitEvent;

    private void Start()
    {
        exitEvent = ExitEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 이벤트 오브젝트에 근접하면 공지 띄우고 이벤트 실행
        if (other.CompareTag("Player")) {
            NotifyMessageEvent.Instance.PrintMainText("EXIT");
            StartCoroutine(exitEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //플레이어가 이벤트 오브젝트에서 멀어지면 공지 지우고 이벤트 정지
        if (other.CompareTag("Player"))
            NotifyMessageEvent.Instance.MainTextDisable();
            StopCoroutine(exitEvent);
    }

    /**
        @brief  던전 종료 이벤트, 메시지 띄우고 타이틀 씬으로 
    */
    private IEnumerator ExitEvent()
    {
        while (!Input.GetKeyDown(KeyCode.E))
            yield return null;

        NotifyMessageEvent.Instance.PrintMainText("포트폴리오는 여기까지 입니다.", 2);
        yield return Yields.WaitSeconds(2f);
        NotifyMessageEvent.Instance.PrintMainText("감사합니다.", 2);
        yield return Yields.WaitSeconds(3f);
        SceneControlManager.Instance.LoadScene("Title");
    }
}
