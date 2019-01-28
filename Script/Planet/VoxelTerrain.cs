using UnityEngine;
using System.Collections;

/**
    @file   VoxelTerrain.cs
    @class  VoxelTerrain
    @date   2018.08.08
    @author 황준성(hns17.tistory.com)
    @brief  지형 생성 및 청크 정보
 */
public class VoxelTerrain : VoxelPlanet
{
    /**
        @brief  청크를 생성하고, 블럭을 구성한다.
     */
    public IEnumerator BuildChunk()
    {
        Debug.Log("Build");

        var info = planetInfo.terrainInfo;
       
        //지형 생성 영역을 청크사이즈에 맞게 보간
        info.radius = Mathf.FloorToInt(info.radius / info.chunkSize) * info.chunkSize;

        //청크 생성 Loop문에 사용될 파라미터 
        int radius = info.radius;
        int chunkSize = info.chunkSize;

        float exceptRange = info.radius * info.coreRate;

        //행성 기준 좌표 위치값
        Vector3 planetPos = transform.parent.position;

        //디폴트 청크 오브젝트 리드
        GameObject chunkPrefab = Resources.Load("Prefabs/VoxelPlanet/Chunk_Terrain") as GameObject;
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
                    if (MyUtil.IsCubeInRange(chunkPos, chunkSize, exceptRange) == 8)
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
        @brief  지형 블럭을 생성한다.
        @return 만들어진 지형을 반환한다.
    */
    private Block CreateBlock(Vector3Int worldPos)
    {
        Block newBlock = null;
        var info = planetInfo.terrainInfo;

        float coreRange = info.radius * info.coreRate;

        //코어의 표면은 블럭으로 생성
        int isInVertCnt = MyUtil.IsCubeInRange(worldPos, 1, coreRange);
        if (isInVertCnt > 0 && isInVertCnt < 8)
        {
            newBlock = new Block();
            newBlock.isLock = true;
        }
        //코어 외부는 Mode에 따라 함수를 통해 생성
        else if (isInVertCnt == 0)
        {
            if (MyUtil.GetSNoise(worldPos.x, worldPos.y, worldPos.z, 0.05f, 50) > 20)
            {
                float noise = (MyUtil.Get3DNoise(worldPos.x, worldPos.y, worldPos.z, info.frequency,
                                            info.octave, info.persistence) + 1.0f) * 0.5f;
                //int holeChance = MyUtil.GetSNoise(worldPos.x, worldPos.y, worldPos.z, 0.05f, 50);

                noise = Mathf.Clamp01(noise);

                if (noise <= info.blockRate)
                    newBlock = new Block();
                else if (noise <= info.desertRate)
                    newBlock = new BlockDesert();
                else if (noise <= info.grassRate)
                    newBlock = new BlockGrass();
            }
        }

        //Ocean BLock
        if (newBlock == null)
        {
            float height = Mathf.Max(Mathf.Abs(worldPos.x + 0.5f), Mathf.Abs(worldPos.y + 0.5f), Mathf.Abs(worldPos.z+0.5f));
            
            if (height <= info.oceanHeight && height > info.oceanHeight - info.oceanDepth)
            {
                newBlock = new BlockWater();
                newBlock.isLock = true;
            }
        }

        if (newBlock == null)
            newBlock = new BlockAir();

        return newBlock;
    }
    
}