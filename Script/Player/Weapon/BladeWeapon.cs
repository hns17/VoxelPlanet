/**
    @file  BladeWeapon.cs
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;


/**
    @class  BladeWeapon
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  Particle Blade 장착 시 공격 이벤트
*/
public class BladeWeapon : MonoBehaviour {
    private Animator anim;      //player animator
    private string currentClip; //play clip
    [SerializeField] private GameObject hitParticle;

    private void Start()
    {
        anim = PlayerInfo.Instance.Anim;   
    }

    private void OnTriggerEnter(Collider other)
    {
        var info = PlayerInfo.Instance;
        //플레이어가 공격 중인 경우
        if (info.State == PlayerInfo.PlayerState.ATTACK)
        {
            //현재 재생 클립을 가져온다.
            var playClip = info.Anim.GetCurrentAnimatorClipInfo(1);
            if (playClip.Length <= 0)
                return;

            var playClipName= playClip[0].clip.name;
            currentClip = playClipName;

            //현재 클립에 맞게 적에게 데미지
            EnemyManager.Instance.EnemyDamage(other.gameObject, CalcBladeAtkDamage());
        }
    }
    

    /**
        @brief  공격에 따라 데미지 계산
    */
    private int CalcBladeAtkDamage()
    {
        int damage = Random.Range(0, 99);
        if (currentClip.Equals("LeftSlash"))
            damage += 100;
        else if (currentClip.Equals("Thrust"))
            damage += 200;
        else if (currentClip.Equals( "CircleSlash"))
            damage += 300;
        return damage;
    }

    private void Update () {
        if (Input.GetKey(KeyCode.Mouse0))
            anim.SetBool("IsBladeAttack", true);
        else
            anim.SetBool("IsBladeAttack", false);


        if (PlayerInfo.Instance.State == PlayerInfo.PlayerState.ATTACK)
            hitParticle.SetActive(true);
        else
            hitParticle.SetActive(false);

	}
}
