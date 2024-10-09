using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int healAmount;
    public int price;
    public Sprite icon;
    public bool isStackable;
   
    public override string ToString()
    {
        return itemName;
    }
}
