/**
    @file   ProtectWeapon.cs
    @date   2018.12.28
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  ProtectWeapon
    @date   2018.12.28
    @author 황준성(hns17.tistory.com)
    @brief  보호막 활성화
*/
public class ProtectWeapon : MonoBehaviour {

    private void Update () {
        //Disable Protect
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            PlayerAnimation.Instance.Idle();
            WeaponManager.Instance.SetPrevWeapon();
        }
    }
}
