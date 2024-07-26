using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardPrefab;
    public CamerasManager cameraManager;
    public GameObject cardsParent;
    private void Start()
    {
        SpawnCards();
    }

    private void SpawnCards()
    {
        Debug.Log(CardsData.Instance.Cards.Count);
        foreach (Card card in CardsData.Instance.Cards)
        {
            GameObject newCard = Instantiate(_cardPrefab, cardsParent.transform);
            newCard.GetComponent<Card>().creaturePrefab = card.creaturePrefab;
            newCard.transform.SetParent(cardsParent.transform);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Начало битвы
            cameraManager.ActivateBattleCamera();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // Конец битвы и возвращение к псевдо 2D
            cameraManager.ActivatePseudo2DCamera();
        }
    }
}
