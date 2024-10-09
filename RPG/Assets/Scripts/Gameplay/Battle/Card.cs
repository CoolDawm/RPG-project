using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject creaturePrefab;
    public int evolutionMultiplyer = 1;
    public int cardId=0;
    private CardData cardData; // Переменная для хранения данных ScriptableObject

    private void Start()
    {
        // Инициализация cardId
        if (cardId == 0) {
            cardId = Random.Range(1000000, 10000000); // You need to change it for DB card id
        }

        // Инициализация компонента TextMeshProUGUI для отображения информации о карте
        TextMeshProUGUI cardText = GetComponentInChildren<TextMeshProUGUI>();
        Creature creatureInfo = creaturePrefab.GetComponentInChildren<Creature>();

        // Создаем новый экземпляр CardsData и передаем в него данные
        cardData = ScriptableObject.CreateInstance<CardData>();
        cardData.creaturePrefab = creaturePrefab;
        cardData.evolutionMultiplyer = evolutionMultiplyer;
        cardData.cardId = cardId;

        // Отображение информации о существе на карте
        cardText.text = creatureInfo.creatureName + "\n" + " HP - " + creatureInfo.maxHP + ", Dmg - " + creatureInfo.attack + ", Dfn - " + creatureInfo.defense + ", Spd - " + creatureInfo.defense;
    }
}
