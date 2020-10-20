using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    FOOD,
    HELMET,
    WEAPON,
    SHIELD,
    BOOTS,
    ARMOR,
    DEFAULT
}


public enum Attributes
{
    AGILITY,
    INTELLECT,
    STAMINA,
    STRENGTH
}

public abstract class ItemObject : ScriptableObject
{

    public int id;
    public Sprite uiDisplay;
    public ItemType type;
    public ItemBuffScript[] buffs; 

    [TextArea(15,20)]
    public string description;

    public Item CreateItem()
    {
        Item itemScript = new Item(this);
        return itemScript;
    }
    
}
