/**
    @file   EnemyAttack.cs
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;


/**
    @class  EnemyAttack
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
    @brief  몬스터의 공격 이벤트
*/
public class EnemyAttack : MonoBehaviour
{
    //일격필살
    [SerializeField] private bool deathAttack;
    
    //공격 범위
    [SerializeField] private float attackRange = 1f;
    //공격 속도
    [SerializeField] private float timeBetweenAttacks = 0.5f;    
    
    
    private Animator    anim;       //몬스터 애니메이터                     
    private PlayerInfo  player;     //플레이어 정보
    private ObjectHealth health;

    private void Awake()
    {
        player = PlayerInfo.Instance;
        anim = GetComponent<Animator>();
        health = GetComponent<ObjectHealth>();
    }

    private IEnumerator Start()
    {
        //몬스터가 죽으면 루틴 해제
        while (!health.IsDeath) {
            if (!player.State.Equals(PlayerInfo.PlayerState.DEATH))
                Attack();
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    /**
        @brief  몬스터의 공격함수.
        @detail 몬스터가 플레이어를 공격하는 함수인데 
                현재 공격범위내에 들어가면 하나의 공격만 하는 단순한 패턴이다.
                추후 클래스 확장하여 몬스터 AI 로직 작성이 필요
    */
    private void Attack()
    {
        var distPlayer = Vector3.Distance(player.transform.position, transform.position);

        //공격범위 체크
        if (distPlayer <= attackRange) {
            transform.LookAt(player.transform);
            anim.Play("Attack1");

            //몬스터가 원펀맨이면 플레이어 죽음
            if (deathAttack)
                PlayerAnimation.Instance.Death();
        }
    }
}