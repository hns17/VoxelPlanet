/**
    @file   Save.cs
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
*/
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    @class  SaveChunk
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  수정된 Block 정보를 담아둔다.
*/
[Serializable]
public class SaveChunk
{
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();

    /**
         @brief  생성자, 수정된 Block 정보를 Dictionary에 기록
    */
    public SaveChunk(Chunk chunk)
    {
        int chunkSize = chunk.chunkSize;

        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                for (int z = 0; z < chunkSize; z++) {
                    if (!chunk.blocks[x, y, z].changed)
                        continue;

                    blocks.Add(new WorldPos(x,y,z), chunk.blocks[x, y, z]);
                }
            }
        }
    }
}

/**
    @class  SavePlanet
    @date   2018.08.08
    @author 황준성(hns17.tistory.com)
    @brief  Planet의 생성 정보를 저장한다.
*/
[Serializable]
public class SavePlanet
{
    public string name;
    public bool isSphere;

    public TerrainInfo terrainInfo = null;
    public CloudInfo cloudsInfo = null;
    

    public Dictionary<WorldPos, List<ModifyArea>> modifyList;
    public List<string> areaObjPath;

    /**
        @brief  생성자, Planet의 Terrain, Clouds 생성정보를 기록
        @param  info : planet info, tfTerrain : 지형 오브젝트, tfClouds : 구름 오브젝트
    */
    public SavePlanet(VoxelPlanetInfo info, Transform tfTerrain, Transform tfClouds)
    {
        if (info == null)
            return;

        this.name = info.name;
        this.isSphere = info.isSphere;

        if(tfTerrain != null)
            this.terrainInfo = info.terrainInfo;

        if (tfClouds != null)
            this.cloudsInfo = info.cloudInfo;

        modifyList = info.modifyList;

        if(info.modifyAreaObject.Count != 0)
        {
            areaObjPath = new List<string>();
            foreach (KeyValuePair<string, GameObject> pair in info.modifyAreaObject)
                areaObjPath.Add(pair.Key);
        }
    }
}