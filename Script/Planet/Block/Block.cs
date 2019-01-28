/**
    @file   Block.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using System;

/**
    @class  Block
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  모든 블럭의 기본이 되는 객체
*/
[Serializable]
public class Block {
    //블럭의 면
    public enum Direction { front, right, back, left, up, down };

    //Block Change Flag,
    public bool changed = false;

    //블럭 잠금, true면 변경 및 파괴 불가능한 블럭
    public bool isLock = false;                    

    //uv Scale
    const float tileSize = 0.25f;

    /**
        @struct Tile
        @date   2018.07.26
        @author 황준성(hns17.tistory.com)
        @brief  Face의 UV 정보
    */
    public struct Tile { public int x; public int y; }
  
    /**
        @brief  참조할 타일 텍스쳐 정보를 생성한다.
        @return 생성된 타일 정보 반환
    */
    public virtual Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    /**
       @brief  타일 정보를 기반으로 Face의 UV구성
       @return 생성된 UV 정보 반환
   */
    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        return UVs;
    }

    /**
        @brief  Mesh 정보가 있는 BLock 인지 검사합니다.
        @return Mesh정보가 있는 경우 true, 없는 경우 false
    */
    public virtual int IsSolid()
    {
        return 1;
    }

    /**
        @brief  상단 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y + 1.0f, z +1.0f));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z + 1.0f));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z));
        meshData.AddVertex(new Vector3(x, y + 1.0f, z));
        meshData.AddQuadTriangles();

        meshData.uv.AddRange(FaceUVs(Direction.up));
        
        return meshData;
    }

    /**
        @brief  하단 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y, z + 1.0f));
        meshData.AddVertex(new Vector3(x, y, z + 1.0f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    /**
        @brief  뒷 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataBack (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 1.0f, y, z + 1.0f));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z + 1.0f));
        meshData.AddVertex(new Vector3(x, y + 1.0f, z + 1.0f));
        meshData.AddVertex(new Vector3(x, y, z + 1.0f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.back));
        return meshData;
    }

    /**
        @brief  오른쪽 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataRight (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 1.0f, y, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z + 1.0f));
        meshData.AddVertex(new Vector3(x + 1.0f, y, z + 1.0f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.right));
        return meshData;
    }

    /**
        @brief  앞 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataFront (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x, y + 1.0f, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y + 1.0f, z));
        meshData.AddVertex(new Vector3(x + 1.0f, y, z));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.front));
        return meshData;
    }

    /**
        @brief  왼쪽 면 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    protected virtual MeshData FaceDataLeft (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y, z + 1.0f));
        meshData.AddVertex(new Vector3(x, y + 1.0f, z + 1.0f));
        meshData.AddVertex(new Vector3(x, y + 1.0f, z));
        meshData.AddVertex(new Vector3(x, y, z));

        
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.left));
        return meshData;
    }



    /**
        @brief  블럭의 Mesh정보 구성
        @return 구성된 MeshData 반환
    */
    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        //충돌 영역 생성 플래그
        meshData.useRenderDataForCol = true;

        var solid = IsSolid();

        //이웃한 블럭을 탐색하여, 블럭이 존재하면 겹치는 면을 제외하고 메쉬 생성
        if (solid > chunk.GetBlock(x, y + 1, z).IsSolid())
            meshData = FaceDataUp(chunk, x, y, z, meshData);

        if (solid > chunk.GetBlock(x, y - 1, z).IsSolid())
            meshData = FaceDataDown(chunk, x, y, z, meshData);

        if (solid > chunk.GetBlock(x, y, z + 1).IsSolid())
            meshData = FaceDataBack(chunk, x, y, z, meshData);

        if (solid > chunk.GetBlock(x, y, z - 1).IsSolid())
            meshData = FaceDataFront(chunk, x, y, z, meshData);

        if (solid > chunk.GetBlock(x + 1, y, z).IsSolid())
            meshData = FaceDataRight(chunk, x, y, z, meshData);

        if (solid > chunk.GetBlock(x - 1, y, z).IsSolid())
            meshData = FaceDataLeft(chunk, x, y, z, meshData);

        return meshData;
    }

    
}
