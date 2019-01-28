/**
    @file   PlayerSkill.cs
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @class  PlayerSkill
    @date   2018.12.30
    @author 황준성(hns17.tistory.com)
    @brief  PlayerSkill 제어
*/
public class PlayerSkill : MonoBehaviour {
    //스킬 사용 활성화 / 비활성화
    [SerializeField] private bool useSkill = false;

    //스킬에 사용되는 파티클
    [SerializeField] private GameObject hemiParticle;
    [SerializeField] private GameObject rayParticle;
    [SerializeField] private GameObject upParticle;
    
    private void Update () {
        if (PlayerInfo.Instance.IsLock || !useSkill)
            return;

        //Protect Wall
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            WeaponManager.Instance.SetProtectWeapon();
            PlayerAnimation.Instance.Block();
        }
        //Idle <-> Atk
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            WeaponManager.Instance.TurnAtkWeapon();
        }
        //Swap Atk weapon
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            WeaponManager.Instance.SwapAtkWeapon();
        }
        //Particle Chain
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponManager.Instance.SetMagicWeapon();
            PlayerAnimation.Instance.ParticleChain();
            
        }
        //Particle Blade
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponManager.Instance.SetBladeWeapon();
            PlayerAnimation.Instance.Slash();
        }
        //Escape
        else if (Input.GetMouseButtonDown(1))
        {
            PlayerAnimation.Instance.Roll();  
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position + transform.up * 0.7f, PlayerInfo.Instance.Model.forward);

            if (Physics.Raycast(ray, out hit, 0.8f))
            {
                //Debug.Log(hit.point);
                VoxelHitEvent.SetBlock(hit, new BlockAir());
            }
        }
    }

    public void ParticleRay()
    {
        var model = PlayerInfo.Instance.Model;
        rayParticle.transform.position = model.position +
            model.forward * 0.936f + model.right * 0.174f + model.up * 1.19f;
        rayParticle.SetActive(true);
        iTween.MoveBy(rayParticle, iTween.Hash("amount", model.forward * 10, "time", 0.4f,
                                                "easetype", iTween.EaseType.linear));
    }

    public void ParticleUp()
    {
        var model = PlayerInfo.Instance.Model;

        upParticle.transform.position = model.position + (-model.right + model.forward);
        upParticle.SetActive(true);
        var moveDir = upParticle.transform.position + model.right * 2;
        iTween.MoveTo(upParticle, iTween.Hash("position", moveDir, "time", 0.5f,
                                                "easetype", iTween.EaseType.linear));
    }

    public void ParticleChain()
    {
        hemiParticle.SetActive(true);
    }
}
