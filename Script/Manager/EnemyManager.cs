/**
    @file   EnemyManager.cs
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections.Generic;

/**
    @class  EnemyManager
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
    @brief  Enemy를 관리한다.
    @detail 현재 EnemyManager의 자식을 탐색해 관리하지만
            리스폰 등에 오브젝트풀을 이용하기 위해 변경이 필요
*/
public class EnemyManager : MonoSingleton<EnemyManager> {
    //Enemy 오브젝트 풀
    private Dictionary<GameObject, ObjectHealth> acts;
    private Dictionary<GameObject, ObjectHealth> idles;


    private void Awake()
    {
        acts = new Dictionary<GameObject, ObjectHealth>();
        idles = new Dictionary<GameObject, ObjectHealth>();

        for (int i=0; i < transform.childCount; i++) {
            var enemy = transform.GetChild(i);
            acts.Add(enemy.gameObject, enemy.GetComponent<ObjectHealth>());
        }
    }

    /**
        @brief  대상에게 Damage를 준다.
        @param  enemy : 대상, damage : 충격량
    */
    public void EnemyDamage(GameObject enemy, int damage)
    {
        if (!acts.ContainsKey(enemy))
            return;
        
        var target = acts[enemy];

        if(target != null)
            target.TakeDamage(damage);
    }

    /**
        @brief  Enemy제거
        @param  actEnemy : 제거 대상
    */
    public void RemoveEnemy(GameObject actEnemy)
    {
        idles.Add(actEnemy, acts[actEnemy]);
        acts.Remove(actEnemy);
    }

    /**
        @brief  Enemy 추가
    */
    public void AddEnemy()
    {
        var item = idles.GetEnumerator();
        item.MoveNext();

        acts.Add(item.Current.Key, item.Current.Value);

        idles.Remove(item.Current.Key);
    }
}
