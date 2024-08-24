using UnityEngine;

using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private BattleTrigger[] _enemiesList;
    private int rewardGoldAmount;

    void Start()
    {
        rewardGoldAmount = StartMission.Instance.selectedMission.rewardGoldAmount;
        _enemiesList = FindObjectsOfType<BattleTrigger>();
        Debug.Log("Enemies Amount = " + _enemiesList.Length);
    }

    public void CheckForReturn()
    {
        for (int i = 0; i < _enemiesList.Length; i++)
        {
            if (_enemiesList[i] != null) return;
        }

        CurrencyManager.Instance.ApplyMissionReward(rewardGoldAmount);
        rewardGoldAmount = 0;
        Debug.Log("Mission completed! Reward set to " + rewardGoldAmount + " gold.");
        GetComponent<SceneChange>().ChangeScene("City");
    }

    public void InvokeCheckingAfterTime()
    {
        Invoke("CheckForReturn", 1);
    }
}

