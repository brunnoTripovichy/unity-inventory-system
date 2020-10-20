using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateSlot(new Item(), -1, 0);
        }
    }
}
