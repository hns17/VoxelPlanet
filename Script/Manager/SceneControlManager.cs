/**
    @file   SceneControlManager.cs
    @date   2018.12.20
    @author 황준성(hns17.tistory.com)
    @brief  Projcet Scene Control
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
    @class  SceneControlManager
    @date   2018.12.20
    @author 황준성(hns17.tistory.com)
    @brief  프로젝트의 씬 전환 작업(Fade, unLoad, Load)을 수행한다.
*/
public class SceneControlManager : MonoSingleton<SceneControlManager> {
    //Scene Fade Class
    [SerializeField] private Fade fade;
    [SerializeField] private GameObject fadeCamera;


    public GameObject bgmMng;
    public GameObject ui;

    //현재 씬
    private int currentSceneIndex = 0;

    //전체 씬 정보<index, Scene NickName>
    private Dictionary<int, string> sceneList;

    private void Awake()
    {
        fadeCamera.SetActive(false);
        sceneList = new Dictionary<int, string>();

        sceneList[0] = "Title";
        sceneList[1] = "Temple";
        sceneList[2] = "VoxelWorld";


        //Main Scene Obj, 전환시 삭제 되지 않는다.
        //DontDestroyOnLoad를 사용하는 것 보다는 Additive Scene으로 관리하는걸 권장한다해서
        //만들었더니 오클루전 컬링 정보가 로드 되지 않는다.
        //라이트 맵은 로드 되는데 방법이 따로 있는지 해결 법 찾을때 까지는 Single로 로드해서 사용해야 할것 같다.
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(bgmMng);
        DontDestroyOnLoad(ui);

    }

    private void Start () {
        //0번 씬 로드
        SceneManager.LoadScene(sceneList[currentSceneIndex], LoadSceneMode.Single);
	}
	

    /**
        @brief  다음 씬으로 전환
    */
    public void NextScene()
    {
        int idx = currentSceneIndex + 1;
        if (idx < sceneList.Count)
            StartCoroutine(ChangeScene(sceneList[idx]));
    }

    /**
        @brief  이전 씬으로 전환
    */
    public void PrevScene()
    {
        int idx = currentSceneIndex - 1;
        if (idx >= 0)
            StartCoroutine(ChangeScene(sceneList[idx]));
    }

    /**
        @brief  첫번째 씬으로 전환
    */
    public void FirstScene()
    {
        StartCoroutine(ChangeScene(sceneList[0]));
    }

    /**
        @brief  지정된 씬으로 전환
    */
    public void LoadScene(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName));
    }

    /**
        @brief  씬 전환 이벤트
        @param  sceneName : 전환하려는 scene Nick Name
        @detail 현재 화면을 로딩 화면으로 Fade후 현재 씬 UnLoad
                UnLoad가 끝나면 Load, 완료 후 로드된 씬 화면으로 Fade
                UnLoad를 기다리지 않고 바로 로딩하거나 UnLoading을 마지막에 하는게 좋을지도...
    */
    public IEnumerator ChangeScene(string sceneName)
    {
        //화면 Fade
        yield return fade.FadeIn(2);
        fadeCamera.SetActive(true);
        //Scene Load & UnLoad
        //yield return SceneManager.UnloadSceneAsync(sceneList[currentSceneIndex]);
        yield return LoadingScene(sceneName);

        //화면 Fade
        fadeCamera.SetActive(false);
        yield return fade.FadeOut(2);
    }


    /**
        @brief  SceneLoading Event, 씬을 로딩하며 작업 완료를 체크
        @param  sceneName : Loading Scene Nick Name
    */
    IEnumerator LoadingScene(string sceneName)
    {
        
        AsyncOperation ao;
        ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        ao.allowSceneActivation = false;

        while (!ao.isDone) {
            if (ao.progress == 0.9f)
                ao.allowSceneActivation = true;

            yield return null;
        }

        //로딩 후 CurrentIndex 변경
        foreach (var keyvaluepair in sceneList) {
            if (keyvaluepair.Value ==  sceneName) {
                currentSceneIndex = keyvaluepair.Key;    
                break;
            }
        }
    }

}
