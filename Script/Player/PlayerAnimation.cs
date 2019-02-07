/**
    @file   PlayerAnimation.cs
    @class  PlayerAnimation
    @date   2018.08.04
    @author 황준성(hns17.tistory.com)
    @brief  플레이어의 기본 애니메이션 이벤트를 관리
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimation : MonoSingleton<PlayerAnimation> {
    //플레이어 시네 카메라
    [SerializeField] private Cinemachine.CinemachineFreeLook freeLook;

    //플레이어 라이팅
    [SerializeField] private List<Light> lgtList;

    //Player 죽을 때 사용되는 파티클
    [SerializeField] private ParticleSystem psDeathPolygon;
    [SerializeField] private ParticleSystem psDeathVoxel;
    

    private Animator anim = null;

    private void Awake()
    {
        anim = PlayerInfo.Instance.Anim;
    }
    
    /**
        @brief  슬래쉬 애니메이션
    */
    public void Slash()
    {
        StartCoroutine(WaitForAnimEvent("CircleSlash", 0.9f));
    }
    
    /**
        @brief  회피 애니메이션
    */
    public void Roll()
    {
        Idle();
        StartCoroutine(WaitForAnimEvent("Roll", 0.9f));
    }
    
    /**
        @brief  파티클체인 애니메이션
    */
    public void ParticleChain()
    {
        StartCoroutine(WaitForAnimEvent("ParticleChain", 0.9f));
    }
    
    /**
        @brief  가드 애니메이션
    */
    public void Block()
    {
        PlayerInfo.Instance.IsLock = true;
        anim.SetBool("IsBlock", true);
    }
    
    /**
        @brief  걷기 애니메이션
    */
    public void Walk()
    {
        anim.SetBool("IsRun", false);
        anim.SetBool("IsWalk", true);
    }
    
    /**
        @brief  달리기 애니메이션
    */
    public void Run()
    {
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsRun", true);
    }
    
    /**
        @brief  점프 애니메이션
    */
    public void Jump()
    {
        anim.SetBool("IsJump", true);
    }
    
    /**
        @brief  대기 애니메이션
    */
    public void Idle()
    {
        PlayerInfo.Instance.IsLock = false;
        anim.SetBool("IsBlock", false);
        anim.SetBool("IsRun", false);
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsBladeAttack", false);
        anim.SetBool("IsCubeAttack", false);
    }
    
    /**
        @brief  플레이어 기상 애니메이션
    */
    public void WakeUp()
    {
        Idle();
        StartCoroutine(WaitForAnimEvent("WakeUp"));
    }

    /**
        @brief  플레이어 Death 애니메이션
    */
    public void Death()
    {
        if (PlayerInfo.instance.State == PlayerInfo.PlayerState.DEATH)
            return;
        
        StartCoroutine(PlayerDeath());
    }

    /**
        @brief  플레이어 Death 애니메이션 이벤트
    */
    private IEnumerator PlayerDeath()
    {
        //플레이어 정보 죽음 상태로
        var info = PlayerInfo.Instance;
        
        info.IsLock = true;
        info.Rigid.useGravity = false;
        info.State = PlayerInfo.PlayerState.DEATH;

        //애니메이션 정지
        anim.speed = 0;

        //애니메이션 재생을 오브젝트가 off되도 항상 갱신하도록 변경
        var originAnimCull = anim.cullingMode;
        anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;

        //바인드 오브젝트 해제
        transform.parent = null;

        //렌더 모델 off
        var model = PlayerInfo.Instance.ModelMesh;
        model.gameObject.SetActive(false);

        //death particle 선택
        ParticleSystem psDeath = psDeathPolygon;
        if (info.Type == PlayerInfo.ModelType.VOXEL)
            psDeath = psDeathVoxel;

        //death sound 재생
        EfxPlayer.Instance.Death();
        
        //play death particle
        yield return ParticleManager.Play(psDeath);

        //update respawn
        transform.position = info.RespawnPosition;

        //set wakeup animation
        anim.Play("WakeUp");

        //play respawn sound
        EfxPlayer.Instance.Respawn();

        //reverce death particle
        yield return StartCoroutine(ParticleManager.Reverse(psDeath));

        //set player state
        info.Rigid.useGravity = true;
        anim.cullingMode = originAnimCull;
        model.gameObject.SetActive(true);

        //play wakeup
        WakeUp();
    }


    /**
        @brief  Player Model Poly <-> Voxel 변환시 재구성 애니메이션 
    */
    public void Reconstruction()
    {
        StartCoroutine(PlayerReconstruction());
    }

    /**
        @brief  player 재구성 애니메이션 이벤트 
    */
    private IEnumerator PlayerReconstruction()
    {
        //Player off 상태에서도 애니메이션 재생
        var originAnimCull = anim.cullingMode;
        anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        
        //Model off
        PlayerInfo.Instance.ModelMesh.SetActive(false);

        //Set Event Particle
        var firstParticle = psDeathPolygon;
        var secondParticle = psDeathVoxel;

        if (PlayerInfo.Instance.Type == PlayerInfo.ModelType.VOXEL) {
            firstParticle = psDeathVoxel;
            secondParticle = psDeathPolygon;
        }
        
        //Play Particle
        yield return StartCoroutine(ParticleManager.Play(firstParticle, 1f));

        //Change Model(Poly <-> Voxel)
        SwapModel();

        //Reverce Particle
        yield return StartCoroutine(ParticleManager.Reverse(secondParticle, 1f));

        //Active on Player
        PlayerInfo.Instance.ModelMesh.SetActive(true);

        //return anim mode
        anim.cullingMode = originAnimCull;
    }

    /**
        @brief  player 모델을 전환한다. 
    */
    public void SwapModel()
    {
        if (PlayerInfo.Instance.Type == PlayerInfo.ModelType.POLYGON)
            PlayerInfo.Instance. SetModel(PlayerInfo.ModelType.VOXEL, false);
        else
            PlayerInfo.Instance.SetModel(PlayerInfo.ModelType.POLYGON, false);
    }

    /**
        @brief  player Scale Animation
        @param  sizeScale : scale 될 크기, spdScale : scale될 속도, duration : scale time
    */
    public void Scale(float sizeScale, float spdScale, float duration)
    {
        StartCoroutine(PlayerScale(sizeScale, spdScale, duration));
    }

    /**
        @brief  player Scale Animation Event
        @param  sizeScale : scale 될 크기, spdScale : scale될 속도, duration : scale time
    */
    private IEnumerator PlayerScale(float scaleSize, float scaleSpd, float duration)
    {
        var model = PlayerInfo.Instance.Model;

        //Scale 이전 정보
        var originScale = model.transform.localScale;
        var originSpeed = PlayerInfo.Instance.BaseSpeed;
        var originJumpH = PlayerInfo.Instance.JumpHeight;
        var originOrbit = (Cinemachine.CinemachineFreeLook.Orbit[])freeLook.m_Orbits.Clone();
        
        float stepValue = 0;
        while (stepValue < 1f) {
            stepValue += Time.deltaTime * duration;

            if (stepValue > 1f)
                stepValue = 1;

            float sizeFactor = Mathf.SmoothStep(1, scaleSize, stepValue);
            float spdFactor = Mathf.SmoothStep(1, scaleSpd, stepValue);

            
            model.localScale = originScale * sizeFactor;
            PlayerInfo.Instance.BaseSpeed = originSpeed * spdFactor;
            PlayerInfo.Instance.JumpHeight = originJumpH * spdFactor;

            //Scale 에 맞게 카메라 조정
            for (int j = 0; j < freeLook.m_Orbits.Length; j++) {
                freeLook.m_Orbits[j].m_Height = originOrbit[j].m_Height * sizeFactor;
                freeLook.m_Orbits[j].m_Radius = originOrbit[j].m_Radius * sizeFactor;
            }


            yield return null;
        }

        //Scale 맞게 Light range 조정
        if (lgtList != null) {
            foreach (var lgt in lgtList)
                lgt.range *= scaleSize;
        }

    }

    /**
        @brief  애니메이션 재생이 종료될 때 까지 플레이어 조작을 막는다.
        @param  name : 재생할 애니메이션 이름, endTime : 종료시간
    */
    IEnumerator WaitForAnimEvent(string name, float endTime = 1f)
    {
        PlayerInfo.Instance.IsLock = true;

        anim.speed = 1;
        anim.Play(name);

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(name))
            yield return Yields.EndOfFrame;
        
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < endTime)
            yield return Yields.EndOfFrame;

        PlayerInfo.Instance.IsLock = false;
    }


}