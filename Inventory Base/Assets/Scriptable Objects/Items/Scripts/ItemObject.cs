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

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
public class ItemObject : ScriptableObject
{

    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    public Item data = new Item();

    [TextArea(15,20)]
    public string description;

    public Item CreateItem()
    {
        Item itemScript = new Item(this);
        return itemScript;
    }
    
}
