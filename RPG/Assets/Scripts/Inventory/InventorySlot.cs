
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public enum ItemType
{
    Weapon,
    Defence,
    Driver,
    Item
}


public class InventorySlot : MonoBehaviour, ISlot, IDropHandler
{
    [SerializeField]
    private ItemType _allowedItemType;
    public Image icon;
    //public Button removeButton;
    public Item item;
    public Item Item => item;
    private int _itemStackCount = 0;
    public Action onItemInfoUpdate;

    private void Start()
    {
        icon=GetComponentInParent<Image>();
        if (item != null)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item newItem)
    {
        if (!CanAcceptItem(newItem))
        {
            Debug.Log("���������� �������� �������: " + newItem.name);
            return; // ������� �� ��������
        }

        item = newItem;
        icon.sprite = item.icon;
        if (item.isStackable)
        {
            _itemStackCount++;
        }
    }


    public void ClearSlot()
    {
        item = null;
        Debug.Log(icon);
        icon.sprite = null;
        _itemStackCount = 0;
       // icon.enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            // Add functionality to use the item here
        }
    }

    public bool CanAcceptItem(Item item)
    {
        if (item == null) return true; 
        return item.itemType == _allowedItemType; 
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler dragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if (dragHandler != null)
        {
            SwapItems(dragHandler.parentSlot);
        }
    }
    

    void SwapItems(ISlot otherSlot)
    {
        Item tempItem = otherSlot.Item;
        if (item != null && !otherSlot.CanAcceptItem(item)) return;

        if (item != null && otherSlot.CanAcceptItem(item))
        {
            otherSlot.AddItem(item);
            if (item.isStackable)
            {
                otherSlot.SetStackCount(_itemStackCount);
                _itemStackCount = 0;
            }
        }
        else
        {
            otherSlot.ClearSlot();
        }

        if (tempItem != null)
        {
            AddItem(tempItem);
            if (tempItem.isStackable)
            {
                otherSlot.GetStackCount();
            }
        }
        else
        {
            ClearSlot();
        }
    }
    public void SetStackCount(int count)
    {
        _itemStackCount = count;
        onItemInfoUpdate?.Invoke();
    }
    
    public void IncreaseStackCount()
    {
        _itemStackCount++;
        onItemInfoUpdate?.Invoke();

    }
    public int GetStackCount()
    {
        return _itemStackCount;
    }
}
