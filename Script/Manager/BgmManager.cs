/**
    @file   BgmManager.cs
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @class  BgmManager
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
    @brief  Bgm Event
*/
public class BgmManager : MonoSingleton<BgmManager> {
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /**
        @brief  BGM 정보 출력
        @param  info : 출력할 배경음 정보
    */
    private IEnumerator PrintBgmInfo(BgmInfo info) {
        yield return Yields.WaitSeconds(3.0f);

        string txt = "BGM INFO\n" +
                     "Artist : " + info.artist + "\n" +
                     "Title : " + info.title;
        NotifyMessageEvent.Instance.PrintSubText(txt);

    }

    /**
        @brief  BGM 재생
        @param  info : 재생할 배경음 정보
    */
    public void BgmPlay(BgmInfo info)
    {
        BgmStop();
        
        audioSource.clip = info.clip;
        audioSource.Play();

        StartCoroutine(PrintBgmInfo(info));
    }

    /**
        @brief  현재 BGM 정지
    */
    public void BgmStop()
    {
        audioSource.Stop();
    }

    /**
        @brief  음소거
    */
    public void BGMVolumeOff()
    {
        audioSource.mute = true;
    }

    /**
        @brief  음소거 해제
    */
    public void BgmVolumeOn()
    {
        audioSource.mute = false;
    }

    /**
        @brief  BGM 변경
        @param  info : 변경할 배경음 정보
    */
    public void BgmChange(BgmInfo info)
    {
        StartCoroutine(BgmChangeEvent(info));
    }

    /**
        @brief  BGM을 Smooth하게 변경한다
        @param  info : 변경할 배경음 정보
    */
    private IEnumerator BgmChangeEvent(BgmInfo info)
    {
        //현재 배경음 소리를 줄인다.
        if (audioSource.isPlaying) {
            while (audioSource.volume > 0) {
                audioSource.volume -= 0.01f;
                yield return null;
            }
        }

        //배경음을 재생한다.
        BgmPlay(info);

        //배경음 소리를 올린다.
        while (true) {
            audioSource.volume += 0.01f;

            if(audioSource.volume >= 1) {
                audioSource.volume = 1;
                break;
            }

            yield return null;
        }
    }
}
