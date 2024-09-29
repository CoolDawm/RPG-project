using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    public GameObject creaturePrefab;
    public int evolutionMultiplyer = 1;
    public int cardId;
}