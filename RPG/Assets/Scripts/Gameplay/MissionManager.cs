using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private BattleTrigger[] _enemiesList;
    
    void Start()
    {
        _enemiesList = FindObjectsOfType<BattleTrigger>();
        Debug.Log("Enemies Amount = "+_enemiesList.Length);
    }
    public void CheckForReturn()
    {
        for(int i = 0; i < _enemiesList.Length; i++)
        {
            if (_enemiesList[i] != null) return;
        }
        GetComponent<SceneChange>().ChangeScene("City");
    }
    public void InvokeCheckingAfterTime()
    {
        Invoke("CheckForReturn",1);
    }
}
