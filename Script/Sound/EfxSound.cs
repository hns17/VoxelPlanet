/**
    @file   EfxSound.cs
    @date   2018.12.18
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
/**
    @class  EfxSound
    @date   2018.12.18
    @author 황준성(hns17.tistory.com)
    @brief  오디오 소스를 재생
            Animation Event에서 호출되는 함수로 사용
*/

[RequireComponent(typeof(AudioSource))]
public class EfxSound : MonoBehaviour {

    private AudioSource efxSource;
    
    void Start () {
        efxSource = GetComponent<AudioSource>();
    }
	
    /**
        @brief  AudioSource를 재생한다.
    */
    void PlayEfx()
    {
        if(efxSource != null)
            efxSource.Play();
    }
}
