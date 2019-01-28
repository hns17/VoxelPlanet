/**
    @file   UICyilnderModifier.cs
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;


/**
    @class  UICyilnderModifier
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
    @brief  TextUI의 Vertex를 캡슐 형태로 변환한다.
*/
public class UICylinderModifier : UIVertexModifier {

    [SerializeField]
    private float radius = 10;

    /**
        @brief  TextAniamation 진행에 맞춰 TextUIVertex 캡슐 형태로 변경
        @param  txtData : text animation info, uiVertex : text UI Vertex
    */
    public override void Apply(TextData txtData, ref UIVertex uiVertex)
    {
        float x = uiVertex.position.x;
        float finalRadius = Mathf.Clamp(radius * txtData.Progress, 1, radius);
        
        uiVertex.position.z = -finalRadius * Mathf.Cos(x / finalRadius);
        uiVertex.position.x = finalRadius * Mathf.Sin(x / finalRadius);
    }
}
