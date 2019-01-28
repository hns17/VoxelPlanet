/**
    @file   UIVertexModifier.cs
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  UIVertexModifier
    @date   2019.01.09
    @author 황준성(hns17.tistory.com)
    @brief  UI Vertex Modifier 추상클래스
*/
[RequireComponent(typeof(TextEffect))]
public abstract class UIVertexModifier : MonoBehaviour {
    //Text Animation 정보
    private TextEffect textEffect;

    //Animation 정보 수정 된 경우 업데이트
    private void OnValidate()
    {
        if (textEffect == null)
            textEffect = GetComponent<TextEffect>();

        textEffect.IsDirty = true;
    }

    //Vertex 정보 수정 함수
    public abstract void Apply(TextData txtData, ref UIVertex uiVertex);
}
