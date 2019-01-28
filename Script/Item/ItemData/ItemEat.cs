/**
    @file   ItemEat.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
*/

using UnityEngine;

/**
    @class  ItemEat
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  음식, 포션 등
*/
public class ItemEat : Item
{
    public int hp = 0;
    public int mp = 0;

    public void Init(string name, ItemType type, string desc, bool isSell, Sprite icon, uint price, int hp, int mp)
    {
        base.Init(name, type, desc, isSell, icon, price);

        this.hp = hp;
        this.mp = mp;

    }

    public override void Update()
    {
        base.Update();
    }
}
