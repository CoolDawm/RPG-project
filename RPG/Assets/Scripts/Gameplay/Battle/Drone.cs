using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public string droneName;
    public List<Card> cards = new List<Card>();
    public List<Creature> activeCreatures = new List<Creature>();
    public List<Item> items = new List<Item>();

    void Start()
    {
        int i = 0;
        foreach (var card in cards)
        {
            GameObject creatureObj = Instantiate(card.creaturePrefab);
            Creature creature = creatureObj.GetComponent<Creature>();
            activeCreatures.Add(creature);
            creature.Initialize($"{gameObject.name} creature number " + (i + 1));
            i++;
        }
        if (gameObject.tag == "Player")
        {
            items=InventoryData.Instance.items;
        }
    }

    public void UseItem(Creature creature, Item item)
    {
        creature.Heal(item.healAmount);
        Debug.Log($"{droneName} used {item.itemName} on {creature.creatureName}. It healed {item.healAmount} HP.");
        items.Remove(item);
    }
    public bool CheckForActiveCards()
    {
        if (cards.Count!=0)
        {
            return true;
        }else
        {
            return false;
        }
    }
}
