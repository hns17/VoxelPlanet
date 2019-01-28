/**
    @class  VoxelPlanetEditor.cs 
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
*/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


/**
    @class  VoxelPlanetEditor 
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  Planet Edior WIndow의 구성
*/
public class VoxelPlanetEditor : EditorWindow {
    //에디터의 스크롤바 위치
    private Vector2                 scrollPos;

    //생성자 객체
    private VoxelPlanetGenerator    genPlanet = null;
    public List<GameObject>         installObject;

    private string                  planetName = "VoxelPlanet";
    
    //Planet을 구형으로 만들것인가?
    private bool                    isSphere = false;

    //지형 및 구름 생성에 사용될 정보를 담아둘 객체  
    private TerrainInfo             genTerrainInfo;
    private CloudInfo               genCloudInfo;
    
    
    //Custom Area 정보를 담아둘 객체
    private FillArea                fillArea;
    private NoiseArea               noiseArea;
    private ModifyFillMode fillmode = ModifyFillMode.NOISE;

    
    
    [MenuItem("Editor/Editor VoxelPlanet")]
    /**
       @brief  Voxel Planet Window 활성화시 초기화
    */
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(VoxelPlanetEditor));
        window.position = new Rect(780, 170, 470, 850);
        window.Show();
    }

    /**
       @Function    DrawUILine(색상, 높이, 굵기)
       @brief  HorizontalLine Draw
    */
    static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }


    /**
        @brief  Generator 객체 생성
    */
    private void OnEnable()
    {

        if(genPlanet == null)
            genPlanet =VoxelPlanetGenerator.GetPlanetGenerator();

        if (genTerrainInfo == null)
            genTerrainInfo = new TerrainInfo();
        if (genCloudInfo == null)
            genCloudInfo = new CloudInfo();

        if (fillArea == null)
            fillArea = new FillArea();
        if (noiseArea == null)
            noiseArea = new NoiseArea();
    }

    /**
        @brief  Terrain 생성 정보 구성
    */
    private void TerrainPropertie()
    {
        GUILayout.Label("[Terrain Properties]", EditorStyles.boldLabel);
        GUILayout.Label("[Basic Info]", EditorStyles.miniBoldLabel);
        genTerrainInfo.radius = EditorGUILayout.IntSlider("TerrainRadius", genTerrainInfo.radius, 16, 128);
        genTerrainInfo.chunkSize = EditorGUILayout.IntSlider("ChunkSize", genTerrainInfo.chunkSize, 8, 32);

        GUILayout.Space(2);
        GUILayout.Label("[Terrain Area Rate]", EditorStyles.miniBoldLabel);
        genTerrainInfo.coreRate = EditorGUILayout.Slider("CoreRate", genTerrainInfo.coreRate, 0.1f, 0.7f);
        genTerrainInfo.blockRate = EditorGUILayout.Slider("BlockRate", genTerrainInfo.blockRate, 0.1f, 1f);
        genTerrainInfo.desertRate = EditorGUILayout.Slider("DesertRate", genTerrainInfo.desertRate, 0.0f, 1f);
        genTerrainInfo.grassRate = EditorGUILayout.Slider("GrassRate", genTerrainInfo.grassRate, 0.0f, 1f);


        GUILayout.Space(2);
        GUILayout.Label("[Terrain Noise Paramiter]", EditorStyles.miniBoldLabel);
        genTerrainInfo.octave = EditorGUILayout.IntSlider("Octave", genTerrainInfo.octave, 1, 7);
        genTerrainInfo.frequency = EditorGUILayout.Slider("Frequency", genTerrainInfo.frequency, 0.005f, 0.1f);
        genTerrainInfo.persistence = EditorGUILayout.Slider("Persistence", genTerrainInfo.persistence, 0.05f, 0.3f);

        GUILayout.Space(2);
        GUILayout.Label("[Ocean Paramiter]", EditorStyles.miniBoldLabel);
        genTerrainInfo.oceanHeight = EditorGUILayout.IntSlider("Ocean Height", genTerrainInfo.oceanHeight, genTerrainInfo.radius / 2, genTerrainInfo.radius);
        genTerrainInfo.oceanDepth = EditorGUILayout.IntSlider("Ocean Depth", genTerrainInfo.oceanDepth, 1, 5);

        //Terrain 생성 함수 호출
        if (GUILayout.Button("Generate Terrain"))
        {
            if (planetName.Equals(""))
                planetName = "VoxelPlanet";
            genPlanet.GenerateTerrain(planetName, genTerrainInfo, isSphere);
        }

        DrawUILine(Color.gray);
    }

    /**
        @brief  Cloud 생성 정보 구성
    */
    private void CloudProperite()
    {
        //Clouds Param Control
        GUILayout.Label("[Cloud Properties]", EditorStyles.boldLabel);

        GUILayout.Label("[Cloud Info]", EditorStyles.miniBoldLabel);
        genCloudInfo.chunkSize = EditorGUILayout.IntSlider("ChunkSize", genCloudInfo.chunkSize, 8, 32);
        genCloudInfo.groundFromDist = EditorGUILayout.IntSlider("GroundFromDistance", genCloudInfo.groundFromDist, 
            genTerrainInfo.radius, genTerrainInfo.radius + 30);

        GUILayout.Label("[Cloud Area Density]", EditorStyles.miniBoldLabel);
        genCloudInfo.densityHeight = EditorGUILayout.Slider("Density Height", genCloudInfo.densityHeight, 0.1f, 1.0f);
        genCloudInfo.densityWidth = EditorGUILayout.Slider("Density Width", genCloudInfo.densityWidth, 0.1f, 0.5f);

        GUILayout.Space(2);
        GUILayout.Label("[Cloud Noise Paramiter]", EditorStyles.miniBoldLabel);
       genCloudInfo.octave = EditorGUILayout.IntSlider("Octave", genCloudInfo.octave, 1, 7);
       genCloudInfo.frequency = EditorGUILayout.Slider("Frequency", genCloudInfo.frequency, 0.005f, 0.05f);
       genCloudInfo.persistence = EditorGUILayout.Slider("Persistence", genCloudInfo.persistence, 0.05f, 0.3f);

        GUILayout.Space(6);
        //Clouds 생성 함수 호출
        if (GUILayout.Button("Generate Clouds"))
        {
            if (planetName.Equals(""))
                planetName = "VoxelPlanet";
            genPlanet.GenerateClouds(planetName, genCloudInfo, isSphere);
        }

        DrawUILine(Color.gray);
    }



    /**
        @brief  CustomArea 정보 구성
    */
    private void ModifyAreaPropertie()
    {
        //Modify Area 
        GUILayout.Label("[Modify Area Properties]", EditorStyles.boldLabel);
        fillmode = (ModifyFillMode)EditorGUILayout.EnumPopup("Modify Mode:", fillmode);


        ModifyArea area = null;
        //채우기 모드일 경우
        if (fillmode == ModifyFillMode.FILL)
        {
            fillArea.blockType = (FillArea.BlockType)EditorGUILayout.EnumPopup("BlockType:", fillArea.blockType);
            area = fillArea;
        }
        //Noise 모드 일 경우
        else
        {
           
            GUILayout.Space(2);
            GUILayout.Label("[Area Block Rate]", EditorStyles.miniBoldLabel);
            noiseArea.blockRate = EditorGUILayout.Slider("BlockRate", noiseArea.blockRate, 0.1f, 1f);
            noiseArea.desertRate = EditorGUILayout.Slider("DesertRate", noiseArea.desertRate, 0.0f, 1f);
            noiseArea.grassRate = EditorGUILayout.Slider("GrassRate", noiseArea.grassRate, 0.0f, 1f);
            noiseArea.cloudRate = EditorGUILayout.Slider("Cloud", noiseArea.cloudRate, 0.0f, 1f);


            GUILayout.Space(2);
            GUILayout.Label("[Area Noise Paramiter]", EditorStyles.miniBoldLabel);
            noiseArea.octave = EditorGUILayout.IntSlider("Octave", noiseArea.octave, 1, 7);
            noiseArea.frequency = EditorGUILayout.Slider("Frequency", noiseArea.frequency, 0.005f, 0.1f);
            noiseArea.persistence = EditorGUILayout.Slider("Persistence", noiseArea.persistence, 0.05f, 0.3f);
            area = noiseArea;
        }

        //Modify 함수 호출
        if (GUILayout.Button("Modity Block Area"))
            genPlanet.ModifyArea(area);

        DrawUILine(Color.gray);
    }


    /**
        @brief  Editor Window 구성 및 업데이트
    */
    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(455), GUILayout.Height(850));

        GUILayout.Label("[Planet Properties]", EditorStyles.boldLabel);
        planetName = EditorGUILayout.TextField("PlanetName : ", planetName);
        isSphere = EditorGUILayout.Toggle("Sphere Planet", isSphere);
        DrawUILine(Color.gray);
        

        TerrainPropertie();
        CloudProperite();
        ModifyAreaPropertie();
        



        GUILayout.Label("[Convert Planet Type]", EditorStyles.boldLabel);
        if (GUILayout.Button("Convert Cube <-> Sphere"))
            genPlanet.ConvertToPlanet();

        
        GUILayout.Space(6);
        GUILayout.Label("[Save & Load]", EditorStyles.boldLabel);
        //행성 데이터 저장
        if (GUILayout.Button("Save Planet"))
            genPlanet.SavePlanet();

        //행성 데이터 불러오기
        if (GUILayout.Button("Load Planet"))
            genPlanet.LoadPlanet();

        EditorGUILayout.EndScrollView();
    }
}



/**
    @class  CustomObjectEditor
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  CustomObject에 대한 Inspector UI를 수정
*/
[CustomEditor(typeof(ProceduralObjectGenerator))]
public class CustomObjectEditor : Editor
{
    public ProceduralObjectGenerator customObj;


    /**
        @brief  Target을 초기화한다.   
    */
    private void OnEnable()
    {
        customObj = target as ProceduralObjectGenerator;
    }

    /**
        @brief  CustomObject 관련된 UI 및 이벤트 구성   
    */
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(2);
        if (GUILayout.Button("Install Object"))
        {
            if (customObj != null)
                customObj.InstallObject();
        }

       
        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(customObj);
        //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        //}
    }
    
    
}
