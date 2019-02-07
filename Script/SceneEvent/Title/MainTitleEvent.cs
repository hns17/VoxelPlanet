/**
    @file   MainTitleEvent.cs
    @date   2018.01.13
    @author 황준성(hns17.tistory.com)
    @brief  Title Scene Event
*/
using UnityEngine;


/**
    @class  MainTitleEvent
    @date   2018.01.13
    @author 황준성(hns17.tistory.com)
    @brief  게임 시작 및 종료 메뉴 선택 이벤트
*/
public class MainTitleEvent : MonoBehaviour {
    private void Start()
    {
        MyUtil.EnableCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            //Play Menu 와 Exit Menu 충돌 확인 후 이벤트 처리
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.name == "Play")
                    SceneControlManager.Instance.NextScene();
                else if (hit.transform.gameObject.name == "Exit")
                    Application.Quit();
            }
        }
    }

}
