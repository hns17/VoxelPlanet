/**
    @file   NPCAnimation.cs
    @date   2018.12.27
    @author 황준성(hns17.tistory.com)
    @brief  VoxelNpc Animation
*/
using System.Collections;
using UnityEngine;


/**
    @class  NPCAnimation
    @date   2018.12.27
    @author 황준성(hns17.tistory.com)
    @brief  Voxel Npc 애니메이션을 재생한다.
*/
public class NPCAnimation : MonoBehaviour {
    //NPC Animation 재생모드
    enum PlayMode { SINGLE, RANDOM }
    //Animation List
    enum NPCAniList { IDLE, CLAP, BOW, DANCE, FREAK, WALK, WAVING, SURPRISE }
    //Animation Name
    string[] npcAniName = new string[] { "Idle", "Clap", "Bow", "Dance", "Freak", "Walk", "Waving", "Surprise"};

    //재생 모드 지정, Single : 지정된 Animation 재생, Random : Random 하게 재생
    [SerializeField] private PlayMode playMode;
    
    //애니메이션 지정, 싱글 모드에서 만 사용
    [SerializeField] private NPCAniList playAni;

    //애니메이션 재생시간
    [SerializeField] private float playTime = 1f;

    //재생완료 후 RePlay까지 간격
    [SerializeField] private float waitTime = 1f;
    
    private Animator npcAnim;

    private void Awake()
    {
        npcAnim = GetComponent<Animator>();
    }

    void Start () {
        StartCoroutine(NpcAniPlay());
	}
	
    /**
        @brief  NPC Animation 재생 이벤트, 
                우선 코루틴으로 만들어 두고 후에 필요하면 Play, Stop함수 만들어 코루틴 제어하자.
    */
    IEnumerator NpcAniPlay()
    {
        int aniIdx = (int)playAni;

        while (true) {
            float loopTime = playTime;

            //랜덤 모드 일 경우 재생할 AniIdx 랜덤하게 변경
            if (playMode == PlayMode.RANDOM)
                aniIdx = Random.Range(0, npcAniName.Length - 1);

            //Play Time 체크
            npcAnim.SetBool("IsPlay", true);
            while ((loopTime -= Time.deltaTime) > 0) {

                 npcAnim.Play(npcAniName[aniIdx]);
                 yield return Yields.EndOfFrame;
            }
            npcAnim.SetBool("IsPlay", false);

            //플레이가 끝나면 대기시간 만큼 대기
            yield return Yields.WaitSeconds(waitTime);
        }
    }
    
}
