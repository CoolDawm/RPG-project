using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMission : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardsParent;
    [SerializeField]
    private SceneChange _sceneChanger;
    public string sceneName;
    public void SaveCardsAndLoadMissionScene()
    {
        List<Card> cards = new List<Card>();

        Debug.Log(_cardsParent.transform.childCount);
        foreach (Transform cardTransform in _cardsParent.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                cards.Add(card);
            }
        }

        CardsData.Instance.SetCards(cards);

        _sceneChanger.ChangeScene(sceneName);
    }
}
