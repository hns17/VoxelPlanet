/**
    @file   UIYModifier.cs
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  UIYModifier
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
    @brief  TextUI의 Vertex를 변경한다.(Y값 수정)
*/
public class UIYModifier : UIVertexModifier {
    //변경에 사용될 offset
    [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1));

    /**
        @brief  TextAniamation 진행에 맞춰 TextUIVertex (Y)위치 값 변경
        @param  txtData : text animation info, uiVertex : text UI Vertex
    */
    public override void Apply(TextData txtData, ref UIVertex uiVertex)
    {
        uiVertex.position.y *= curve.Evaluate(txtData.Progress);
    }

}
