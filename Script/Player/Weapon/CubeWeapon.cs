/**
    @file  CubeWeapon.cs
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
/**
    @class  CubeWeapon
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  Particle Cube 장착 시 공격 이벤트
*/
public class CubeWeapon : MonoBehaviour {
    private Animator anim;

    private void Start()
    {
        anim = PlayerInfo.Instance.Anim;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) 
            anim.SetBool("IsCubeAttack", true);
        else 
            anim.SetBool("IsCubeAttack", false);
    }
}
