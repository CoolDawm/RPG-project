using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMission : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _missionInfoPanel;
    [SerializeField]
    private GameObject _cardsParent;
    [SerializeField]
    private SceneChange _sceneChanger;
    public static StartMission Instance { get; private set; }
    public Mission selectedMission;
    public string sceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetSelectedMission(Mission mission)
    {
        selectedMission = mission;
        _missionInfoPanel.text = $"{mission.missionName}\nGold Reward: {mission.rewardGoldAmount}";

    }

    public void SaveCardsAndLoadMissionScene()
    {
        List<Card> cards = new List<Card>();

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

