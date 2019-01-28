/**
    @file   VoxelPlanetMapControl.cs
    @date   2019.01.15
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/**
    @struct ObjectLocateInfo
    @date   2019.01.15
    @author 황준성(hns17.tistory.com)
    @brief  miniMap과 worldMap에 표시될 Object 정보
*/
[System.Serializable]
public struct ObjectLocateInfo
{
    public string      name;

    public Sprite      icon;
    public Transform   target;

    public bool        isDynamic;
}

/**
    @class  VoxelPlanetMapControl
    @date   2019.01.15
    @author 황준성(hns17.tistory.com)
    @brief  WorldMap & MiniMap Event
*/
public class VoxelPlanetMapControl : MonoBehaviour {
    [SerializeField] GameObject miniMap;    //미니맵
    [SerializeField] GameObject worldMap;   //월드맵

    //미니맵과 월드맵 거리
    [SerializeField] private float miniMapRange = 10f;
    [SerializeField] private float worldMapRange = 10f;

    //맵에 표시될 오브젝트 정보
    [SerializeField] private List<ObjectLocateInfo> locateInfos;

    private Transform player;

    //월드맵 활성화시 이벤트
    private IEnumerator worldMapControl;

    //맵에 표시될 오브젝트중 움직이는 오브젝트 리스트
    private List<KeyValuePair<Transform, Transform>> dynamicIcons;

    /**
        @brief  표시할 지역 정보 반환 
    */
    public List<ObjectLocateInfo> LocateInfos {
        get { return locateInfos; }
    }

    /**
        @brief 월드맵 Range 정보 반환
    */
    public float WorldMapRange
    {
        get { return worldMapRange; }
    }

    /**
        @brief 월드맵 transform 정보 반환
    */
    public Transform WorldMap
    {
        get { return worldMap.transform; }
    }

    private void Awake()
    {
        //Minitmap에 표시될 ui 생성
        if (locateInfos != null)
            MakeMapIcon();
    }

    private void Start()
    {
        player = PlayerInfo.Instance.transform;
        worldMapControl = WorldMapControl();
    }


    /**
        @brief miniMap에 표시할 Icon UI 생성
    */
    private void MakeMapIcon()
    {
        
        var parentIcon = transform.Find("MapIcons");

        foreach(var info in locateInfos) {
            var target = info.target;

            if (target == null)
                continue;

            //icon ui prefab load
            var iconObj = Resources.Load("Prefabs/UI/Minimap_Icon") as GameObject;

            //transform 정보 구성
            var pos = target.position + target.up;
            var rot = Quaternion.LookRotation(target.up);

            //객체 생성
            iconObj = Instantiate(iconObj, pos, rot);
            iconObj.transform.parent = parentIcon;

            //ui image 설정
            iconObj.GetComponentInChildren<Image>().sprite = info.icon;

            //움직이는 객체인 경우 따로 관리한다.
            if (info.isDynamic) {
                if (dynamicIcons == null)
                    dynamicIcons = new List<KeyValuePair<Transform, Transform>>();
                dynamicIcons.Add(new KeyValuePair<Transform, Transform>(info.target, iconObj.transform));
            }
        }
    }

    private void Update()
    {
        //월드맵 모드로 전환
        if (Input.GetKeyDown(KeyCode.M))
            SetActiveWorldMap(!worldMap.activeSelf);
        //miniMap을 끈다.
        else if (Input.GetKeyDown(KeyCode.N))
            SetActiveMiniMap(false);

        //미니맵이 활성화되어 있는 경우
        //플레이어와 카메라 사이를 오브젝트가 가리면 미니맵 줌인
        if (miniMap.activeSelf) {
            var range = miniMapRange;

            RaycastHit hit;
            Ray ray = new Ray(player.position + player.up * 2.5f, player.up);

            if (Physics.SphereCast(ray, 1f, out hit, miniMapRange))
                range = hit.distance+1;


            UpdateCameraRange(miniMap.transform, range);
            UpdateDynamicIcon();
        }
    }

    /**
        @brief  miniMap에 표시될 아이콘이 움직이는 객체면 위치 갱신
    */
    private void UpdateDynamicIcon()
    {
        if (dynamicIcons == null)
            return;

        foreach(var icon in dynamicIcons) {
            icon.Value.position = icon.Key.position + icon.Key.up;
            icon.Value.LookAt(icon.Key.up);
        }
    }


    /**
        @brief  월드맵 On / Off
        @param  isActive : On / Off Condition
    */
    public void SetActiveWorldMap(bool isActive)
    {
        //활성화
        if (isActive) {
            MyUtil.EnableCursor();
            SetActiveMiniMap(false);
            PlayerInfo.Instance.IsLock = true;

            UpdateCameraRange(worldMap.transform, worldMapRange);
            StartCoroutine(worldMapControl);
        }
        //비활성화
        else {
            MyUtil.DisableCursor();
            PlayerInfo.Instance.IsLock = false;
            SetActiveMiniMap(true);

            StopCoroutine(worldMapControl);
        }
        worldMap.SetActive(isActive);
    }


    /**
        @brief  MiniMap On / Off
        @param  isActive : minimap on / off Condition
    */
    public void SetActiveMiniMap(bool isActive)
    {
        miniMap.SetActive(isActive);
    }


    /**
        @brief  Player와 카메라 거리 조절
        @param  target : 조절할 카메라, range : 거리
    */
    private void UpdateCameraRange(Transform target, float range)
    {
        target.position = player.position + player.up * range;
        target.LookAt(player.position);
    }


    /**
        @brief  마우스 입력을 통해 WorldMap을 회전한다.
    */
    private IEnumerator WorldMapControl()
    {
        var tfWorldMap = worldMap.transform;

        while (true) {
            yield return null;

            if (Input.GetKey(KeyCode.Mouse0)) {
                var axis = tfWorldMap.up * Input.GetAxis("Mouse X");
                axis -= tfWorldMap.right * Input.GetAxis("Mouse Y");
                tfWorldMap.RotateAround(Vector3.zero, axis, 1);
            }
                
        }
    }
}
