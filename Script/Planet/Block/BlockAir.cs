/**
    @file   BlockAir.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/
using System;

/**
    @class  BlockAir
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  공기 블럭, 메쉬정보가 없는 객체.
*/
[Serializable]
public class BlockAir : Block
{
    /**
        @brief  Mesh정보 구성 함수
        @return 없기 때문에 그대로 반환.
    */
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    /**
        @brief  Mesh정보가 있는지 검사하는 함수
        @return 없기 때문에 false 반환
    */
    public override int IsSolid()
    {
        return -1;
    }
}