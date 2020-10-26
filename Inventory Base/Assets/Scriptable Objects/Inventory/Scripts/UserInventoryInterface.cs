using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInventoryInterface : MonoBehaviour
{

    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.container.slots.Length; i++)
        {
            inventory.container.slots[i].parent = this;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }

    public abstract void CreateSlots();

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInventoryInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.id >= 0)
        {
            tempItem = new GameObject();
            var rectTrans = tempItem.AddComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(200, 200);
            tempItem.transform.SetParent(transform.parent);

            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }

        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

}

public static class MouseData
{
    public static UserInventoryInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}