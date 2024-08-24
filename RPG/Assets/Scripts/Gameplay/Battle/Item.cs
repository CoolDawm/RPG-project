using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int healAmount;
    public int price;
    public override string ToString()
    {
        return itemName;
    }
}
