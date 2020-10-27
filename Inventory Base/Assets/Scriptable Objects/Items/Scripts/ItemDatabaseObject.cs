using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] itemObjects;

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
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (itemObjects[i].data.id != i)
            {
                itemObjects[i].data.id = i;
            }
        }
    }
}
