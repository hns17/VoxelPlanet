/**
    @file   HitDamage.cs
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  HitDamage
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
    @brief  트리거된 대상에게 충격을 준다.
    @detail Player의 무기나 트랩 등에 추가하여 사용
*/
public class HitDamage : MonoBehaviour {
    enum HitTarget { PLAYER, ENEMY}

    //충격을 가할 대상
    [SerializeField] private HitTarget hitTarget;

    //충격량
    [SerializeField] private int damage;

    private ObjectHealth playerHealth;
    private EnemyManager enemyMng;

    private void Start()
    {
        playerHealth = PlayerInfo.Instance.GetComponent<ObjectHealth>();
        enemyMng = EnemyManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        var randomDamage = Random.Range(0, 99);
        //몬스터에게 Damage
        if (hitTarget.Equals(HitTarget.ENEMY))
            enemyMng.EnemyDamage(other.gameObject, damage + randomDamage);
        //Player에게 Damage
        else
            playerHealth.TakeDamage(damage + randomDamage);
    }
    
}
