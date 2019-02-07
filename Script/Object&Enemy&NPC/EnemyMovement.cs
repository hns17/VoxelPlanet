/**
    @file   EnemyMovement.cs
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/**
    @class  EnemyMovement
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
    @brief  몬스터의 이동 이벤트
*/
public class EnemyMovement : MonoBehaviour
{
    //몬스터의 이동 AI
    //ATTACK    : isAttack이 활성화되면 끝까지 쫓아간다.
    //RANGE     : 일정 거리내에 들어오면 쫓아간다.
    public enum EnemyBehaviour { FOLLOW, RANGE }

    [SerializeField] private EnemyBehaviour behaviour = EnemyBehaviour.FOLLOW;

    //쫓아 갈것 인가?
    [SerializeField] private bool isFollow;

    //탐지 범위
    [SerializeField] private float range = 8f;

    //이동 속도
    [SerializeField] private float moveSpeed = 3;

    private Animator anim;
    private Transform player;
    private NavMeshAgent nav;

    private Vector3 originPos;   //초기 위치 정보
    private Quaternion originRot;   //초기 회전 정보

    private ObjectHealth health;

    public bool IsFollow
    {
        set { isFollow = value; }
        get { return isFollow; }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        health = GetComponent<ObjectHealth>();
        player = PlayerInfo.Instance.transform;

        originPos = transform.localPosition;
        originRot = transform.localRotation;
    }

    private IEnumerator Start()
    {
        if (behaviour == EnemyBehaviour.FOLLOW)
            yield return ConditionFollow();
        else if (behaviour == EnemyBehaviour.RANGE)
            yield return ConditionRange();
    }

    /**
        @brief  isFollow 변수가 True면 몬스터가 죽을때까지 쫓아간다.
    */
    private IEnumerator ConditionFollow()
    {

        while (true)
        {
            yield return null;

            if (health != null)
            {
                if (health.IsDeath)
                    break;
            }

            if (isFollow)
            {
                var distPlayer = Vector3.Distance(transform.position, player.transform.position);
                MoveEnemy(player.position);

                //근처에 있으면 빠르게 쫓아간다.
                if (distPlayer < range)
                    nav.speed = moveSpeed * 2;
            }
            else
            {
                MoveEnemy(originPos, true);
            }
        }
    }

    /**
        @date   2019.01.10
        @brief  범위내에 있는 경우 쫓아간다.
    */
    private IEnumerator ConditionRange()
    {
        while (true)
        {
            yield return null;

            if (health != null)
            {
                if (health.IsDeath)
                    break;
            }

            var distPlayer = Vector3.Distance(transform.position, player.position);

            if (distPlayer <= range && PlayerInfo.Instance.State != PlayerInfo.PlayerState.DEATH)
                MoveEnemy(player.position);
            else
                MoveEnemy(originPos, true);
        }
    }

    /**
        @brief  목표를 향해 이동한다.
        @param  target : 목표지점, isReturn : 원래 위치로 돌아가는 중인가?
    */
    private void MoveEnemy(Vector3 target, bool isReturn = false)
    {
        //목표 지점으로 이동
        nav.speed = moveSpeed;
        transform.LookAt(target);
        nav.SetDestination(target);

        //도착하지 못했으면 
        if (nav.remainingDistance > nav.stoppingDistance)
        {
            anim.SetFloat("Distance", nav.remainingDistance);
        }
        //도착 했으면 
        else
        {
            //원래 위치로 돌아온 경우 처음에 바라보던 방향으로 회전
            if (isReturn)
                transform.localRotation = originRot;

            anim.SetFloat("Distance", 0f);
        }

    }
}