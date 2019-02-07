/**
    @file   Room7Event.cs
    @date   2018.12.29
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
    @class  Room7Event
    @date   2018.12.29
    @author 황준성(hns17.tistory.com)
    @brief  Room7 Event를 진행
*/
public class Room7Event : MonoBehaviour {
    //이벤트에 사용될 오브젝트 및 파티클
    [SerializeField] private List<GameObject> disableObjects;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private List<Rigidbody> floorObject;
    [SerializeField] private SphereCollider trigger;

    public ParticleSystem cubeParticle;
    public GameObject cubeObj;
    public GameObject gateObj;

    //이벤트용 카메라
    public GameObject eventCamFirst;
    public GameObject eventCamSecond;
    public GameObject eventCamThird;

    
    //바닥의 흔들림에 사용되는 값
    public float scale = 7f;
    public float scaleModifier = 5f;
    public float offsetHeight = 7f;

    private void OnTriggerEnter(Collider other)
    {
        NotifyMessageEvent.Instance.PrintMainText("제거한다.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        NotifyMessageEvent.Instance.MainTextDisable();

        //Start Room7 Event
        StartCoroutine(PlayRoom7Event());
    }

    /**
        @brief  Start Room7 Event 
        @detail First Event     : 주변 오브젝트를 붕괴시키고 플레이어를 복셀화
                Second Event    : 바닥 흔들림
                Third Event     : 바닥 붕괴 및 플레이어 낙하에 따른 카메라 이벤트
    */
    IEnumerator PlayRoom7Event()
    {
        //=================== Start First Event =======================//
        //Play Release Particle and Disable Object
        ps.Play();
        foreach (var obj in disableObjects)
            obj.SetActive(false);

        //disable Trigger
        trigger.enabled = false;

        //Rebuild Player
        PlayerAnimation.Instance.Reconstruction();

        yield return Yields.WaitSeconds(4f);
        //=================== End Firset Event =======================//

        //=================== Start Second Event =======================//
        //Shake Floor
        float t = 0f;
        while (t < 6f) {
            t += Time.deltaTime;
            foreach (var rigid in floorObject) {
               var height = scaleModifier * Mathf.PerlinNoise(Time.time + rigid.position.x * scale,
                                            Time.time + rigid.position.z * scale);
                Vector3 pos = rigid.transform.position;
                pos.y = height + offsetHeight;
                rigid.transform.position = pos;
            }

            yield return new WaitForEndOfFrame();
        }
        //=================== End SecondEvent =======================//

        //=================== Start ThirtEvent =======================//
        //Destory Floor
        foreach (var rigid in floorObject) {
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }
        PlayerInfo.Instance.Anim.Play("Fall_Second");

        eventCamFirst.SetActive(true);
        yield return Yields.WaitSeconds(2.5f);

        PlayerInfo.Instance.Rigid.isKinematic = true;
        PlayerInfo.Instance.IsLock = true;

        yield return Yields.WaitSeconds(1.0f);
        eventCamSecond.SetActive(true);
        yield return Yields.WaitSeconds(1.0f);
        yield return ParticleManager.Reverse(cubeParticle, 1f);
        cubeObj.SetActive(true);
        yield return Yields.WaitSeconds(0.5f);
        
        cubeObj.SetActive(false);
        yield return ParticleManager.Play(cubeParticle);

        eventCamThird.SetActive(true);
        gateObj.SetActive(true);
        
        iTween.MoveAdd(gateObj, iTween.Hash("z", 1, "time", 3, "easetype", iTween.EaseType.linear));
        yield return Yields.WaitSeconds(2.5f);
        //=================== End ThirtEvent =======================//

        SceneControlManager.Instance.NextScene();
    }

   
}
