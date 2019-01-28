/**
    @file   NotifyMessage.cs
    @date   2019.01.05
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;

/**
    @class  NotifyMessage
    @date   2019.01.05
    @author 황준성(hns17.tistory.com)
    @brief  화면 중앙에 지정된 메시지를 표시한다.
*/
public class NotifyMessage : MonoBehaviour {
    //공지 할 메시지
    [SerializeField] private string message;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            NotifyMessageEvent.Instance.PrintMainText(message);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            NotifyMessageEvent.Instance.MainTextDisable();
    }
}
