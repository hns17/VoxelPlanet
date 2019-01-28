/**
    @file   SlotEvent.cs
    @date   2018.08.15
    @author 황준성(hns17.tistory.com)
*/
using UnityEngine;
using UnityEngine.UI;

/**
    @class  SlotEvent
    @date   2018.08.15
    @author 황준성(hns17.tistory.com)
    @brief  Slot Item의 tooltip 및 사용과 관련된 이벤트
*/
public class SlotEvent : MonoBehaviour {
    //item info
    private Item item = null;
    private Image slotIcon;
    private Text itemCnt;

    //tooltip object
    public GameObject tooltip;

    //ToolTip Info
    private Image   tipIcon;
    private Text    tipName;
    private Text    tipDesc;
    private Text    tipPrice;
    private Text    tipOption;
    private Text    tipOption2;


    private void Awake()
    {
        slotIcon = GetComponent<Image>();
        itemCnt = GetComponentInChildren<Text>();

        var tfTooltip = tooltip.transform;

        tipIcon = tfTooltip.Find("Icon").GetComponent<Image>();
        tipName = tfTooltip.Find("Name").GetComponent<Text>();
        tipDesc = tfTooltip.Find("Desc").GetChild(0).GetComponent<Text>();
        tipPrice = tfTooltip.Find("Price").GetComponent<Text>();

        tipOption = tfTooltip.Find("Option_1").GetComponent<Text>();
        tipOption2 = tfTooltip.Find("Option_2").GetComponent<Text>();

        gameObject.SetActive(false);
    }


    /**
        @brief  Slot Item 정보 Set
        @param  item : item info
    */
    public void SetItem(Item item, int cnt)
    {
        this.item = item;
        slotIcon.sprite = item.icon;
        itemCnt.text = cnt.ToString();
        gameObject.SetActive(true);
    }

    /**
        @brief  item tooltip을 보여준다.
    */
    public void ShowTooltip()
    {

        tipIcon.sprite = item.icon;
        tipName.text = item.itemName;
        tipDesc.text = item.desc;

        if (item.isSell)
            tipPrice.text = "Price : " + item.price;
        else
            tipPrice.text = "Price : Not Sell";
        

        tipOption.text = "";
        tipOption2.text = "";

        if (item.type == ItemType.EAT)
        {
            tipOption.text = "HP : " + ((ItemEat)item).hp;
            tipOption2.text = "MP : " + ((ItemEat)item).mp;
        }
        else if (item.type == ItemType.BOMB)
        {
            tipOption.text = "Range : " + ((ItemBomb)item).range;
            tipOption2.text = "Damge : " + ((ItemBomb)item).damage;
        }


        tooltip.transform.position = transform.position + new Vector3(-85, 90, 0);
        tooltip.SetActive(true);
    }

    /**
        @brief  Tooltip을 숨긴다
    */
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    /**
        @brief  Slot Item 사용한다.
    */
    public void UseItem()
    {
        item.Update();
    }

}
