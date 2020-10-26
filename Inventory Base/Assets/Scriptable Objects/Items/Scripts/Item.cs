using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id = -1;
    public string name;
    public ItemBuff[] buffs;

    public Item()
    {
        id = -1;
        name = "";
    }

    public Item(ItemObject itemObject)
    {
        id = itemObject.data.id;
        name = itemObject.name;
        buffs = new ItemBuff[itemObject.data.buffs.Length];

        for (int i = 0; i < itemObject.data.buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemObject.data.buffs[i].min, itemObject.data.buffs[i].max)
            {
                attribute = itemObject.data.buffs[i].attribute
            };
        }
    }
}
