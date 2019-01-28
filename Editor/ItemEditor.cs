/**
    @file   ItemEditor.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
*/
using UnityEditor;
using UnityEngine;

/**
    @class  ItemEditor
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  ItemEdiot Window
*/
public class ItemEditor : EditorWindow
{
    private ItemInfo itemList = null;

    //Editor Mode
    private string[] editMode = new string[] { "Add", "Modify&Remove" };

    //Category
    private ItemType itemType = ItemType.EAT;

    //현재 Edit List Index
    private int editIndex = 0;
    //Edit List에 보여질 첫번째 Item Index
    private int startItemIndex = 0;

    //Item Info 정보
    private string itemName = "";
    private Sprite icon = null;

    private int price = 0;
    private int hp = 0;
    private int mp = 0;
    private int damage = 0;
    private int range = 0;
    private string desc = "";
    private Texture2D texture = null;

    //판매가능 한 아이템?
    private bool isSellGroup = false;

    //프리펩으로 생성 할 것인가?
    private bool isPrefab = false;
    

    [MenuItem("Editor/Editor ItemManager")]

    /**
        @brief  Window Init
    */
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ItemEditor));
        window.Show();
    }


    void OnGUI()
    {
        if (itemList == null)
            itemList = ItemInfo.Instance;

        //Edit Mode
        GUILayout.Label("[EditMode]", EditorStyles.boldLabel);
        editIndex = EditorGUILayout.Popup(selectedIndex: editIndex, displayedOptions: editMode);
        GUILayout.Space(10);

        //Item Category
        GUILayout.Label("[Item Type]", EditorStyles.boldLabel);
        itemType = (ItemType)EditorGUILayout.EnumPopup(itemType);
        GUILayout.Space(10);


        //Item 추가 모드면 정보 추가 창 표시
        if (editIndex == 0)
            BuildGUIInsertMode();
        //Item 수정 삭제 모드면 수정&삭제 창 표시
        else
            BuildGUIModifyAndRemoveMode();

    }

    /**
        @brief  Item 수정&삭제 창 구성 
    */
    void BuildGUIModifyAndRemoveMode()
    {
        GUILayout.Label("[ItemList]", EditorStyles.boldLabel);

        //아이템 리스트 표시
        for (int i = startItemIndex; i < startItemIndex + 3; i++) {
            var item = itemList.GetItem(itemType, i);

            if (item == null)
                break;

            ShowItemInfo(item, i - startItemIndex);
        }
       
        //이전 페이지 버튼 이벤트
        if (GUI.Button(new Rect(115, 460, 60, 20), "Prev")) {
            if (startItemIndex - 3 > 0)
                startItemIndex -= 3;
            else
                startItemIndex = 0;
        }

        //다음 페이지 아이템 이벤트
        if (GUI.Button(new Rect(215, 460, 60, 20), "Next"))
            startItemIndex += 3;
        
    }

    /**
        @brief  Item 정보와 수정, 삭제, Prefab기능
        @param  item : 표시할 아이템 정보, cnt : 아이템 정보가 표시될 위치
    */
    void ShowItemInfo(Item item, int cnt)
    {
        //item이 표시될 범위 구성(cnt에 따라 변화)
        GUILayoutOption[] options = new[] {
             GUILayout.Width (400),
             GUILayout.Height (100)
        };

        int offsetH = cnt * 110;

        GUILayout.BeginArea(new Rect(10, 130 + offsetH, 400, 100));
        GUILayout.Box("", options);
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(20, 135 + offsetH, 40, 40));
        options[0] = GUILayout.Width(40);
        options[1] = GUILayout.Height(40);
        item.icon = (Sprite)EditorGUILayout.ObjectField(item.icon, typeof(Sprite), true, options);
        GUILayout.EndArea();


        //아이템 기본 정보 표시
        GUI.Label(new Rect(70, 135 + offsetH, 40, 20), "Name", EditorStyles.boldLabel);
        item.itemName = GUI.TextField(new Rect(115, 135 + offsetH, 70, 15), item.itemName);

        GUI.Label(new Rect(195, 135 + offsetH, 40, 20), "IsSell", EditorStyles.boldLabel);
        item.isSell = GUI.Toggle(new Rect(235, 133 + offsetH, 10, 10), item.isSell, "");

        GUI.Label(new Rect(260, 135 + offsetH, 40, 20), "Price", EditorStyles.boldLabel);
        GUILayout.BeginArea(new Rect(300, 135 + offsetH, 60, 15));
        item.price = (uint)EditorGUILayout.IntField((int)item.price);
        GUILayout.EndArea();


        //아이템 카테고리별 추가 정보 표시
        //먹을 수 있는 아이템
        if (itemType == ItemType.EAT) {
            var eatItem = (ItemEat)item;
            GUI.Label(new Rect(70, 155 + offsetH, 25, 20), "HP", EditorStyles.boldLabel);
            GUILayout.BeginArea(new Rect(95, 155 + offsetH, 60, 15), "");
            eatItem.hp = EditorGUILayout.IntField(eatItem.hp);
            GUILayout.EndArea();

            GUI.Label(new Rect(170, 155 + offsetH, 25, 20), "MP", EditorStyles.boldLabel);
            GUILayout.BeginArea(new Rect(195, 155 + offsetH, 60, 15), "");
            eatItem.mp = EditorGUILayout.IntField(eatItem.mp);
            GUILayout.EndArea();
        }
        //폭탄
        else if (itemType == ItemType.BOMB) {
            var bombItem = (ItemBomb)item;
            GUI.Label(new Rect(70, 155 + offsetH, 60, 20), "Damage", EditorStyles.boldLabel);
            GUILayout.BeginArea(new Rect(130, 155 + offsetH, 60, 15), "");
            bombItem.damage = (uint)EditorGUILayout.IntField((int)bombItem.damage);
            GUILayout.EndArea();

            GUI.Label(new Rect(200, 155 + offsetH, 50, 20), "Range", EditorStyles.boldLabel);
            GUILayout.BeginArea(new Rect(250, 155 + offsetH, 60, 15), "");
            bombItem.range = (uint)EditorGUILayout.IntField((int)bombItem.range);
            GUILayout.EndArea();
        }

        //아이템 상세 설명 
        GUI.Label(new Rect(20, 180 + offsetH, 80, 20), "Description", EditorStyles.boldLabel);
        item.desc = GUI.TextArea(new Rect(20, 195 + offsetH, 300, 30), item.desc);

        //아이템 삭제
        if (GUI.Button(new Rect(325, 190 + offsetH, 80, 15), "Remove"))
            itemList.Remove(item);

        //프리팹 생성
        if (GUI.Button(new Rect(325, 210 + offsetH, 80, 15), "Gen Prefab"))
            CreateItemPrefab(item);
    }

    /**
        @brief  Item 추가 창 구성 
    */
    void BuildGUIInsertMode()
    {
        //Item 기본 입력 정보
        GUILayout.Label("[Item Info]", EditorStyles.boldLabel);
        itemName = EditorGUILayout.TextField("Name", itemName);
        icon = (Sprite)EditorGUILayout.ObjectField("Item Icon", icon, typeof(Sprite), true);
        GUILayout.Space(5);

        //Item 카테고리에 따른 추가 입력 정보
        //먹는 아이템
        if (itemType == ItemType.EAT) {
            hp = EditorGUILayout.IntField("HP", hp);
            mp = EditorGUILayout.IntField("MP", mp);
        }
        //폭탄
        else if (itemType == ItemType.BOMB) {
            damage = EditorGUILayout.IntField("Damage", damage);
            range = EditorGUILayout.IntField("Range", range);
        }
        //문서
        else if (itemType == ItemType.DOCUMENT) {
            texture = (Texture2D)EditorGUILayout.ObjectField("Item Icon", texture, typeof(Texture2D), true);
        }

        GUILayout.Space(5);

        //판매 가능 아이템?
        isSellGroup = EditorGUILayout.BeginToggleGroup("IsSell", isSellGroup);
        price = EditorGUILayout.IntField("Price", price);
        EditorGUILayout.EndToggleGroup();
        GUILayout.Space(5);

        //아이템 설명
        GUILayout.Label("Item Desc");
        desc = EditorGUILayout.TextArea(desc, GUILayout.Height(50));
        GUILayout.Space(5);

        //생성시 Prefab화 할것 인가?
        GUILayout.Label("[Create Prefab]", EditorStyles.boldLabel);
        isPrefab= EditorGUILayout.Toggle("IsCreatePrefab", isPrefab);

        //Item 추가
        GUILayout.Space(5);
        if (GUILayout.Button("Insert"))
            InsertItem();
    }

    /**
       @brief   Item 정보를 추가하고 Prefab화 한다.
    */
    void InsertItem()
    {
        Item item = null;

        //카테고리 별 Item 생성
        switch (itemType) {
            case ItemType.EAT:
                item = CreateInstance<ItemEat>();
                ((ItemEat)item).Init(itemName, itemType, desc, isSellGroup, icon, (uint)price, hp, mp);
                break;
            case ItemType.BOMB:
                item = CreateInstance<ItemBomb>();
                ((ItemBomb)item).Init(itemName, itemType, desc, isSellGroup, icon, (uint)price, (uint)damage, (uint)range);
                break;
            case ItemType.CRAFT:
                item = CreateInstance<ItemCraft>();
                item.Init(itemName, itemType, desc, isSellGroup, icon, (uint)price);
                break;
            case ItemType.KEY:
                item = CreateInstance<ItemKey>();
                item.Init(itemName, itemType, desc, isSellGroup, icon, (uint)price);
                break;
            case ItemType.EXCHANGE:
                item = CreateInstance<ItemExchange>();
                item.Init(itemName, itemType, desc, isSellGroup, icon, (uint)price);
                break;
            case ItemType.DOCUMENT:
                item = CreateInstance<ItemDocument>();
                ((ItemDocument)item).Init(itemName, itemType, desc, isSellGroup, icon, (uint)price, texture);
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }

        //생성된 아이템을 ItemInfo에 추가한다.
        itemList.AddItem(item);

        //prefab 생성
        if (isPrefab)
            CreateItemPrefab(item);
    }

    /**
        @brief  Item을 Prefab화한다.
        @item   Prefab화 할 item 
    */
    void CreateItemPrefab(Item item)
    {
        //Item Prefab Base Object
        string itemPath = "Assets/Resources/Prefabs/Item/";
        GameObject baseItem = Resources.Load("Prefabs/Item/ItemBase") as GameObject;
        
        //Prefab으로 생성
        var goItem = PrefabUtility.CreatePrefab(itemPath + item.itemName + ".prefab", baseItem);

        //item 정보 구성
        goItem.GetComponent<ItemObject>().INFO = item;
        var goIcon = goItem.transform.Find("Icon");
       
        if (goIcon != null)
            goIcon.GetComponent<SpriteRenderer>().sprite = item.icon;
    }

   
}