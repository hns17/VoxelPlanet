/**
    @file   NotifyMessageEvent.cs
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  게임 공지 문자 표시 이벤트(화면에 글자 띄움)
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
    @class   NotifyMessageEvent
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  게임 내 배경음 정보나 오브젝트 상호작용 메시지 기타 등등 표시
*/
public class NotifyMessageEvent : MonoSingleton<NotifyMessageEvent> {
    //화면 중앙에 뜨는 메인 메시지
    [SerializeField] private Text mainText;
    //화면 우 하단에 뜨는 서브 메시지
    [SerializeField] private Text subText;
	
    /**
        @brief  MainMessage를 화면에 표시
        @param  text : 표시할 문자, time : 표시할 시간(0이면 계속 표시)
    */
	public void PrintMainText(string text, float time = 0f)
    {
        mainText.text = text;
        mainText.gameObject.SetActive(true);

        if (time > 0)
            StartCoroutine(PlayMainTextEvent(time));
    }

    /**
        @brief  MainText 비활성화
    */
    public void MainTextDisable()
    {
        mainText.gameObject.SetActive(false);
    }

    /**
        @brief  MainText 주어진 시간 동안 표시 후 Disable
    */
    IEnumerator PlayMainTextEvent(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        MainTextDisable();
    }

    /**
        @date   2019.01.19
        @breif  SubText 표시
        @param  text : 표시할 문자
    */
    public void PrintSubText(string text)
    {
        subText.text = text;
        subText.gameObject.SetActive(true);
    }
}
