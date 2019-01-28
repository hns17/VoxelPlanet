/**
    @file   EfxPlayer.cs
    @date   2019.12.23
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  EfxPlayer
    @date   2019.12.23
    @author 황준성(hns17.tistory.com)
    @brief  Player 효과음
*/
public class EfxPlayer : MonoSingleton<EfxPlayer>
{
    public AudioClip walk;      //걷기
    public AudioClip landing;   //착지
    public AudioClip death;     //죽은 경우
    public AudioClip respawn;   //살아날 경우

    //Set Default Audio Source
    public AudioSource  efxSource;
    public float        lowPitchRange   = 0.95f;
    public float        highPitchRange  = 1.05f;

    /**
        @brief  걷기 사운드 재생
    */
    public void Walk()
    {
        PlaySingle(walk);
    }

    /**
        @brief  착지 사운드 재생
    */
    public void Landing()
    {
        PlaySingle(landing);
    }

    /**
        @brief  부활 사운드 재생
    */
    public void Respawn()
    {
        PlaySingle(respawn);
    }

    /**
        @brief  죽음 사운드 재생
    */
    public void Death()
    {
        PlaySingle(death);
    }


    /**
        @brief  player effect sound 재생
        @param  clip : 재생될 오디오
    */
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    /**
        @brief  player effect sound 정지
    */
    public void StopSound()
    {
        efxSource.Stop();
    }

    /**
        @brief  player effect sound 동시 재생
        @brief  clips : 오디오 클립 목록
    */
    public void RandomizeEfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;

        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
