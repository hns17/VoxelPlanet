/**
    @file   ItemInfo.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
*/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/**
    @class  ItemInfo
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @class  Scriptable Object로 Item 정보 관리
*/
public class ItemInfo : ScriptableObject
{
    //저장 위치
    static string path = "Assets/Editor/Item/ItemList.asset";

    //작업한 아이템 정보 리스트
    [SerializeField] List<Item> items = null;

    private static ItemInfo instance = null;
    /**
        @brief  SingleTone Class
    */
    public static ItemInfo Instance {
        get {
            //기존의 Item정보를 가져온다.
            if (instance == null)
                instance = LoadItem();

            //없으면 만든다.
            if (instance == null) {
                instance = CreateInstance<ItemInfo>();

                AssetDatabase.CreateAsset(instance, path);
                AssetDatabase.ImportAsset(path);
                instance.hideFlags = HideFlags.HideAndDontSave;
            }

            return instance;
        }
    }

    /**
        @brief  기존 정보 로드
    */
    public static ItemInfo LoadItem()
    {
        return AssetDatabase.LoadAssetAtPath<ItemInfo>(path);

    }

    /**
        @brief  Item 추가
        @param item : 추가할 item
    */
    public void AddItem(Item item)
    {
        if (items == null)
            items = new List<Item>();

        //리스트에 추가
        items.Add(item);

        //Asset 갱신
        hideFlags = HideFlags.None;
        item.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(item, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);
        hideFlags = HideFlags.HideAndDontSave;
        
    }

    /**
        @brief 지정된 type의 index에 위치한 item 반환
        @param type : item type, index : index
    */
    public Item GetItem(ItemType type, int index)
    {
        if (items == null || index > items.Count)
            return null;

        int cnt = 0;
        
        for(int i=0; i<items.Count; i++) {
            var item = items[i];

            if (item.type != type)
                continue;

            if (cnt == index)
                return item;

            cnt++;

        }
        return null;
    }
    

    /**
        @brief  Item 정보 제거
        @param  item : 제거할 item
    */
    public void Remove(Item item)
    {
        //제거 후 asset 갱신
        items.Remove(item);
        DestroyImmediate(item, true);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);
    }
}