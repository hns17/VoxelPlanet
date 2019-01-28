/**
    @file   ItemBomb.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  폭탄 아이템
*/
using UnityEngine;

/**
    @class  ItemBomb
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  폭탄 아이템
*/
public class ItemBomb : Item
{
    public uint damage = 0;
    public uint range = 0;

    public void Init(string name, ItemType type, string desc, bool isSell, Sprite icon, uint price, uint damage, uint range)
    {
        base.Init(name, type, desc, isSell, icon, price);

        this.damage = damage;
        this.range = range;
    }

    public override void Update()
    {
        base.Update();
    }
}