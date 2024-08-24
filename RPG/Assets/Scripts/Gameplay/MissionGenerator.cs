using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionGenerator : MonoBehaviour
{
    [SerializeField] private GameObject missionButtonPrefab;
    [SerializeField] private Transform missionButtonParent;  
    private List<Mission> generatedMissions;

    private void Start()
    {
        GenerateMissions();
        CreateMissionButtons();
    }

    private void GenerateMissions()
    {
        generatedMissions = new List<Mission>();
        int missionCount = Random.Range(3, 6); 

        for (int i = 0; i < missionCount; i++)
        {
            Mission newMission = new Mission
            {
                missionName = "Mission " + (i + 1),
                rewardGoldAmount = Random.Range(50, 201) 
            };
            generatedMissions.Add(newMission);
        }
    }

    private void CreateMissionButtons()
    {
        foreach (Mission mission in generatedMissions)
        {
            GameObject missionButton = Instantiate(missionButtonPrefab, missionButtonParent);

          
            TMP_Text buttonText = missionButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = mission.missionName;
            }


            Button button = missionButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    StartMission.Instance.SetSelectedMission(mission);
                });
            }
        }
    }
}
