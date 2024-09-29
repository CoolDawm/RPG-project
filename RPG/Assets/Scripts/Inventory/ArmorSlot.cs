using UnityEngine;

public class ArmorSlot : DroneSlot
{
    public override bool CanAcceptItem(Item item)
    {
        Debug.Log("3");

        return item is Armor;
    }
}