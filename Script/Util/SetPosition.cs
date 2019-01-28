/**
    @file   SetPostion.cs
    @date   2018.12.28
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
/**
    @class  SetPostion
    @date   2018.12.28
    @author 황준성(hns17.tistory.com)
    @brief  지정된 Material에 오브젝트 위치값 전달
*/
public class SetPosition : MonoBehaviour {

	public Material mat;
	// Update is called once per frame
	void Update () {
		mat.SetVector("_Position", transform.position);
	}
}
