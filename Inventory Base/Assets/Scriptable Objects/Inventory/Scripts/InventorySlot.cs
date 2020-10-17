using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemScript item;
    public int id;
    public int amount;

    public InventorySlot(ItemScript _itemObject, int _id, int _amount)
    {
        item = _itemObject;
        id = _id;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
