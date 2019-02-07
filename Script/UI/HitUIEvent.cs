/**
    @file   HitUIEvent.cs
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
    @class   HitUIEvent
    @date   2019.01.10
    @author 황준성(hns17.tistory.com)
    @brief  Damage를 표시한다.
*/
public class HitUIEvent : MonoBehaviour {
    //Damage를 표시할 텍스트 UI 리스트
    [SerializeField] private List<Text> hitTexts;
    [SerializeField] private Image hp;
    
    private void Start()
    {
        foreach (var text in hitTexts)
            text.gameObject.SetActive(false);
    }

    /**
        @brief  현재 사용되지 않은 Text UI를 찾아서 Damage 표시
        @param  damage : 표시될 문자
    */
    public void PrintHitText(int damage, float currentHpRate)
    {
        foreach (var text in hitTexts)
        {
            //사용 가능한 text UI 탐색
            if (!text.gameObject.activeSelf)
            {
                text.text = damage.ToString();
                text.gameObject.SetActive(true);

                hp.fillAmount = currentHpRate;

                //일정 시간 지나면 Disable한다.
                StartCoroutine(DisableHitText(text.gameObject, 1.5f));
                break;
            }
        }

    }

    /**
        @brief  text ui disable
        @param  hitText : text ui,  time : waiting time
    */
    IEnumerator DisableHitText(GameObject hitText, float time)
    {
        yield return Yields.WaitSeconds(time);
        hitText.SetActive(false);
    }

    private void Update()
    {
        bool isPrintText = false;

        //print 중인 text가 있는지 탐색
        foreach(var text in hitTexts)
        {
            if (text.gameObject.activeSelf)
            {
                isPrintText = true;
                break;
            }
        }

        //print 중인 text가 있으면 camera 방향으로 ui 회전
        if (isPrintText) {
            var tfCam = Camera.main.transform;
            transform.LookAt(tfCam.position);
            var euler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(0, euler.y, 0);

            hp.transform.parent.gameObject.SetActive(true);
        }
        else {
            hp.transform.parent.gameObject.SetActive(false);
        }

    }

}
