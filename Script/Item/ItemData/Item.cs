/**
    @file   item.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  Item 정보 추상클래스
*/
using UnityEngine;


/**
    brief   ItemType, 종류 별로 인벤토리 카테고리에 분류
*/
[System.Flags] public enum ItemType {
    EAT         =   1,
    BOMB        =   2,
    CRAFT       =   4,
    KEY         =   8,
    EXCHANGE    =   16,
    DOCUMENT    =   32
}

/**
    @class  item
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  기본이 되는 아이템 클래스
*/
public abstract class Item : ScriptableObject
{
    //아이템 타입
    public ItemType type;       

    //아이템 이름 및 세부정보
    public string itemName;
    public string desc;

    //판매 가능 아이템인지 체크
    public bool isSell = false;

    //판매 가격
    public uint price = 0;
    
    //아이콘
    public Sprite icon = null;
    
    /**
        @brief  초기화 함수
        @param  name    : item 이름
                type    : item type
                desc    : item 상세 설명
                isSell  : 판매 가능한 아이템 인가
                icon    : item icon
                price   : 판매 가격
    */
    public void Init(string name, ItemType type, string desc, bool isSell, Sprite icon, uint price)
    {
        this.itemName = name;
        this.desc = desc;
        this.isSell = isSell;
        this.icon = icon;
        this.price = price;
        this.type = type;
    }


    public virtual void Update()
    {

    }
    
}