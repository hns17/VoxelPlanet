/**
    @file   VoxelPlanetGenerator.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

/**
    @class  VoxelPlanetGenerator
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Voxel Planet Object 생성을 위한 class 
*/
public class VoxelPlanetGenerator : ScriptableObject {
    static VoxelPlanetGenerator instance = null;

    /**
        @brief  Instance 반환, 없는 경우 생성 후 반환
    */
    public static VoxelPlanetGenerator GetPlanetGenerator()
    {
        if(instance == null)
            instance = CreateInstance<VoxelPlanetGenerator>();

        return instance;
    }

    /**
        @brief  Planet의 구성 Object를 초기화한다.
    */
    private GameObject InitPlanetObject(GameObject voxelPlanet, string objName)
    {
        var tf = voxelPlanet.transform.Find(objName);

        if (tf != null)
            GameObject.DestroyImmediate(tf.gameObject);


        GameObject obj = new GameObject(objName);
        obj.transform.parent = voxelPlanet.transform;
       
        return obj;
    }

    

    /**
        @function   CreateVoxelPlanet(Object Name)
        @brief      Planet Obj를 생성 
    */
    private GameObject CreateVoxelPlanet(string planetName)
    {
        var voxelPlanet = new GameObject(planetName);
        voxelPlanet.AddComponent<VoxelPlanetInfo>();
        voxelPlanet.tag = "VoxelPlanet";


        var installList = new GameObject("InstallObjList");
        installList.AddComponent<ProceduralObjectGenerator>();
        installList.transform.parent = voxelPlanet.transform;

        return voxelPlanet;
    }

    /**
        @function   InitVoxelPlanet(초기화 할 이름)
        @brief      행성을 초기화한다.
     */
    public GameObject InitVoxelPlanet(string planetName)
    {
        var voxelPlanet = VoxelPlanetInfo.GetVoxelPlanet();
        
        
        if (voxelPlanet == null)
            voxelPlanet = CreateVoxelPlanet(planetName);
        else
            voxelPlanet.name = planetName;
     

        return voxelPlanet;
    }


    /**
        @function   GenerateTerrain(행성 이름, 지형 구성정보, 구형 생성 Flag)
        @brief      지형을 생성한다.
    */
    public void GenerateTerrain(string planetName, TerrainInfo info, bool isSphere)
    {
        if (info == null)
            return;

        //행성 정보 재구성
        var voxelPlanet = InitVoxelPlanet(planetName);
        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();
        planetInfo.Init(planetName);
        planetInfo.isSphere = isSphere;

        //지형 생성 정보 재구성
        planetInfo.terrainInfo = info.DeepCopy();

        //지형 오브젝트 초기화
        GameObject objTerrain = InitPlanetObject(voxelPlanet, "Terrain");
        var compTerrain = objTerrain.AddComponent<VoxelTerrain>();
        compTerrain.planetInfo = planetInfo;

        //청크 생성
        EditorCoroutineRunner.StartEditorCoroutine(compTerrain.BuildChunk());

        SaveCurrentPlanetInfo(voxelPlanet);
    }

    /**
        @function   GenerateTerrain(PlanetObject)
        @brief      PlanetObject의 정보를 바탕으로 지형을 생성한다.
    */
    public void GenerateTerrain(GameObject voxelPlanet)
    {
        if (voxelPlanet == null)
            return;
        
        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();
        
        //지형 오브젝트 초기화
        GameObject objTerrain = InitPlanetObject(voxelPlanet, "Terrain");
        var compTerrain = objTerrain.AddComponent<VoxelTerrain>();
        compTerrain.planetInfo = planetInfo;

        //청크 생성
        EditorCoroutineRunner.StartEditorCoroutine(compTerrain.BuildChunk());
        SaveCurrentPlanetInfo(voxelPlanet);
    }


    /**
        @function   GenerateTerrain(행성 이름, 구름 구성정보, 구형 생성 Flag)
        @brief      구름을 생성한다.
     */
    public void GenerateClouds(string planetName, CloudInfo info, bool isSphere)
    {
        if (info == null)
            return;

        //행성 정보 재구성
        var voxelPlanet = InitVoxelPlanet(planetName);
        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();
        planetInfo.Init(planetName);
        planetInfo.isSphere = isSphere;

        //구름 생성 정보 재구성
        planetInfo.cloudInfo = info.DeepCopy();
        
        //구름 오브젝트 초기화
        GameObject objClouds = InitPlanetObject(voxelPlanet, "Clouds");
        var compClouds = objClouds.AddComponent<VoxelClouds>();
        compClouds.planetInfo = planetInfo;


        //청크 생성
        EditorCoroutineRunner.StartEditorCoroutine(compClouds.BuildChunk());
        SaveCurrentPlanetInfo(voxelPlanet);
    }

    /**
        @function   GenerateClouds(PlanetObject)
        @brief      PlanetObject의 정보를 바탕으로 구름을 생성한다.
    */
    public void GenerateClouds(GameObject voxelPlanet)
    {
        if (voxelPlanet == null)
            return;

        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();


        //구름 오브젝트 초기화
        GameObject objClouds = InitPlanetObject(voxelPlanet, "Clouds");
        var compClouds = objClouds.AddComponent<VoxelClouds>();
        compClouds.planetInfo = planetInfo;


        //청크 생성
        EditorCoroutineRunner.StartEditorCoroutine(compClouds.BuildChunk());
        SaveCurrentPlanetInfo(voxelPlanet);
    }

    /**
       @brief  Planet Object 구면화
    */
    public void ConvertToPlanet()
    {
        var voxelPlanet = VoxelPlanetInfo.GetVoxelPlanet();

        if (voxelPlanet == null)
            return;

        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();
        planetInfo.isSphere = !planetInfo.isSphere;

        if (voxelPlanet.transform.Find("Terrain"))
            GenerateTerrain(voxelPlanet);
        if (voxelPlanet.transform.Find("Clouds"))
            GenerateClouds(voxelPlanet);

        ConvertToObject(planetInfo.isSphere);
    }
    
    /**
        @brief      Object를 Sphere된 Planet에 맞춰 회전시킨다.
       
        @warning    블럭보다 범위가 큰 물체(주로 건물)의 경우 외곽이 공중에 뜨게된다.
                    지형이 라운드화 되다보니 오브젝트 자체를 지형에 맞춰 라운드화 하는것 말고는 해결책이 없어보인다.
                    문제는 오브젝트 정점 위치가 지형과 달리 균등한 범위로 구성되어 있지 않다.
                    작은 단위로 Sampling하면 변환은 가능하겠지만 많은 오브젝트를 변환하는 것은 어려워 보인다.
                    오브젝트가 조금 묻히는 걸 감안하고 설치하면 나쁘지 않아보이지만, 
                    크기가 클수록 그리고 행성의 내부로 갈수록 곡면의 정도가 강해 그대로 설치하기는 어렵다.
     */
    void ConvertToObject(bool isSphere)
    {
        
        var objs = GameObject.FindGameObjectsWithTag("InstallObject");
        int cnt = objs.Length;

        for(int i=0; i<cnt; i++)
        {
            var tfObject = objs[i].transform;
            var installPos = tfObject.position;


            float len = Mathf.Max(Mathf.Abs(installPos.x), Mathf.Abs(installPos.y), Mathf.Abs(installPos.z));

            if (!isSphere)
            {
                //거리비 구하기
                var blockLen = Vector3.Magnitude(installPos);
                var element = Mathf.Max(Mathf.Abs(installPos.x), Mathf.Abs(installPos.y), Mathf.Abs(installPos.z));
                //거리비 구하기
                len = blockLen * (blockLen / element);
            }

            installPos = installPos.normalized * len;

            tfObject.rotation = Quaternion.FromToRotation(tfObject.up, installPos.normalized);
            tfObject.position = installPos;
        }
        
    }



    /**
        @function   ModifyArea(변경 정보)
        @brief      지정된 영역의 블럭 정보를 변경한다.
    */
    public void ModifyArea(ModifyArea areaInfo)
    {
        var voxelPlanet = VoxelPlanetInfo.GetVoxelPlanet();
        if (voxelPlanet == null)
            return;

        ModifyChunk(voxelPlanet.transform.Find("Terrain"), areaInfo);
        ModifyChunk(voxelPlanet.transform.Find("Clouds"), areaInfo);
    }


    /**
        @brief  Block 정보를 변경한다.
                1. CustomObject와 청크 충돌 검사
                2. 충돌된 청크를 대상으로 Modify 정보 구성 함수 호출
                3. 구성된 정보를 바탕으로 Block 정보를 변경할 함수 호출
                ** CustomObject는 Layer가 CustomBlock인 오브젝트로 Prefab/CustomArea 내에 위치해야 한다.
    */
    void ModifyChunk(Transform tf, ModifyArea areaInfo)
    {
        if (tf == null)
            return;
        
        VoxelPlanet planet = tf.GetComponent<VoxelPlanet>();
        int layerMask = (1 << LayerMask.NameToLayer("CustomBlock"));
        
        
        for (int i = 0; i < tf.childCount; i++) {
            //Custom Area에 포함된 청크 체크를 위해 청크 범위 정보를 구성
            Chunk chunk = tf.GetChild(i).GetComponent<Chunk>();
            //int chunkSize = chunk.chunkSize;
            float radius = chunk.chunkSize * 0.5f;

            Vector3 halfExtent = new Vector3(radius, radius, radius);
            
            //(tf.position+ chunk.pos) : world 좌표
            //world좌표 + halfExtent : chunk의 pos 정보는 -x,-y,-z 위치이기 때문에 center로 맞춰준다.
            Vector3 center = tf.position + chunk.pos + halfExtent;

            Quaternion checkQuat = Quaternion.identity;

            //Sphere화 된 경우
            if (chunk.isSphere) {
                var prevCenter = center;

                //center 정보를 Sphere 좌표로 변환
                float len = Mathf.Max(Mathf.Abs(center.x), Mathf.Abs(center.y), Mathf.Abs(center.z));
                center = center.normalized * len;

                //chunk의 회전 정보
                checkQuat= Quaternion.FromToRotation(prevCenter, center);
            }


            //Area에 포함된 Chunk 검사
            var colList = Physics.OverlapBox(center, halfExtent, checkQuat, layerMask);
            if (colList.Length == 0)
                continue;

            // area 정보 구성
            string objPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(colList[0]));
            planet.SetModifyArea(chunk.pos, areaInfo, colList[0], objPath);

            //Block 정보 수정
            planet.UpdateModifyBlockArea(chunk, false);
        }
        SaveCurrentPlanetInfo(tf.parent.gameObject);
        Debug.Log("Finish Modify");
    }
    

   

    /**
        @brief  Planet 정보 저장 
    */
    public void SavePlanet()
    {
        var planet = VoxelPlanetInfo.GetVoxelPlanet();

        if (planet == null)
            return;

        //공용 다이얼로그 호출
        string path = EditorUtility.SaveFilePanel("Save Planet as planet",
            "Assets/MapData", planet.name, "planet");

        if (path.Length == 0)
            return;
        
        if (planet != null)
            Serialization.SaveVoxelPlanet(planet, path);
    }

    /**
        @brief  저장된 Planet 정보 불러오기
    */
    public void LoadPlanet()
    {
        string path = EditorUtility.OpenFilePanel("Load with planet", "Assets/MapData", "planet");
        if (path.Length == 0)
            return;


        var loadData = Serialization.LoadPlanet(path);

        //불러온 정보를 바탕으로 Planet Object 구성
        var voxelPlanet = InitVoxelPlanet(loadData.name);
        var planetInfo = voxelPlanet.GetComponent<VoxelPlanetInfo>();

        //Planet Info 구성
        planetInfo.ReLoadInfo(loadData);
        

        //지형 생성
        if (loadData.terrainInfo != null)
            GenerateTerrain(voxelPlanet);

        //구름 생성
        if (loadData.cloudsInfo != null)
            GenerateClouds(voxelPlanet);
    }

    private void SaveCurrentPlanetInfo(GameObject planet)
    {
        var savePath = Serialization.GetCurrentPlanetInfoPath();
        Serialization.SaveVoxelPlanet(planet, savePath);
    }
}
