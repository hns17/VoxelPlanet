/**
    @file   WorldPos.cs 
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/
using System;
using UnityEngine;

/**
    @struct WorldPos
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Vector3 or Vector3Int에 저장된 데이터 정보 보관
    @detail Map의 정보를 Serializable을 통해 저장하는데, Vector3, Vector3Int는
            C# 기본 자료구조가 아니므로 사용할 수 없다.
            Save와 Load시 사용될 위치정보를 저장하기 위해 만든 구조체.
*/
[Serializable]
public struct WorldPos {

    public int x, y, z;
   
    public WorldPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /**
        @brief  캐스팅 연산자 함수
        @detail 대입연산시 형 변환하여 값을 대입한다.
                (Vector3Int variable = WorldPos value) 
    */
    public static explicit operator Vector3Int(WorldPos worldPos)
    {
        return new Vector3Int(worldPos.x, worldPos.y, worldPos.z);
    }

    /**
        @brief  캐스팅 연산자 함수
        @detail 대입연산시 형 변환하여 값을 대입한다.
                (WorldPos variable = Vector3Int value) 
    */
    public static explicit operator WorldPos(Vector3Int pos)
    {
        return new WorldPos(pos.x, pos.y, pos.z);
    }
}
