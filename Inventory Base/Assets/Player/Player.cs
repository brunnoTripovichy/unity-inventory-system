using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    public InventoryObject equipment;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItemScript>();
        
        if (item)
        {
            Item _item = new Item(item.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.Clear();
        equipment.container.Clear();
    }
}
