/**
    @file   VoxelClouds.cs
    @class  VoxelClouds
    @date   2018.08.08
    @author 황준성(hns17.tistory.com)
    @brief  구름 청크/회전 정보
 */
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VoxelClouds : VoxelPlanet {

    public Vector3 axis = Vector3.zero;
    public float rotSpeed = 0.0f;

    
    private void FixedUpdate()
    {
        transform.Rotate(axis, Time.fixedDeltaTime * rotSpeed);
    }

    /**
        @brief  청크를 생성하고, 블럭을 구성한다.
     */
    public IEnumerator BuildChunk()
    {
        Debug.Log("Build");
        var info = planetInfo.cloudInfo;
        float exceptRange = info.groundFromDist + info.chunkSize;

        //구름 생성 영역을 chunk 에 맞춰 보간
        info.radius = Mathf.FloorToInt((exceptRange + info.chunkSize) / info.chunkSize) * info.chunkSize;


        //청크 생성 Loop문에 사용될 파라미터 
        int radius = info.radius;
        int chunkSize = info.chunkSize;

        //행성 기준 좌표 위치값
        Vector3 planetPos = transform.parent.position;

        //디폴트 청크 오브젝트 리드
        GameObject chunkPrefab = Resources.Load("Prefabs/VoxelPlanet/Chunk_Cloud") as GameObject;


        //청크 생성
        for (int y = -radius; y < radius; y += chunkSize)
        {
            for (int z = -radius; z < radius; z += chunkSize)
            {
                for (int x = -radius; x < radius; x += chunkSize)
                {
                    Vector3Int chunkPos = new Vector3Int(x, y, z);
                    Vector3 chunkWorldPos = chunkPos + planetPos;

                    //생성 외 영역이면 생성하지 않는다.
                   
                    int condition = MyUtil.IsCubeInRange(chunkPos, chunkSize, exceptRange);
                    if (condition == 8 || condition == 0)
                        continue;

                    //청크 생성
                    GameObject newChunkObj = Instantiate(chunkPrefab, chunkWorldPos, Quaternion.Euler(Vector3.zero)) as GameObject;
                    newChunkObj.name = "Chunk(" + x + ", " + y + ", " + z + ")";
                    newChunkObj.transform.parent = this.transform;

                    //Chunk 컴포넌트 가져오기
                    Chunk newChunk = newChunkObj.GetComponent<Chunk>();
                    newChunk.InitChunk(chunkSize, chunkPos);
                    newChunk.isSphere = planetInfo.isSphere;

                    //블럭 구성
                    BuildBlocks(newChunk);
                }
                yield return null;
            }
        }

        Debug.Log("Finish");
    }


    /**
       @brief  청크의 블럭을 구성한다.
    */
    public override void BuildBlocks(Chunk chunk)
    {
        var parent = chunk.transform.parent;
        int chunkSize = chunk.chunkSize;

        if (chunk.blocks == null)
            chunk.blocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3Int worldPos = new Vector3Int(chunk.pos.x + x, chunk.pos.y + y, chunk.pos.z + z);
                    chunk.SetBlock(x, y, z, CreateBlock(worldPos));
                }
            }
        }

        UpdateModifyBlockArea(chunk, true);
    }

    

    /**
        @brief  구름 블럭을 생성한다.
        @return 만들어진 지형을 반환한다.
    */
    private Block CreateBlock(Vector3Int worldPos)
    {
        Block newBlock = null;
        var info = planetInfo.cloudInfo;

        //구름의 높이 밀도
        float heightDensity = info.radius;
        heightDensity -= info.chunkSize * (1 - info.densityHeight);

        //heightDensity 내인 경우 노이즈 함수를 이용해 구름 생성
        if (MyUtil.IsCubeInRange(worldPos, 1, heightDensity) != 0)
        {
            float noise = (MyUtil.Get3DNoise(worldPos.x, worldPos.y, worldPos.z, info.frequency,
                                             info.octave, info.persistence) + 1.0f) * 0.5f;

            //구름의 너비 밀도
            if (noise < info.densityWidth)
                newBlock = new BlockCloud();
        }

        if (newBlock == null)
            newBlock = new BlockAir();

        return newBlock;
    }
    
}
