using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public enum UserInterfaceType
{
    INVENTORY,
    EQUIPMENT,
    CHEST
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public string savePath;
    public ItemDatabaseObject database;
    public UserInterfaceType interfaceType;
    public Inventory container;

    public InventorySlot[] GetSlots
    {
        get
        {
            return container.slots;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FinditemOnInventory(item);
        if (!database.itemObjects[item.id].stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }

        slot.AddAmount(amount);
        return true;
    }

    public InventorySlot FinditemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id == item.id)
            {
                return GetSlots[i];
            }
        }

        return null;
    }

    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.id <= -1)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public InventorySlot SetEmptySlot(Item item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id <= -1)
            {
                GetSlots[i].UpdateSlot(item, _amount);
                return GetSlots[i];
            }
        }

        // Set up functionality for when its full
        return null;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        if (slot2.CanPlaceInSlot(slot1.ItemObject) && slot1.CanPlaceInSlot(slot2.ItemObject))
        {
            InventorySlot tem = new InventorySlot(slot2.item, slot2.amount);
            slot2.UpdateSlot(slot1.item, slot1.amount);
            slot1.UpdateSlot(tem.item, tem.amount);
        }
    }

    public void RemoveItem(Item item)
    {
        foreach (var slot in GetSlots)
        {
            if (slot.item == item)
            {
                slot.UpdateSlot(null, 0);
            }
        }

    }

    [ContextMenu("Save")]
    public void Save()
    {
        // JSON Save method
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter binaryFormatter = new BinaryFormatter();
        //FileStream fileStream = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //binaryFormatter.Serialize(fileStream, saveData);
        //fileStream.Close();

        // IFormatter method
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            // JSON method
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            //FileStream fileStream = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(), this);
            //fileStream.Close();

            // IFormatter Method
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].amount);
            }

            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }

}
