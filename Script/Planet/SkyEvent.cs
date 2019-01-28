/**
    @file   SkyEvent.cs
    @class  SkyEvent
    @date   2018.01.04
    @author 황준성(hns17.tistory.com)
    @brief  sun의 위치에 맞춰 하늘의 uv 정보를 변경
*/
using UnityEngine;

public class SkyEvent : MonoBehaviour {
    public Light sunLight;
    public Material skyDome;

    const float PIPI = Mathf.PI + Mathf.PI;
	
	// Update is called once per frame
	void Update () {
        
        if(skyDome != null) {
            var tfPlayer = PlayerInfo.Instance.transform;
            var sunDir = transform.position - tfPlayer.position;

            //player와 sun의 각
            var angle = Vector3.SignedAngle(tfPlayer.position,
               sunDir, tfPlayer.forward) + 90;

            //플레이어와 태양의 내적값을 이용해 태양의 세기를 구함
            var intencity = Vector3.Dot(tfPlayer.up, sunDir.normalized) *0.5f + 0.5f;
            sunLight.intensity = intencity * 1.25f;

            //angle 값을 기반으로 하늘의 uv 값을 계산
            float uvOffset = (angle * Mathf.Deg2Rad) / PIPI + 0.7f;

            if (uvOffset > 1)
                uvOffset -= 1;
            
            skyDome.SetTextureOffset("_MainTex", new Vector2(uvOffset, 0));
        }
    }
}
