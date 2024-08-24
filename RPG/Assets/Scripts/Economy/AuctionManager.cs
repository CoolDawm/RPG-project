using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuctionManager : MonoBehaviour
{
    //from pubic to private all of it
    public List<Card> auctionCards; 
    public Transform cardsPanel;
    public GameObject cardButtonPrefab; 
    public GameObject cardPrefab; 
    public CurrencyManager _currencyManager;
    public Transform _parentForButtons;
    void Start()
    {
        _currencyManager = CurrencyManager.Instance;
        PopulateAuction();
    }

    void PopulateAuction()
    {
        foreach (var card in auctionCards)
        {
            GameObject newCardButton = Instantiate(cardButtonPrefab, _parentForButtons);
            TextMeshProUGUI buttonText = newCardButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = card.name + " - " + (card.evolutionMultiplyer * 25) + " Diamonds"; // Предполагаемая цена
            newCardButton.GetComponent<Button>().onClick.AddListener(() => PurchaseCard(card));
        }
    }

    public void PurchaseCard(Card card)
    {
        int cardPrice = card.evolutionMultiplyer * 25; // Рассчитываем цену карты

        if (_currencyManager.Diamonds >= cardPrice)
        {
            _currencyManager.SpendDiamonds(cardPrice);
            SpawnCard(card);
            Debug.Log("Purchased: " + card.name);
        }
        else
        {
            Debug.Log("Not enough diamonds!");
        }
    }

    private void SpawnCard(Card card)
    {
        GameObject newCardInstance = Instantiate(cardPrefab);
        newCardInstance.name = card.name;
        newCardInstance.transform.SetParent(cardsPanel, false); 
        Draggable dragComp=newCardInstance.GetComponent<Draggable>();
        Destroy(dragComp);
        Card cardComponent = newCardInstance.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.creaturePrefab = card.creaturePrefab; 
            cardComponent.evolutionMultiplyer = card.evolutionMultiplyer;
        }
        Debug.Log("Card spawned and assigned: " + card.name);
    }
}
