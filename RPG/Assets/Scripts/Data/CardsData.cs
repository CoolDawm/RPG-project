using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsData : MonoBehaviour
{
    public static CardsData Instance;

    public List<Card> Cards = new List<Card>();

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

    public void SetCards(List<Card> cards)
    {
        Cards = new List<Card>(cards);
    }
}
