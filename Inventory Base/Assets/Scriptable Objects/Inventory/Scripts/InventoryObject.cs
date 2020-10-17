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
    public InventoryScript container;

    public void AddItem(ItemScript item, int amount)
    {
        // Change to a bool isStackable on itemScript
        if (item.buffs.Length > 0)
        {
            container.slots.Add(new InventorySlot(item, item.id, amount));
            return;
        }

        foreach (var slot in container.slots)
        {
            if (slot.item.id == item.id)
            {
                slot.AddAmount(amount);
                return;
            }
        }

        container.slots.Add(new InventorySlot(item, item.id, amount));
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
            container = (InventoryScript)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new InventoryScript();
    }

}
