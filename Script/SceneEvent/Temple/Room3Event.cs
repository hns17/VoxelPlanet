/**
    @file   Room3Event.cs
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections.Generic;


/**
    @class  Room3Event
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
    @brief  Temple Room3에서 사용된 이벤트 클래스
    @detail 조건에 따라 등록된 몬스터가 플레이어를 쫓아간다.
*/
public class Room3Event : MonoBehaviour {

    //몬스터 리스트
    [SerializeField] private List<EnemyMovement> enemyList;

    //Condition
    public bool noEntry;    //입장금지
    public bool doNotRun;   //달리기 금지
    public bool doNotWalk;  //걷기 금지

    private void Start()
    {
        foreach(var enemy in enemyList)
            enemy.IsFollow = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {

            if (enemyList.Count == 0)
                return;

            var behaviour = PlayerInfo.Instance.State;
            bool condition = false;

            //입장 금지인데 들어왔네?
            if (noEntry)
                condition = true;
            //달리기 금지인데 달렸다
            else if (doNotRun && behaviour == PlayerInfo.PlayerState.RUN)
                condition = true;
            //걷기 금지인데 걸었다
            else if (doNotWalk && behaviour == PlayerInfo.PlayerState.WALK)
                condition = true;

            //조건 만족시 쫓아간다.
            if (condition) {
                foreach (var enemy in enemyList)
                    enemy.IsFollow = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Player가 범위를 벋어나면 쫓아가지 않는다.
        if (other.CompareTag("Player")) {
            foreach (var enemy in enemyList)
                enemy.IsFollow = false;
        }
    }
}
