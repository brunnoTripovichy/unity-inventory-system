using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInventoryInterfaceScript : MonoBehaviour
{

    public PlayerScript player;

    public InventoryObject inventory;

    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

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
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.getItem[_slot.Value.item.id].uiDisplay;
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

    public abstract void CreateSlots();

    public void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverSlot = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverSlot = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rectTrans = mouseObject.AddComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(200, 200);
        mouseObject.transform.SetParent(transform.parent);

        if (itemsDisplayed[obj].id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.getItem[itemsDisplayed[obj].id].uiDisplay;
            img.raycastTarget = false;
        }

        player.mouseItem.obj = mouseObject;
        player.mouseItem.slot = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverSlot = itemOnMouse.hoverSlot;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var getItemObject = inventory.database.getItem;

        if (itemOnMouse.interfaceScript)
        {
            if (mouseHoverObj)
            {
                if (mouseHoverSlot.CanPlaceInSlot(getItemObject[itemsDisplayed[obj].id])
                    && (mouseHoverSlot.item.id <= -1 || (mouseHoverSlot.item.id >= 0 && itemsDisplayed[obj].CanPlaceInSlot(getItemObject[mouseHoverSlot.item.id]))))
                {
                    inventory.MoveItem(itemsDisplayed[obj], mouseHoverSlot.parent.itemsDisplayed[itemOnMouse.hoverObj]);
                }

            }
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(itemOnMouse.obj);
        itemOnMouse.slot = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnEnterInterface(GameObject obj)
    {
        player.mouseItem.interfaceScript = obj.GetComponent<UserInventoryInterfaceScript>();
    }

    public void OnExitInterface(GameObject obj)
    {
        player.mouseItem.interfaceScript = null;
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

public class MouseItem
{
    public UserInventoryInterfaceScript interfaceScript;
    public GameObject obj;
    public InventorySlot slot;
    public InventorySlot hoverSlot;
    public GameObject hoverObj;
}
