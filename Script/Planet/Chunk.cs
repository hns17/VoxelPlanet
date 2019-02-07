/**
    @file   Chunk.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/

using UnityEngine;

//컴포넌트에 추가.
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

/**
    @class  Chunk
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Planet을 구성하는 Block의 집합
*/
[System.Serializable][ExecuteInEditMode]
public class Chunk : MonoBehaviour {
   
    public bool             update      = false;                     //업데이트 Flag
    public bool             rendered    = true;                     //드로우 Flag
    public bool             isSphere    = false;

    public int              chunkSize;                              //블록 공간 크기 정보
    public Vector3Int       pos;                                    //World Position


    MeshFilter mainFilter;         //메쉬 정보
    MeshCollider mainColl;         //충돌 메쉬 정보

    MeshFilter waterFilter;         //메쉬 정보
    MeshCollider waterColl;         //충돌 메쉬 정보

    //블럭 정보
    public  Block[,,]       blocks;

    bool                    isRebuild   = false;

    private void Awake()
    {
        mainFilter = GetComponent<MeshFilter>();
        mainColl = GetComponent<MeshCollider>();

        waterFilter = transform.GetChild(0).GetComponent<MeshFilter>();
        waterColl = transform.GetChild(0).GetComponent<MeshCollider>();
    }

    public Chunk(int chunkSize, Vector3Int pos)
    {
        this.chunkSize = chunkSize;
        this.pos = pos;
    }

    /**
        @brief  Chunk Update가 필요하면 update 수행
     */
    void Update()
    {
        if (update == true)
            UpdateChunk();

        update = false;
        
    }

    /**
        @brief  Chunk 정보 초기화
     */
    public void InitChunk(int chunkSize, Vector3Int chunkPos)
    {
        this.chunkSize = chunkSize;
        this.pos = chunkPos;

        blocks = new Block[chunkSize, chunkSize, chunkSize];
    }

   
    /**
        @brief  큐브 형태로 만들어진 청크를 구면 메시로 변환
        @param  meshData 변경할 meshData
        @return 변경된 메쉬데이터
    */
    private MeshData CubeToSphere(MeshData meshData)
    {
        
        for (int i = 0; i < meshData.vertices.Count; i++)
        {
            //정점 좌표를 월드 기준으로 변환
            Vector3 vec = meshData.vertices[i] + pos ;
            vec = MyUtil.RoundVector3(vec);

            float len = Mathf.Max(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));


            vec = vec.normalized * len;
            meshData.vertices[i] = vec - pos;
        }
        

        return meshData;
    }

    /**
        @brief  ChunkSize를 벗어나는지 검사
        return  벗어나면 false, 아니면 true
     */
    public bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;
        
        return true;
    }


    /**
        @brief  Block의 WorldPos 기준으로 Chunk를 탐색
        return  탐색한 chunk정보 반환
     */
    public Chunk GetChunk(Vector3 worldPos)
    {
        Vector3Int pos = Vector3Int.zero;

        float multiple = chunkSize;
        pos.x = Mathf.FloorToInt(worldPos.x / multiple) * chunkSize;
        pos.y = Mathf.FloorToInt(worldPos.y / multiple) * chunkSize;
        pos.z = Mathf.FloorToInt(worldPos.z / multiple) * chunkSize;

        string name = "Chunk(" + pos.x + ", " + pos.y + ", " + pos.z + ")";

        var objChunk = transform.parent.Find(name);

        if (objChunk == null)
            return null;

        return objChunk.GetComponent<Chunk>();
    }

    /**
        @brief  Block을 찾는다.
        return  탐색한 Block정보 반환
     */
    public Block GetBlock(int x, int y, int z)
    {
        if (blocks == null)
            RebuildBlock();

        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];

        return GetBlock(new Vector3Int(pos.x + x, pos.y + y, pos.z + z));
    }

    /**
         @brief  World에서 Block을 찾는다.
         return  탐색한 Block정보 반환
     */
    public Block GetBlock(Vector3Int worldPos)
    {
        Chunk chunk = GetChunk(worldPos);

        if (chunk != null)
            return chunk.GetBlock(worldPos.x - chunk.pos.x, worldPos.y - chunk.pos.y, worldPos.z - chunk.pos.z);

        return new BlockAir();
    }

    /**
          @brief  Block정보를 변경한다.
     */
    public void SetBlock(int x, int y, int z, Block block)
    {
        //Block정보가 없으면 재구성
        if (blocks == null)
            RebuildBlock();


        Vector3Int worldPos = new Vector3Int(pos.x + x, pos.y + y, pos.z + z);

        //찾으려는 블럭이 Chunk 범위 내에 존재하는지 체크 후 변경
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;


            if (isRebuild)
                return;

            //블럭 정보가 변경되면 청크 업데이트
            update = true;
            //block.changed = true;

            //이웃 청크 정보도 업데이트
            UpdateIfEqual(worldPos.x - pos.x, 0, new Vector3Int(worldPos.x - 1, worldPos.y, worldPos.z));
            UpdateIfEqual(worldPos.x - pos.x, chunkSize - 1, new Vector3Int(worldPos.x + 1, worldPos.y, worldPos.z));
            UpdateIfEqual(worldPos.y - pos.y, 0, new Vector3Int(worldPos.x, worldPos.y - 1, worldPos.z));
            UpdateIfEqual(worldPos.y - pos.y, chunkSize - 1, new Vector3Int(worldPos.x, worldPos.y + 1, worldPos.z));
            UpdateIfEqual(worldPos.z - pos.z, 0, new Vector3Int(worldPos.x, worldPos.y, worldPos.z - 1));
            UpdateIfEqual(worldPos.z - pos.z, chunkSize - 1, new Vector3Int(worldPos.x, worldPos.y, worldPos.z + 1));
        }
        //없으면 월드 기준으로 찾아서 변경
        else
        {
            SetBlock(worldPos, block);
        }


    }

    /**
          @brief  World 에서 Block 탐색 후 변경한다.
     */
    public void SetBlock(Vector3Int worldPos, Block block)
    {
        Chunk chunk = GetChunk(worldPos);

        if (chunk != null)
            chunk.SetBlock(worldPos.x - chunk.pos.x, worldPos.y - chunk.pos.y, worldPos.z - chunk.pos.z, block);
    }


    /**
          @brief  Block정보를 재구성 한다.
     */
    public void RebuildBlock()
    {
        isRebuild = true;
        var saveInfo = Serialization.LoadPlanet(Serialization.GetCurrentPlanetInfoPath());
        transform.parent.parent.GetComponent<VoxelPlanetInfo>().ReLoadInfo(saveInfo);
        Serialization.LoadChunk(this);


        isRebuild = false;
        
    }

    /**
           @brief   변경된 블럭이 이웃 청크의 블럭과 연결되어 
                    있으면 이웃 청크 정보도 업데이트한다.
     */
    void UpdateIfEqual(int value1, int value2, Vector3Int pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos);
            if (chunk != null)
                chunk.update = true;
        }
    }
    
    /**
          @brief   Chunk Mesh 정보 업데이트.
    */
    public void UpdateChunk()
    {
        rendered = true;
        MeshData meshData = new MeshData();
        MeshData waterMeshData = new MeshData();
        //Chunk 메쉬 정보 구성
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    var block = GetBlock(x, y, z);
                    if (block.GetType() == typeof(BlockWater))
                        waterMeshData = block.Blockdata(this, x, y, z, waterMeshData);
                    else
                        meshData = block.Blockdata(this, x, y, z, meshData);

                }
            }
        }

        if (isSphere)
        {
            CubeToSphere(meshData);
            CubeToSphere(waterMeshData);
        }


        //구성된 데이터로 Object Mesh 정보 갱신
        RenderMesh(meshData, mainFilter, mainColl);
        RenderMesh(waterMeshData, waterFilter, waterColl);
    }

    /**
          @brief   Chunk Object Mesh 정보 갱신
    */
    void RenderMesh(MeshData meshData, MeshFilter filter, MeshCollider coll)
    {
        
        //MeshRenderer render = GetComponent<MeshRenderer>();     //충돌 메쉬 정보

        //메쉬 정보 구성

        Mesh mesh = new Mesh();

        mesh.Clear();
        mesh.vertices = meshData.vertices.ToArray();
        mesh.triangles = meshData.triangles.ToArray();
        mesh.uv = meshData.uv.ToArray();
        mesh.RecalculateNormals();

        filter.mesh = mesh;

        //충돌 메쉬 구성
        coll.sharedMesh = null;
        Mesh collMesh = new Mesh();
        collMesh.vertices = meshData.colVertices.ToArray();
        collMesh.triangles = meshData.colTriangles.ToArray();
        collMesh.RecalculateNormals();

        coll.sharedMesh = mesh;
        
    }
}
