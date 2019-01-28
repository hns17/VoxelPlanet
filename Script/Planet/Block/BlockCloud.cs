/**
    @file   BlockCloud.cs
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
*/
using System;

/**
    @class  BlockCloud
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
    @brief  구름 블럭을 생성한다.
*/
[Serializable]
public class BlockCloud : Block
{

    /**
        @brief  참조할 텍스쳐 정보를 구름 타일로 구성
        @return 구성된 Tile 정보 반환
    */
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
   
        tile.x = 0;
        tile.y = 1;
        return tile;
    }
}