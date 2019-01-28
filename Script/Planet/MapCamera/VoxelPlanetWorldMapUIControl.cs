/**
    @file   VoxelPlanetWorldMapUIControl.cs
    @date   2019.01.15
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/**
    @class  VoxelPlanetWorldMapUIControl
    @date   2019.01.15
    @author 황준성(hns17.tistory.com)
    @brief  WorldMap에 사용되는 UI 이벤트
*/
public class VoxelPlanetWorldMapUIControl : MonoBehaviour {
    //지정된 대상과 카메라 거리
    private float cutInRange = 10f;
    private float cutOutRange;
    private float currentCutRange;

    //리스트에 표시되는 텍스트
    private Text[] slots;

    //리스트의 현재 페이지
    private Text listIdxText;

    //현재 페이지
    private int currentListIdx;
    //최근 선택한 리스트 아이템
    private int currentLocateIdx;

    //월드맵에 표시될 지역 정보
    private List<ObjectLocateInfo> locates;

    //맵 컨트롤 컴포넌트
    private VoxelPlanetMapControl control;

    private void Awake () {
        listIdxText = transform.Find("ListIdx").GetComponent<Text>();

        slots = transform.Find("Slots").GetComponentsInChildren<Text>();
        control = transform.root.GetComponent<VoxelPlanetMapControl>();
        locates = control.LocateInfos;
        cutOutRange = control.WorldMapRange;
        
    }

    private void OnEnable()
    {
        currentListIdx = 0;
        currentLocateIdx = 0;
        currentCutRange = cutOutRange;

        UpdateSlots();
    }

    /**
        @brief  List Slot 정보 업데이트
    */
    private void UpdateSlots()
    {
        //페이지 텍스트
        listIdxText.text = currentListIdx + 1 + " / " + Mathf.Ceil((float)locates.Count / 3);

        //Locate Text
        var idx = currentListIdx * slots.Length;
        for (int i = 0; i < slots.Length; i++) {
            if (idx + i >= locates.Count)
                slots[i].text = "";
            else
                slots[i].text = locates[idx + i].name;
        }
    }

    /**
        @brief  next page
    */
    public void RightButton() {
        if (currentListIdx + 1 >= Mathf.Ceil((float)locates.Count / 3))
            return;

        currentListIdx++;

        UpdateSlots();
    }

    /**
        @brief  prev page
    */
    public void LeftButton() {
        if (currentListIdx - 1 < 0)
            return;

        currentListIdx--;

        UpdateSlots();
    }

    /**
        @brief  지역을 선택하면 해당 지역으로 카메라를 이동한다.
    */
    public void SelectSlot()
    {
        var selectSlot = EventSystem.current.currentSelectedGameObject;

        var idx = slots.Length * currentListIdx;

        //선택한 Slot 위치 가져오기
        for (int i=0; i<slots.Length; i++) {
            if (selectSlot.Equals(slots[i].transform.parent.gameObject)) {
                idx += i;
                break;
            }
        }

        //월드맵 카메라 이동
        if (idx < locates.Count) {
            currentLocateIdx = idx;
            StartCoroutine(SetWorldMapPosition(control.WorldMap, locates[idx].target));
        }
        
    }

    /**
        @brief  WorldMap 카메라를 Target으로 이동
        @param  origin : 초기 위치, target : 목적지
    */
    private IEnumerator SetWorldMapPosition(Transform origin, Transform target)
    {
        float t = 0;
        var originPos = origin.position;
        var pos = target.position + target.up * currentCutRange;

        //Lerp Move
        while (t < 1) {
            yield return null;

            t += Time.deltaTime;

            origin.position = Vector3.Lerp(originPos, pos, t);
            origin.LookAt(target.position);
        }
    }


    /**
        @brief WorldMap 카메라를 ZoomIn
    */
    public void CutIn()
    {
        currentCutRange = cutInRange;

        var worldMap = control.WorldMap;
        var dest = locates[currentLocateIdx].target.transform;
        var pos = dest.position + dest.up * cutInRange;
        iTween.MoveTo(worldMap.gameObject, iTween.Hash("time", 1, "position", pos, "islocal", true));
    }

    /**
        @brief WorldMap 카메라를 ZoomOut
    */
    public void CutOut()
    {
        currentCutRange = cutOutRange;

        var worldMap = control.WorldMap;
        var dest = locates[currentLocateIdx].target.transform;
        var pos = dest.position + dest.up * cutOutRange;
        iTween.MoveTo(worldMap.gameObject, iTween.Hash("time", 1, "position", pos, "islocal", true));
    }
}
