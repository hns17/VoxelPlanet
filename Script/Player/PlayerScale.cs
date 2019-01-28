/**
    @file   PlayerScale.cs
    @date   2019.12.19
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  PlayerScale
    @date   2019.12.19
    @author 황준성(hns17.tistory.com)
    @brief  플레이어의 크기와 속도를 변경한다.
*/
public class PlayerScale : MonoBehaviour {
    //변경할 크기
    public float sizeFactor = 1f;
    //변경할 속도
    public float spdFactor = 1f;
    //크기 변경 속도
    public float duration = 1f;

    //원래 Trigger 크기
    private Vector3 originTriggerScale;

    private void OnTriggerEnter(Collider other)
    {
        //크기를 변경한다.
        originTriggerScale = transform.localScale;
        transform.localScale *= 1.2f;
        PlayerAnimation.Instance.Scale(1 / sizeFactor, 1 / spdFactor, duration);
    }

    private void OnTriggerExit(Collider other)
    {
        //원래대로 돌아온다.
        transform.localScale = originTriggerScale;
        PlayerAnimation.Instance.Scale(sizeFactor, spdFactor, duration);
    }

}
