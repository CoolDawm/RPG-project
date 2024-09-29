using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<Item> shopItems; // Список предметов в магазине
    public Transform shopContent; // Контент в UI
    public GameObject itemPrefab; // Префаб предмета
    private CurrencyManager _currencyManager;
    private InventoryManager _inventoryManager;
    void Start()
    {
        _currencyManager = FindObjectOfType<CurrencyManager>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
        PopulateShop();
        Debug.Log(_currencyManager);
       
    }

    void PopulateShop()
    {
        foreach (var item in shopItems)
        {
            GameObject newItem = Instantiate(itemPrefab, shopContent);
            newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " - " + item.price + " Gold";
            newItem.GetComponent<Button>().onClick.AddListener(() => PurchaseItem(item));
        }
    }

    public void PurchaseItem(Item item)
    {
       
        if (_currencyManager == null)
        {
            _currencyManager = CurrencyManager.Instance;
        }
        if (_currencyManager.Gold >= item.price)
        {
            _currencyManager.SpendGold(item.price);
            _inventoryManager.AddItem(item);
            Debug.Log("Purchased: " + item.itemName);
        }
        else
        {
            Debug.Log("Not enough gold!" + _currencyManager.Gold +"or" + CurrencyManager.Instance.Gold);
        }
    }
}
