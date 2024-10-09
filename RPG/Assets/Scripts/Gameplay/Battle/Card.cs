using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject creaturePrefab;
    public int evolutionMultiplyer = 1;
    public int cardId=0;
    private CardData cardData; // ���������� ��� �������� ������ ScriptableObject

    private void Start()
    {
        // ������������� cardId
        if (cardId == 0) {
            cardId = Random.Range(1000000, 10000000); // You need to change it for DB card id
        }

        // ������������� ���������� TextMeshProUGUI ��� ����������� ���������� � �����
        TextMeshProUGUI cardText = GetComponentInChildren<TextMeshProUGUI>();
        Creature creatureInfo = creaturePrefab.GetComponentInChildren<Creature>();

        // ������� ����� ��������� CardsData � �������� � ���� ������
        cardData = ScriptableObject.CreateInstance<CardData>();
        cardData.creaturePrefab = creaturePrefab;
        cardData.evolutionMultiplyer = evolutionMultiplyer;
        cardData.cardId = cardId;

        // ����������� ���������� � �������� �� �����
        cardText.text = creatureInfo.creatureName + "\n" + " HP - " + creatureInfo.maxHP + ", Dmg - " + creatureInfo.attack + ", Dfn - " + creatureInfo.defense + ", Spd - " + creatureInfo.defense;
    }
}
