using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{

    [System.NonSerialized]
    public UserInventoryInterface parent;
    public Item item;
    public int amount;
    public ItemType[] allowedItems = new ItemType[0];

    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }

    public InventorySlot(Item _itemObject, int _amount)
    {
        item = _itemObject;
        amount = _amount;
    }

    public ItemObject ItemObject
    {
        get
        {
            if (item.id >= 0)
            {
                return parent.inventory.database.items[item.id];
            }
            return null;
        }
    }

    public void UpdateSlot(Item _itemObject, int _amount)
    {
        item = _itemObject;
        amount = _amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedItems.Length <= 0 || _itemObject == null || _itemObject.data.id < 0)
        {
            return true;
        }

        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (_itemObject.type == allowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}
