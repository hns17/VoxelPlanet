/**
    @file   VoxelPlanetInfo.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
/**
    @class  CommonInfo
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Planet생성에 사용 될 공통 정보
*/
[Serializable] 
public abstract class CommonInfo
{
    [Header("[Noise Info]")]
    [ReadOnly] public float frequency = 0.01f;
    [ReadOnly] public float persistence = 0.1f;
    [ReadOnly] public int octave = 1;

    [Header("[Object Info]")]
    [ReadOnly] public int radius = 64;
    [ReadOnly] public int chunkSize = 16;

    protected CommonInfo() { }

    protected CommonInfo(CommonInfo copyThis) : this()
    {
        this.frequency = copyThis.frequency;
        this.persistence = copyThis.persistence;
        this.octave = copyThis.octave;

        this.radius = copyThis.radius;
        this.chunkSize = copyThis.chunkSize;
    }
}


/**
    @class  TerrainInfo
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  지형 생성에 사용 될 정보
*/
[Serializable]
public class TerrainInfo : CommonInfo
{
    [Header("[TerrainRate Info]")]
    [ReadOnly] public float coreRate    = 0.1f;     //내핵 범위
    [ReadOnly] public float blockRate   = 0.25f;    //블럭 비율
    [ReadOnly] public float desertRate  = 0.35f;    //모래 비율
    [ReadOnly] public float grassRate   = 0.5f;     //풀 비율

    [Header("[OceanInfo Info]")]
    [ReadOnly] public int oceanHeight = 30;    //바다 높이
    [ReadOnly] public int oceanDepth = 1;    //바다 깊이

    public TerrainInfo() : base() { }

    protected TerrainInfo(TerrainInfo copyThis) : base(copyThis)
    {
        this.coreRate = copyThis.coreRate;
        this.blockRate = copyThis.blockRate;
        this.desertRate = copyThis.desertRate;
        this.grassRate = copyThis.grassRate;

        this.oceanHeight = copyThis.oceanHeight;
        this.oceanDepth = copyThis.oceanDepth;
    }


    public TerrainInfo DeepCopy()
    {
        return new TerrainInfo(this);
    }
}


/**
    @class  CloudInfo
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  구름 생성에 사용 될 정보
*/
[Serializable]
public class CloudInfo : CommonInfo
{
    [ReadOnly] public int   groundFromDist;           //지면으로 부터의 거리
    [ReadOnly] public float densityHeight   = 0.5f;   //구름의 높이 밀도
    [ReadOnly] public float densityWidth    = 0.1f;   //구름의 너비 밀도

    public CloudInfo() : base() { }

    protected CloudInfo(CloudInfo copyThis) : base(copyThis)
    {
        this.groundFromDist = copyThis.groundFromDist;
        this.densityHeight = copyThis.densityHeight;
        this.densityWidth = copyThis.densityWidth;
    }


    public CloudInfo DeepCopy()
    {
        return new CloudInfo(this);
    }
}


/**
    @class  VoxelPlanetInfo
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  복셀행성 생성에 사용되는 정보
*/
public class VoxelPlanetInfo : MonoBehaviour
{
    //행성 이름
    [ReadOnly] public string planetName;

    //지형 및 구름의 청크 생성 정보
    [SerializeField] public TerrainInfo terrainInfo;
    [SerializeField] public CloudInfo cloudInfo;

    //추가 수정이 적용된 Area 정보
    public Dictionary<WorldPos, List<ModifyArea>> modifyList = new Dictionary<WorldPos, List<ModifyArea>>();

    //AreaObjectList
    public Dictionary<string, GameObject> modifyAreaObject = new Dictionary<string, GameObject>();

    //Planet의 형태가 구형인가 체크
    [HideInInspector] public bool isSphere = false;


    private void Awake()
    {
        ReLoadInfo(Serialization.LoadPlanet(Serialization.GetCurrentPlanetInfoPath()));
    }

    /**
        @brief  Planet Object를 찾아서 반환한다.
        @return 찾은 Planet GameObject를 리턴한다.
    */
    public static GameObject GetVoxelPlanet()
    {
        return GameObject.FindGameObjectWithTag("VoxelPlanet");
    }


    /**
        @brief      Planet정보 초기화
    */
    public void Init(string planetName)
    {
        isSphere = false;
        this.planetName = planetName;
        

        modifyAreaObject.Clear();
        modifyList.Clear();
    }


    /**
        @brief      Load된 정보로 Planet정보 구성
        @param      loadInfo : Load된 정보
    */
    public void ReLoadInfo(SavePlanet loadInfo)
    {
        if (loadInfo == null)
            return;

        Init(loadInfo.name);
        
        isSphere = loadInfo.isSphere;

        modifyList = new Dictionary<WorldPos, List<ModifyArea>>(loadInfo.modifyList);

        //Area Object 정보를 구성하고 생성한다. 
        if (loadInfo.areaObjPath != null) {
            foreach (var objPath in loadInfo.areaObjPath) {
                if (modifyAreaObject.ContainsKey(objPath)) {
                    if (modifyAreaObject[objPath] != null)
                        continue;
                }

                var obj = Instantiate(Resources.Load(objPath), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
               
                //생성된 Object는 참조 할 때만 활성화 
                obj.SetActive(false);
                
                //렌더 비활성화.
                var mesh = obj.GetComponent<MeshRenderer>();
                if (mesh != null)
                    mesh.enabled = false;

                //인스펙터에서 숨긴다.
                obj.hideFlags = HideFlags.HideInHierarchy;
                modifyAreaObject[objPath] = obj;
            }
        }

        if(loadInfo.terrainInfo != null)
            terrainInfo = loadInfo.terrainInfo.DeepCopy();

        if (loadInfo.cloudsInfo != null)
            cloudInfo = loadInfo.cloudsInfo.DeepCopy();
    }
}


/**
    @class  VoxelPlanet
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Terrain과 Clouds의 Base Class
 */
public abstract class VoxelPlanet : MonoBehaviour
{
    public VoxelPlanetInfo planetInfo;
    
    /**
        @brief      변경될 청크와 변경 정보 및 CustomObject 정보를 구성
        @param      pos : 변경될 ChunkPos, area : AreaInfo, collidder : AreaCollider, path : prefab path
    */
    public void SetModifyArea(Vector3Int pos, ModifyArea area, Collider collider, string path)
    {
        //충돌 에리어 정보구성
        area.assetPath = "Prefabs/CustomArea/" + Path.GetFileNameWithoutExtension(path);
        area.isSphere = planetInfo.isSphere;
        area.SetTransformInfo(collider.transform);

        planetInfo.modifyAreaObject[area.assetPath] = collider.gameObject;
        
        if (planetInfo.modifyList == null)
            planetInfo.modifyList = new Dictionary<WorldPos, List<ModifyArea>>();
        
        //Modify 정보 추가
        WorldPos position = (WorldPos)pos;
        if(!planetInfo.modifyList.ContainsKey(position))
            planetInfo.modifyList[position] = new List<ModifyArea>();

        planetInfo.modifyList[position].Add(area.DeepCopy());
    }


    /**
        @brief      Block 정보를 수정
        @param      chunk : chunk 정보, isBuildPlanet : 맵을 생성하는 중인가?
    */
    public void UpdateModifyBlockArea(Chunk chunk, bool isBuildPlanet)
    {
        WorldPos position = (WorldPos)chunk.pos;
        if (planetInfo.modifyList != null)
        {
            //청크에 수정된 정보가 있으면 수정함수 호출
            if(planetInfo.modifyList.ContainsKey(position))
                ModifyBlock(chunk, planetInfo.modifyList[position], isBuildPlanet);
        }
    }

    /**
        @brief      변경정보를 참조해 해당 chunk의 block 정보를 변경
        @param      chunk : 변경될 Chunk, areaList : 변경 정보, isBuildPlanet : Planet 생성 중인가?
    */
    private void ModifyBlock(Chunk chunk, List<ModifyArea> areaList, bool isBuildPlanet)
    {
        if (areaList == null)
            return;

        ModifyArea area = null;

        for (int i = 0; i < areaList.Count; i++) {
            area = areaList[i];

            if (!area.isUpdate && !isBuildPlanet)
                continue;

            area.isUpdate = false;

            //변경 영역 구성 오브젝트 후 활성화
            GameObject areaObj = planetInfo.modifyAreaObject[area.assetPath];
            if(areaObj == null)
                areaObj = Instantiate(Resources.Load(area.assetPath), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;


            areaObj.transform.position = area.GetPosition();
            areaObj.transform.localScale = area.GetScale();
            areaObj.transform.rotation = area.GetRotation();

            areaObj.SetActive(true);

            
            var chunkSize = chunk.chunkSize;
            Vector3 blockHalfExtent = Vector3.one * 0.5f;
            int layerMask = (1 << LayerMask.NameToLayer("CustomBlock"));


            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        //변경 가능한 블럭인지 체크
                        var block = chunk.GetBlock(x, y, z);
                        if (block.isLock)
                            continue;

                        //블럭의 위치 정보구성(worldPos + block의 center)
                        Vector3 blockCenter = transform.position + chunk.pos + new Vector3(x, y, z) + blockHalfExtent;

                        //구형에서 수정된 정보면 구형 좌표에 맞춰 center 좌표 변경
                        if (area.isSphere)
                        {
                            float len = Mathf.Max(Mathf.Abs(blockCenter.x), Mathf.Abs(blockCenter.y), Mathf.Abs(blockCenter.z));
                            blockCenter = blockCenter.normalized * len;

                        }
                        
                        //변환 영역에 포함된 블럭인지 체크
                        if (!Physics.CheckBox(blockCenter, blockHalfExtent, Quaternion.identity, layerMask))
                            continue;

                        //block정보 재구성
                        Block newBlock = area.BuildBlock(new Vector3Int(chunk.pos.x + x, chunk.pos.y + y, chunk.pos.z + z));

                        //재구성 block이 waterBlock인 경우 공기 블럭인 경우만 변경 가능
                        if (newBlock.GetType() == typeof(BlockWater) && block.IsSolid() != -1)
                            continue;

                        chunk.SetBlock(x, y, z, newBlock);
                    }
                }
            }

            //행성 생성중이면 area obj 제거
            if (isBuildPlanet)
                DestroyImmediate(areaObj);
        }

    }
   
    //PlanetInfo 정보를 바탕으로 ChunkBlock Build
    public abstract void BuildBlocks(Chunk chunk);
}



