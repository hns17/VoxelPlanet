/**
    @file   ObjectUIEvent.cs
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/**
    @class  ObjectUIEvent
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
    @brief  Object의 이름을 표시한다.
*/
public class ObjectUIEvent : MonoBehaviour {
    //표시할 이름
    [SerializeField] public string text;

    //Event distance
    [SerializeField] private float dist = 5;

    //text ui
    private Text infoText;
    


    private void Start()
    {
        infoText = GetComponentInChildren<Text>();

        infoText.text = text;
        infoText.gameObject.SetActive(false);

        StartCoroutine(PrintObjInfo());
    }


    /**
        @brief  UI를 MainCamera 방향으로 회전 하여 보여준다.
    */
    IEnumerator PrintObjInfo()
    {
        var tfCam = Camera.main.transform;
        var info = PlayerInfo.Instance;
        while (true) {
            yield return new WaitForSeconds(0.1f);
            
            var pos = info.transform.position;
            if (Vector3.Distance(transform.position, pos) > dist) {
                infoText.gameObject.SetActive(false);
                continue;
            }

            infoText.gameObject.SetActive(true);
            transform.LookAt(tfCam.position);
            var euler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(0, euler.y, 0);
        }
    }
}
