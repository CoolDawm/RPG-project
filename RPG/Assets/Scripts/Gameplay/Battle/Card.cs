using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : MonoBehaviour
{
    public GameObject creaturePrefab;
    public int evolutionMultiplyer = 1;
    public int cardId;
    private void Start()
    {
        cardId = Random.Range(1000000,10000000);//You need to change it for DB card id
        TextMeshProUGUI cardText=GetComponentInChildren<TextMeshProUGUI>();
        Creature creatureInfo=creaturePrefab.GetComponentInChildren<Creature>();
        cardText.text=creatureInfo.creatureName  +"\n"+ " HP - "+creatureInfo.maxHP + ", Dmg - "+creatureInfo.attack+", Dfn - "+creatureInfo.defense+", Spd - "+creatureInfo.defense;
    }
}
