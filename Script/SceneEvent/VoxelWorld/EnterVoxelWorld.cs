/**
    @file   EnterVoxelWorld.cs
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
*/
using System.Collections;
using UnityEngine;


/**
    @class  EnterVoxelWorld
    @date   2019.01.11
    @author 황준성(hns17.tistory.com)
    @brief  VoxelWorld 진입 이벤트
*/
public class EnterVoxelWorld : MonoBehaviour {
    //이벤트에 사용될 파티클 정보
    public ParticleSystem psGate;       
    public ParticleSystem psCube;       
    public ParticleSystem psBalloon;

    //이벤트에 사용될 오브젝트
    public GameObject cube;
    public GameObject balloon;

    //이벤트에 사용될 카메라
    public GameObject eventCamOne;
    public GameObject eventCamTwo;

    //MiniMap Obj
    public GameObject miniMap;

    // Use this for initialization
    IEnumerator Start () {
        //FIrst Event : 첫번째 이벤트 카메라를 셋팅하고 게이트를 연다.
        //Event가 끝나면 Second Event 시작
        eventCamOne.SetActive(true);
        yield return Yields.WaitSeconds(2.0f);
        psGate.Play();
        
        iTween.ValueTo(this.gameObject, 
                        iTween.Hash("from", 0.1f, "to", 1f, "time", 1.5f, 
                                    "onupdatetarget", this.gameObject, 
                                    "onupdate", "UpdateGateRadius",
                                    "oncompletetarget", this.gameObject,
                                    "oncomplete", "SecondEvent",
                                    "easetype", iTween.EaseType.linear));

	}
	
    /**
        @brief Gate 애니메이션
        @param value : gate radius
    */
    void UpdateGateRadius(float value)
    {
        var gate = psGate.shape;
        gate.radius = value;
    }

    /**
        @brief  Gate가 열리면 Gate에서 플레이어가 떨어지며 Gate가 다시 닫히면 Third Event 실행 
    */
    void SecondEvent()
    {
        //플레이어 상태 셋팅
        var info = PlayerInfo.Instance;
        info.Anim.speed = 0;
        info.Model.gameObject.SetActive(true);
        info.Rigid.useGravity = true;
        info.transform.parent = null;

        iTween.ValueTo(this.gameObject,
                        iTween.Hash("from", 1f, "to", 0.5f, "time", 0.5f,
                                    "onupdatetarget", this.gameObject,
                                    "onupdate", "UpdateGateRadius",
                                    "oncompletetarget", this.gameObject,
                                    "oncomplete", "ThirdEvent",
                                    "easetype", iTween.EaseType.linear));
       
    }

    /**
        @brief  Gate를 닫고 낙하 중 열기구를 생성해 낙하 속도를 줄인다.
    */
    void ThirdEvent()
    {
        psGate.Stop();  //Gate Off
        psCube.Play();  //Cube Particle on
        
        //열기구 애니메이션
        StartCoroutine(PlayThirdEvent());
    }

    /**
        @brief  낙하 중인 플레이어에 열기구를 생성해 낙하 속도를 줄인다.
    */
    IEnumerator PlayThirdEvent()
    {
        //큐브가 플레이어의 낙하 방향으로 하강
        iTween.MoveTo(cube, iTween.Hash("position", balloon.transform.position, "time", 8));
        yield return Yields.WaitSeconds(1.5f);
        psCube.Stop();

        //큐브가 플레이어를 따라잡으면 2번 카메라 셋 후 열기구 애니메이션 실행
        eventCamTwo.SetActive(true);
        yield return ParticleManager.Play(psBalloon);
        balloon.SetActive(true);
    }

    /**
        @brief  열기구가 바닥에 착지하면 플레이어가 일어나며 플레이어 카메라를 셋팅. 
    */
    IEnumerator LandingEvent()
    {
        yield return Yields.WaitSeconds(1f);

        Destroy(eventCamOne);
        Destroy(eventCamTwo);

        PlayerInfo.Instance.Anim.speed = 1;
    }

    /**
        @brief  Player가 기구에서 내리면 큐브로 변경 후 이벤트 오브젝트 제거
    */
    IEnumerator EndEvent()
    {
        yield return Yields.WaitSeconds(1.0f);
        balloon.SetActive(false);
        yield return ParticleManager.Play(psBalloon);
        WeaponManager.Instance.SetCubeWeapon();
        miniMap.SetActive(true);

        Destroy(this.transform.parent.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        //착지 이벤트
        StartCoroutine(LandingEvent());
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //마지막 이벤트
        StartCoroutine(EndEvent());
    }
}
