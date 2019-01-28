/**
    @file   InventoryEvent.cs
    @date   2018.08.15
    @author 황준성(hns17.tistory.com)
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
    @class  InventoryEvent
    @date   2018.08.15
    @author 황준성(hns17.tistory.com)
    @brief  Inventory System
*/
public class InventoryEvent : MonoSingleton<InventoryEvent> {
    private Dictionary<Transform, Image> categorys;

    //slot List
    private List<KeyValuePair<GameObject, SlotEvent>> slots;

    //item List
    //private List<Item> items;
    private Dictionary<Item, int> items;

    //show?
    private bool        isEnable    = false;   

    //inventory object
    private GameObject  inventory;

    //Category Btn Sprite
    private Sprite pressSprite;
    private Sprite popSprite;
    private int currentCategory;

    private void Start()
    {
        inventory = transform.Find("Inventory").gameObject;

        MakeCategoryList();
        MakeSlotList();
        inventory.SetActive(isEnable);

        //Category Btn Sprite
        pressSprite = SpriteInfo.Instance.GetSprite("btn_press");
        popSprite = SpriteInfo.Instance.GetSprite("btn_pop");
    }

    /**
        @brief  카테고리 리스트 구성
    */
    private void MakeCategoryList()
    {
        var tfCategoryGroup = inventory.transform.Find("Category");
        int cnt = tfCategoryGroup.childCount;

        categorys = new Dictionary<Transform, Image>();

        for (int i = 0; i < cnt; i++)
        {
            var category = tfCategoryGroup.GetChild(i);
            categorys.Add(category, category.GetComponent<Image>());
        }
    }

    /**
        @brief  Inventory Slot List 구성
    */
    private void MakeSlotList()
    {
        slots = new List<KeyValuePair<GameObject, SlotEvent>>();

        var tfSlots = inventory.transform.Find("Slots");

        for(int i=0; i<tfSlots.childCount; i++)
        {
            var slot = tfSlots.GetChild(i).GetChild(0).gameObject;
            var pair = new KeyValuePair<GameObject, SlotEvent>(slot, slot.GetComponent<SlotEvent>());
            slots.Add(pair);
        }
 
    }

    /**
        @brief  Inventory 위치 변경.
    */
    public void MoveInventory()
    {
        var x = Input.GetAxisRaw("Mouse X")*20;
        var y = Input.GetAxisRaw("Mouse Y")*20;

        inventory.transform.position += new Vector3(x, y, 0);
    }

    /**
        @brief  Inventory Category 단위 아이템 표시
        @btn    Category 버튼
    */
    public void CategoryClick(Transform btn)
    {
        int categoryIdx = 0;
        foreach(var category in categorys)
        {
            if (category.Key.Equals(btn)) {
                categorys[btn].sprite = pressSprite;
                currentCategory = categoryIdx;
            }
            else {
                category.Value.sprite = popSprite;
            }
            categoryIdx++;
        }
        InventoryUpdate(currentCategory);
    }

    /**
        @brief  해당 카테고리에 속한 아이템 표시
        @param  category : category index
    */
    public void InventoryUpdate(int category)
    {
        //카테고리 타입 체크
        var itemType = ItemType.DOCUMENT | ItemType.BOMB | ItemType.KEY 
                        | ItemType.EAT | ItemType.CRAFT | ItemType.EXCHANGE;

        if (category == 1)
            itemType = ItemType.EAT | ItemType.BOMB;
        else if (category == 2)
            itemType = ItemType.CRAFT;
        else if (category == 3)
            itemType = ItemType.EXCHANGE;
        else if (category == 4)
            itemType = ItemType.KEY | ItemType.DOCUMENT;


        //슬롯 초기화
        for (int i = 0; i < slots.Count; i++)
            slots[i].Key.SetActive(false);

        if (items == null)
            return;

        //타입에 맞는 아이템 슬롯에 추가.
        int slotIdx = 0;

        foreach(var value in items) {
            var item = value.Key;
            if (item.type != (itemType & item.type))
                continue;

            slots[slotIdx].Value.SetItem(item, value.Value);
            slotIdx++;
        }
    }

    /**
        @brief  인벤토리 활성화 상태 체크
    */
    public bool IsEnable()
    {
        return isEnable;
    }

    /**
        @brief  Inventory On/Off
    */
    public bool TurnInventory()
    {
        isEnable = !isEnable;

        inventory.SetActive(isEnable);

        if (isEnable) {
            MyUtil.EnableCursor();
            InventoryUpdate(0);
        }
        else {
            MyUtil.DisableCursor();
        }

        return isEnable;
    }

    /**
        @brief  Inventory Slot에 Item 추가
        @param  item : item info
    */
    public void AddItem(Item item)
    {
        if (item == null)
            return;

        if (items == null)
            items = new Dictionary<Item, int>();

        if (items.ContainsKey(item))
            items[item]++;
        else
            items[item] = 1;
        

        //슬롯에 추가 후 업데이트
        InventoryUpdate(currentCategory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            TurnInventory();
    }
}
