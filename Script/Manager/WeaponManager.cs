/**
    @file  WeaponManager.cs
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @struct WeaponInfo
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  무기 정보
*/
[System.Serializable]
struct WeaponInfo
{
    public ParticleSystem ps;   //weapon particle
    public GameObject obj;      //weapon object
    public Transform parent;    //equip target
}

/**
    @class  WeaponManager
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  무기 장착 이벤트
*/
public class WeaponManager : MonoSingleton<WeaponManager> {
    [SerializeField] private WeaponInfo cube;           //큐브
    [SerializeField] private WeaponInfo protectWall;    //보호막

    [SerializeField] private WeaponInfo bladeWeapon;    //소드
    [SerializeField] private WeaponInfo magicWeapon;    //마법무기


    //이전에 장착한 무기
    private WeaponInfo prevWeapon;
    //현재 장착중인 무기
    private WeaponInfo currentWeapon;
    //최근에 장착한 공격 무기
    private WeaponInfo currentAtkWeapon;


    private void Start()
    {
        currentWeapon = cube;
        currentAtkWeapon = bladeWeapon;
        
        //무기를 캐릭터에 연결
        bladeWeapon.obj.transform.parent = bladeWeapon.parent;
        protectWall.obj.transform.parent = protectWall.parent;
        
    }

    /**
        @brief  전투모드 / 일반 모드 전환
    */
    public void TurnAtkWeapon()
    {
        if(currentWeapon.Equals(cube))
            StartCoroutine(EnableWeapon(currentAtkWeapon));
        else
            StartCoroutine(EnableWeapon(cube));

    }

    /**
        @brief  무기 전환
    */
    public void SwapAtkWeapon()
    {
        if (currentAtkWeapon.Equals(bladeWeapon))
            SetMagicWeapon();
        else
            SetBladeWeapon();
    }

    
    /**
        @brief  큐브 장착
    */
    public void SetCubeWeapon()
    {
        StartCoroutine(EnableWeapon(cube));
    }

    /**
        @brief  마법무기 장착
    */
    public void SetMagicWeapon()
    {
        currentAtkWeapon = magicWeapon;
        StartCoroutine(EnableWeapon(currentAtkWeapon));
    }

    
    /**
        @brief  소드 장착
    */
    public void SetBladeWeapon()
    {
        currentAtkWeapon = bladeWeapon;
        StartCoroutine(EnableWeapon(currentAtkWeapon, 0.5f));
    }
    
    /**
        @brief  보호막 장착
    */
    public void SetProtectWeapon()
    {
        StartCoroutine(EnableWeapon(protectWall));
    }
    
    /**
        @brief  이전 무기 장착
    */
    public void SetPrevWeapon()
    {
        StartCoroutine(EnableWeapon(prevWeapon));
    }

    /**
        @brief  무기 해제
    */
    public void ReleaseWeapon()
    {
        StartCoroutine(DisableWeapon());
    }

    /**
        @brief  현재 장착 중인 무기 해제 후 지정된 무기 장착
        @param  weapon : 장착할 무기, playSpd : 무기 변환 파티클 재생 속도
    */
    IEnumerator EnableWeapon(WeaponInfo weapon, float playSpd = 1f)
    {
        //해제
        yield return DisableWeapon();
        currentWeapon = weapon;

        //변환 파티클 재생 후 무기 활성화
        if(currentWeapon.ps != null)
            yield return ParticleManager.Reverse(currentWeapon.ps, playSpd);
        currentWeapon.obj.SetActive(true);
    }

    /**
        @brief  장착 중인 무기 해제
    */
    IEnumerator DisableWeapon()
    {
        prevWeapon = currentWeapon;
        currentWeapon.obj.SetActive(false);

        //해제 파티클 재생
        if (currentWeapon.ps != null)
            yield return ParticleManager.Play(currentWeapon.ps, 2);

        currentWeapon.obj = null;
        currentWeapon.ps = null;
    }
}
