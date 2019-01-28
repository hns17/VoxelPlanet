/**
    @file   LoadScene.cs
    @date   2019.01.13
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System.Collections;

/**
    @class   LoadScene
    @date   2019.01.13
    @author 황준성(hns17.tistory.com)
    @brief  지정된 씬으로 전환한다.
*/
public class LoadScene : MonoBehaviour {
    //로드할 씬 이름
    public string sceneName;           
    
    //공지 텍스트
    public string notifyText = "입장하기";

    private IEnumerator loadSceneEvent;

    private void Start()
    {
        loadSceneEvent = LoadSceneEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            NotifyMessageEvent.Instance.PrintMainText(notifyText);
            StartCoroutine(loadSceneEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            NotifyMessageEvent.Instance.MainTextDisable();
            StopCoroutine(loadSceneEvent);
        }
        
    }

    /**
        @brief  씬을 로드한다.
    */
    private IEnumerator LoadSceneEvent()
    {
        while (true) {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E)) {
                NotifyMessageEvent.Instance.MainTextDisable();
                SceneControlManager.Instance.LoadScene(sceneName);
                break;
            }
        }
    }
}
