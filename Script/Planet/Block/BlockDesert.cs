/**
    @file   BlockDesert.cs
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
*/
using System;

/**
    @class  BlockDesert
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
    @brief  모래 블럭을 생성한다.
*/
[Serializable]
public class BlockDesert : Block
{

    /**
       @brief  참조할 텍스쳐 정보를 모래 타일로 구성
       @return 구성된 Tile 정보 반환
    */
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 1;
        tile.y = 1;
        return tile;
    }
}