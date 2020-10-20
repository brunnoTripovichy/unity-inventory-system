﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public MouseItem mouseItem = new MouseItem();
    public InventoryObject inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            inventory.Load();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var itemScript = other.GetComponent<GroundItemScript>();
        
        if (itemScript)
        {
            inventory.AddItem(new Item(itemScript.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.slots = new InventorySlot[24];
    }
}
