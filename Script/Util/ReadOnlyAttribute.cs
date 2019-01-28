/**
    @file   ReadOnlyAttribute.cs
    @class  ReadOnlyAttribute
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @ref    https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    @brief  변수를 ReadOnly 형태로 Inspector에 표시하기 위한 Attribute,
            [ReadOnly]가 명시된 변수는 Editor/ReadOnlyDrawer에 의해 Custom된다.
    
*/

using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute { }