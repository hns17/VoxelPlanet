/**
    @file   SpriteInfo.cs
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEngine.U2D;

/**
    @class  SpriteInfo
    @date   2018.12.26
    @author 황준성(hns17.tistory.com)
    @brief  SpriteSheet를 로드후 스프라이트를 반환한다.
*/
public class SpriteInfo {
    private SpriteAtlas atlas = null;
    private static SpriteInfo instance = null;
    
    public static SpriteInfo Instance
    {
        get {
            if (instance == null)
                instance = new SpriteInfo();
            return instance;
        }
    }
    
    /**
        @brief  sheet에서 스프라이트 반환
        @param  name : 스프라이트 이름
    */
    public Sprite GetSprite(string name)
    {
        if (atlas == null)
            atlas = Resources.Load("Atlas/UI") as SpriteAtlas;

        return atlas.GetSprite(name);
    }
}
