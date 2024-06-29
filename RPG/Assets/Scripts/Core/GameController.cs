using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CamerasManager cameraManager;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Начало битвы
            cameraManager.ActivateBattleCamera();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // Конец битвы и возвращение к псевдо 2D
            cameraManager.ActivatePseudo2DCamera();
        }
    }
}
