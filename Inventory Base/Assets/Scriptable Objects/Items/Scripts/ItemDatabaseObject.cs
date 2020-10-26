using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;

    public void OnAfterDeserialize()
    {

        UpdateId();

    }

    public void OnBeforeSerialize()
    {
    }

    [ContextMenu("Update Id's")]
    public void UpdateId()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].data.id != i)
            {
                items[i].data.id = i;
            }
        }
    }
}
