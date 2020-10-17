using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventoryScript : MonoBehaviour
{

    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEM_ITEM;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEM_ITEM;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {

        for (int i = 0; i < inventory.container.slots.Count; i++)
        {
            InventorySlot slot = inventory.container.slots[i];
            CreateItem(i, slot);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.slots.Count; i++)
        {

            InventorySlot slot = inventory.container.slots[i];

            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {
                CreateItem(i, slot);
            }
        }
    }

    private void CreateItem(int i, InventorySlot slot)
    {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.getItem[slot.item.id].uiDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        itemsDisplayed.Add(slot, obj);
    }

    public Vector3 GetPosition(int index)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEM_ITEM * (index % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEM_ITEM * (index / NUMBER_OF_COLUMNS)), 0f);
    }

}
