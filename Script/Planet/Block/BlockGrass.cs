/**
    @file   BlockGrass.cs
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
*/
using System;

/**
    @class  BlockGrass
    @date   2018.08.01
    @author 황준성(hns17.tistory.com)
    @brief  잔디 블럭을 생성한다.
*/
[Serializable]
public class BlockGrass : Block
{
    public BlockGrass()
        : base()
    {
    }

    /**
        @brief  참조할 텍스쳐 정보를 잔디 타일로 구성
        @return 구성된 Tile 정보 반환
    */
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        //switch (direction)
        //{
        //    case Direction.up:
        //        tile.x = 2;
        //        tile.y = 0;
        //        return tile;
        //    case Direction.down:
        //        tile.x = 1;
        //        tile.y = 0;
        //        return tile;
        //}
        tile.x = 2;
        tile.y = 0;
        return tile;
    }
}