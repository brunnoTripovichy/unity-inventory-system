using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{

    public UserInventoryInterfaceScript parent;
    public Item item;
    public int id;
    public int amount;
    public ItemType[] allowedItems = new ItemType[0];

    public InventorySlot()
    {
        item = null;
        id = -1;
        amount = 0;
    }

    public InventorySlot(Item _itemObject, int _id, int _amount)
    {
        item = _itemObject;
        id = _id;
        amount = _amount;
    }

    public void UpdateSlot(Item _itemObject, int _id, int _amount)
    {
        item = _itemObject;
        id = _id;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (allowedItems.Length <=0)
        {
            return true;
        }

        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (itemObject.type == allowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}
