using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    private Battle _battle;
    private CamerasManager _cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        _battle = FindAnyObjectByType<Battle>();
        _cameraManager = FindAnyObjectByType<CamerasManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) {
            Drone[] drones = GameObject.FindObjectsOfType<Drone>();
            foreach (Drone drone in drones)
            {
                if (drone.gameObject.tag=="Player") 
                {
                    if (drone.CheckForActiveCards()) { 
                        _battle.StartBattle(gameObject);
                        _cameraManager.ActivateBattleCamera();
                        break; 
                    }
                }
            }
        }
    }
}
