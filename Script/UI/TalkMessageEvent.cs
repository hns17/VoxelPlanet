/**
    @file   TalkMessageEvent.cs
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
    @class  TalkMessageEvent
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
    @brief  Game Talk Message Event를 실행한다.
*/
public class TalkMessageEvent : MonoSingleton<TalkMessageEvent> {
    public Text title;              //title Text
    public Text context;            //text

    public GameObject backGround;   //box
    
    private List<string> textList;  //textList
    private int txtIdx = -1;        //현재 보여지는 텍스트 위치


    /**
        @brief  Talk UI를 보여준다.
        @param  title : TitleText, context : text list    
    */
    public void PlayTalk(string title, List<string> context)
    {
        if (txtIdx != -1)
            return;

        if (context == null || context.Count <= 0)
            return;

        txtIdx = 0;
        this.title.text = title;
        this.context.text = context[txtIdx];

        textList = context;
        backGround.SetActive(true);
    }


    /**
        @brief  Click 하면 Next TextList를 보여주고 없으면 이벤트 해제
    */
    public void ClickEvent()
    {
        txtIdx++;

        if (txtIdx < textList.Count)
        {
            context.text = textList[txtIdx];
        }
        else
        {
            txtIdx = -1;
            MyUtil.DisableCursor();
            PlayerInfo.Instance.IsLock = false;
            backGround.SetActive(false);
        }
    }
}
