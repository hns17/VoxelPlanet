/**
    @file   BgmEvent.cs
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
*/
using System;
using UnityEngine;

/**
    @struct BgmInfo
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
    @brief  BGM 정보
*/
[Serializable]
public struct BgmInfo
{
    public string       artist; //제작자
    public string       title;  //제목
    public AudioClip    clip;   //오디오 클립
    
    public BgmInfo(string artist, string title, AudioClip clip)
    {
        this.artist = artist;
        this.title = title;
        this.clip = clip;
    }
}

/**
    @class  BgmEvent
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
    @brief  Bgm Event 생성
*/
public class BgmEvent : MonoBehaviour {
    //playmode
    //start_play    : 시작과 동시에 재생
    //start_change  : 시작과 동시에 변경(재생 중인 곡 소리를 줄이고 교체)
    //Play          : 트리거 시 재생
    //Stop          : 트리거 시 종료
    //Volume_On     : 트리거 시 소리 on
    //Volume_Off    : 트리거 시 소리 off
    //Change        : 트리거 시 변경
    public enum PlayMode { START_PLAY, START_CHANGE, PLAY, STOP, VOLUME_ON, VOLUME_OFF, CHANGE}
    
    [SerializeField] private BgmInfo    bgmInfo;    //bgm 정보
    [SerializeField] private bool       isRemove;   //이벤트 발생 후 오브젝트 제거
    [SerializeField] private PlayMode   playMode;   //play mode


	// Use this for initialization
	private void Start () {
        if (playMode == PlayMode.START_PLAY) {
            BgmManager.Instance.BgmPlay(bgmInfo);
            if (isRemove)
                Destroy(gameObject);
        }
        else if (playMode == PlayMode.START_CHANGE) {
            BgmManager.Instance.BgmChange(bgmInfo);
            if (isRemove)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayModeHandler();
            if (isRemove)
                Destroy(gameObject);
        }
    }

    /**
        @brief  BGM 트리거 이벤트 
    */
    private void PlayModeHandler()
    {
        switch (playMode)
        {
            case PlayMode.PLAY:
                BgmManager.Instance.BgmPlay(bgmInfo);
                break;
            case PlayMode.STOP:
                BgmManager.Instance.BgmStop();
                break;
            case PlayMode.VOLUME_ON:
                BgmManager.Instance.BgmVolumeOn();
                break;
            case PlayMode.VOLUME_OFF:
                BgmManager.Instance.BGMVolumeOff();
                break;
            case PlayMode.CHANGE:
                BgmManager.Instance.BgmChange(bgmInfo);
                break;
            default:
                break;

        }
    }
}
