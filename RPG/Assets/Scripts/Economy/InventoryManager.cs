using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventoryItems = new List<Item>();
    private InventoryData _inventoryData;
    private void Start()
    {
        _inventoryData = InventoryData.Instance;
    }
    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
        Debug.Log("Item added to inventory: " + item.itemName+ $"  {inventoryItems.Count}");
        _inventoryData.SetItems(inventoryItems);
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);
        Debug.Log("Item removed from inventory: " + item.itemName);
        _inventoryData.SetItems(inventoryItems);

    }

    public void DisplayInventory()
    {
        foreach (var item in inventoryItems)
        {
            Debug.Log("Inventory Item: " + item.itemName);
        }
    }
}
