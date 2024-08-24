using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    public static InventoryData Instance;
    public List<Item> items = new List<Item>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetItems(List<Item> items)
    {
        this.items = new List<Item>(items);
        Debug.Log($"items ={items.Count}");
    }
}
