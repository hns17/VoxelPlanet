/**
    @file   ObjectHealth.cs
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  ObjectHealth
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
    @brief  Object에 체력을 부여하고 히트 이벤트를 처리한다.
*/
public class ObjectHealth : MonoBehaviour
{
    //Object HP
    [SerializeField] private int startingHealth = 100;

    //Hit UI : Hp bar & Damage Text
    [SerializeField] private HitUIEvent hitUI;

    //공격 당했을때 효과음
    [SerializeField] private AudioClip hurtClip;


    private Animator    anim;           
    private AudioSource objAudio;   

    private bool        isDead;         //hp 0?            
    private int         currentHealth;  //현재 hp

    public bool IsDeath
    {
        get { return isDead;  }
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        objAudio = GetComponent<AudioSource>();

        hitUI = GetComponentInChildren<HitUIEvent>();
        anim = GetComponent<Animator>();

        if(objAudio != null)
            objAudio.clip = hurtClip;
    }
    

    /**
        @brief  공격 받을 때
        @param  damage : 충격량
    */
    public void TakeDamage(int damage) {
        if (isDead)
            return;

        if (objAudio != null)
            objAudio.Play();

        //Hp를 줄이도 damage 출력
        currentHealth -= damage;
        hitUI.PrintHitText(damage, (float)currentHealth / startingHealth);
        
        //hp가 0이 되면
        if (currentHealth <= 0) {
            isDead = true;
            Death();
        }
    }


    /**
        @brief  Object가 HP가 0이 되면
        @detail 현재 몬스터와 플레이어에 대해서만 처리 중이지만 
                다른 오브젝트나 이벤트 등을 위해 확장이 필요
    */
    void Death() {
        if (tag.Equals("Player"))
            PlayerAnimation.Instance.Death();
        else
            anim.Play("Dead");
    }
}