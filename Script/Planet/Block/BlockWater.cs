/**
    @file   BlockDesert.cs
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
*/
using System;

/**
    @class  BlockWater
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
    @brief  물물 블럭을 생성한다.
*/
[Serializable]
public class BlockWater : Block
{
    /**
           @brief  참조할 텍스쳐 정보를  타일로 구성
           @return 구성된 Tile 정보 반환
    */
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 3;
        tile.y = 1;
        return tile;
    }


    /**
       @brief  Mesh정보가 있는지 검사하는 함수
       @return 물은 투명이므로 false 반환
    */
    public override int IsSolid()
    {
        return 0;
    }
}


