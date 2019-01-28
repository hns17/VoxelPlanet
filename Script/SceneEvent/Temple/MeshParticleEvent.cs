/**
    @file   MeshParticleEvent.cs
    @date   2018.12.29
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
    @class  MeshParticleEvent
    @date   2018.12.29
    @author 황준성(hns17.tistory.com)
    @brief  Temple Room7의 텍스트 메쉬 파티클에 사용
            추후 큐브의 대화 등에 쓸수도...
*/
public class MeshParticleEvent : MonoBehaviour {
    //텍스트 오브젝트 메쉬 리스트
    [SerializeField] List<Mesh> meshs;

    //파티클 시스템
    private ParticleSystem ps;
    
	void Start () {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(PlayTextParticle());
	}
	
    /**
        @brief  list에 있는 텍스트 mesh를 랜덤하게 대상으로 선택해 파티클 형성 
    */
	IEnumerator PlayTextParticle()
    {
        while (meshs.Count > 0) {
            var idx = Random.Range(0, meshs.Count - 1);
            var shape = ps.shape;
            shape.mesh = meshs[idx];

            yield return ParticleManager.Play(ps);
            yield return new WaitForSeconds(3f);
        }
    }
}
