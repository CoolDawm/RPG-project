using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject itemIconPrefab;

    Inventory inventory;
    InventorySlot[] slots;
    public WeaponSlot weaponSlot;  
    public ArmorSlot armorSlot;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }
    /*
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
    */
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        // Обновите слоты персонажа
        if (weaponSlot.item != null)
        {
            weaponSlot.AddItem(weaponSlot.item);
        }
        else
        {
            weaponSlot.ClearSlot();
        }

        if (armorSlot.item != null)
        {
            armorSlot.AddItem(armorSlot.item);
        }
        else
        {
            armorSlot.ClearSlot();
        }
    }
}
