/**
    @file   ParticleManager.cs
    @date   2018.12.10
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @class  ParticleManager
    @date   2018.12.10
    @author 황준성(hns17.tistory.com)
    @brief  게임내 파티클을 관리하기 위해 만든 클래스인데, 
            개별로 관리하는게 편해서 현재 재생 관련 함수만 존재한다.
            추후에도 안쓰면 MyUtil로 빼는게 좋을것 같다.
*/

public class ParticleManager {
    
    /**
        @brief  particle 재생
        @param  ps : 재생할 particle, playSpd : 재생 속도
    */
    public static IEnumerator Play(ParticleSystem ps, float playSpd = 1f)
    {
       
        var tfParticle = ps.transform;

        ps.Play();

        //재생속도 지정
        var psMain = ps.main;
        psMain.simulationSpeed = playSpd;

        //하위에 위치한 파티클의 재생속도도 변경
        for(int i=0; i< tfParticle.childCount; i++) {
            psMain = tfParticle.GetChild(i).GetComponent<ParticleSystem>().main;
            psMain.simulationSpeed = playSpd;
        }

        //파티클이 종료되면 멈춘다.
        while (ps.isPlaying)
            yield return new WaitForEndOfFrame();
        
    }

    /**
        @brief  particle 역재생
        @param  ps : 역재생할 particle, playSpd : 역재생 속도, endTime : 종료 위치
    */
    public static IEnumerator Reverse(ParticleSystem ps, float playSpd = 1f, float endTime = 0f)
    {

        var psMain = ps.main;
        var duration = psMain.duration;

        while (duration > endTime) {
            duration -= Time.deltaTime * playSpd;

            ps.Simulate(duration, true, true);

            yield return new WaitForEndOfFrame();
        }
        ps.Simulate(0, true, true);

    }
}
