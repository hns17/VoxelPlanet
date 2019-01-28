/**
    @file   DocumentEvent.cs
    @date   2018.12.22
    @author 황준성(hns17.tistory.com)
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
    @class  DocumentEvent
    @date   2018.12.22
    @author 황준성(hns17.tistory.com)
    @brief  Document UI 를 활성화한다.
*/
public class DocumentEvent : MonoSingleton<DocumentEvent> {
    //보여 줄 이미지
    public RawImage image = null;

    private void Start()
    {
        if (image == null)
            image = GetComponent<RawImage>();
    }
    
    /**
        @brief  Document Image 를 보여준다.
        @param  texture : document image
    */
    public void EnableDocument(Texture2D texture)
    {
        image.texture = texture;
        image.enabled = true;

        StartCoroutine(DisableDocument());
    }

    /**
        @brief  Document Image 를 숨긴다.
    */
    IEnumerator DisableDocument()
    {
        while (true) {
            yield return null;

            if (Input.anyKeyDown)
                break;
        }

        image.enabled = false;
    }
}
