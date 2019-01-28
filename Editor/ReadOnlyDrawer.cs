/**
    @file   ReadOnlyDrawer.cs
    @class  ReadOnlyDrawer
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @ref    https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    @brief  [ReadOnly]가 명시된 변수를 Inspector상에서 수정 불가 형태로 표시한다.
    
*/
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}