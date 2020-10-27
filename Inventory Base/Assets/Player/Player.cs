using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    public InventoryObject equipment;
    public Attribute[] attributes;

    private void Start()
    {
        foreach (var att in attributes)
        {
            att.SetParent(this);
        }

        foreach (var slot in equipment.GetSlots)
        {
            slot.OnBeforeUpdate += OnBeforeSlotUpdate;
            slot.OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

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

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }

        switch (_slot.parent.inventory.interfaceType)
        {
            case UserInterfaceType.INVENTORY:
                break;
            case UserInterfaceType.EQUIPMENT:

                foreach (var buff in _slot.item.buffs)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute.type == buff.attribute)
                        {
                            attribute.value.RemoveModifier(buff);
                        }
                    }
                }

                break;
            case UserInterfaceType.CHEST:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }

        switch (_slot.parent.inventory.interfaceType)
        {
            case UserInterfaceType.INVENTORY:
                break;
            case UserInterfaceType.EQUIPMENT:

                foreach (var buff in _slot.item.buffs)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute.type == buff.attribute)
                        {
                            attribute.value.AddModifier(buff);
                        }
                    }
                }

                break;
            case UserInterfaceType.CHEST:
                break;
            default:
                break;
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

    public void AttributeModified(Attribute attribute)
    {

    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}
