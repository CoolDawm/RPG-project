using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EvolutionManager : MonoBehaviour
{
    public EvolutionChain evolutionChain;
    public Transform playerCardPanel;

    public void TryEvolve(GameObject cardPrefab)
    {
        List<Card> matchingCards = new List<Card>();
        Card cardToEvolve = cardPrefab.GetComponent<Card>();
        Debug.Log(playerCardPanel.childCount);
        foreach (Transform child in playerCardPanel)
        {
            Card card = child.GetComponent<Card>();
            if (card != null && card.creaturePrefab == cardToEvolve.creaturePrefab)
            {
                matchingCards.Add(card);
            }
        }

        if (matchingCards.Count >= 3)
        {
            Debug.Log("Should Evolve");
            Evolve(matchingCards, cardToEvolve.creaturePrefab);
        }
    }

    private void Evolve(List<Card> matchingCards, GameObject currentPrefab)
    {
        GameObject nextPrefab = evolutionChain.GetNextEvolution(currentPrefab);
        if (nextPrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(matchingCards[i].gameObject);
            }

            GameObject newCardObject = Instantiate(Resources.Load<GameObject>("CreatureCard"), playerCardPanel);
            Card newCard = newCardObject.GetComponent<Card>();
            newCard.creaturePrefab = nextPrefab;
        }
        else
        {
            Debug.LogWarning("No further evolution found for the current creature.");
        }
    }
}
