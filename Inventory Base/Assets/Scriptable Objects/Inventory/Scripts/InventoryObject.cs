using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;

    public void AddItem(Item item, int amount)
    {
        // Change to a bool isStackable on itemScript
        if (item.buffs.Length > 0)
        {
            SetEmptySlot(item, amount);
            return;
        }

        foreach (var slot in container.slots)
        {
            if (slot.id == item.id)
            {
                slot.AddAmount(amount);
                return;
            }
        }

        SetEmptySlot(item, amount);

    }

    public InventorySlot SetEmptySlot(Item item, int _amount)
    {
        for (int i = 0; i < container.slots.Length; i++)
        {
            if (container.slots[i].id <= -1)
            {
                container.slots[i].UpdateSlot(item, item.id, _amount);
                return container.slots[i];
            }
        }

        // Set up functionality for when its full
        return null;
    }

    public void MoveItem(InventorySlot slot1, InventorySlot slot2)
    {
        InventorySlot tem = new InventorySlot(slot2.item, slot2.id, slot2.amount);
        slot2.UpdateSlot(slot1.item, slot1.id, slot1.amount);
        slot1.UpdateSlot(tem.item, tem.id, tem.amount);
    }

    public void RemoveItem(Item item)
    {
        foreach (var slot in container.slots)
        {
            if (slot.item == item)
            {
                slot.UpdateSlot(null, -1, 0);
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

            for (int i = 0; i < container.slots.Length; i++)
            {
                container.slots[i].UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].id, newContainer.slots[i].amount);
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
