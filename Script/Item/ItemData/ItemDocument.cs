/**
    @file   ItemDocument.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
*/

using UnityEngine;
/**
    @class  ItemDocument
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  문서 아이템
*/

public class ItemDocument : Item {
    public Texture2D texture = null;

    public void Init(string name, ItemType type, string desc, bool isSell, Sprite icon, uint price, Texture2D texture)
    {
        base.Init(name, type, desc, isSell, icon, price);
        this.texture = texture;        
    }


    public override void Update()
    {
        if(texture != null)
            DocumentEvent.Instance.EnableDocument(texture);

    }
}
