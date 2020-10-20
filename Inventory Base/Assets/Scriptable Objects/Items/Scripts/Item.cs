using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public ItemBuffScript[] buffs;

    public Item()
    {
        id = -1;
        name = "";
    }

    public Item(ItemObject itemObject)
    {
        id = itemObject.id;
        name = itemObject.name;
        buffs = new ItemBuffScript[itemObject.buffs.Length];

        for (int i = 0; i < itemObject.buffs.Length; i++)
        {
            buffs[i] = new ItemBuffScript(itemObject.buffs[i].min, itemObject.buffs[i].max)
            {
                attribute = itemObject.buffs[i].attribute
            };
        }
    }
}
