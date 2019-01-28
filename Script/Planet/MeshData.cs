/**
    @file   MeshData.cs
    @class  MeshData
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  메쉬 정보 관리를 위한 객체
*/
using System.Collections.Generic;
using UnityEngine;


public class MeshData {

    //메쉬 정보 변수 들
    public List<Vector3>    vertices                = new List<Vector3>();      //정점 정보
    public List<Vector2>    uv                      = new List<Vector2>();      //uv 정보
    public List<Vector3>    colVertices             = new List<Vector3>();      //충돌 영역 정점 정보

    public List<int>        triangles               = new List<int>();          //메쉬 인덱스
    public List<int>        colTriangles            = new List<int>();          //충돌 영역 인덱스
    
    public bool             useRenderDataForCol     = false;                    //충돌 영역 생성 Flag
    public MeshData() { }

    

    /**
        @brief  quad face index 구성
     */
    public void AddQuadTriangles()
    {
        //메쉬 인덱스 생성
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        //충돌 메쉬 인덱스 생성
        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }

    /**
        @brief  index 정보 추가
     */
    public void AddTriangle(int tri)
    {
        //메쉬 인덱스 생성
        triangles.Add(tri);

        //충돌 메쉬 인덱스 생성
        if (useRenderDataForCol)
        {
            colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }


    /**
        @brief  정점 정보 추가
     */
    public void AddVertex(Vector3 vertex)
    {
        //정점 정보 추가    
        vertices.Add(vertex);

        //충돌 정점 정보 추가.
        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }
    }


}
