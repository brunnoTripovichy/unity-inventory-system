using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{

    [System.NonSerialized]
    public UserInventoryInterface parent;

    [System.NonSerialized]
    public GameObject slotDisplay;

    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;

    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;

    public Item item;
    public int amount;
    public ItemType[] allowedItems = new ItemType[0];

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _itemObject, int _amount)
    {
        UpdateSlot(_itemObject, _amount);
    }

    public ItemObject ItemObject
    {
        get
        {
            if (item.id >= 0)
            {
                return parent.inventory.database.itemObjects[item.id];
            }
            return null;
        }
    }

    public void UpdateSlot(Item _itemObject, int _amount)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }

        item = _itemObject;
        amount = _amount;

        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
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

public delegate void SlotUpdated(InventorySlot _slot);
