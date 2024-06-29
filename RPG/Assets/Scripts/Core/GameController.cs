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
            // ������ �����
            cameraManager.ActivateBattleCamera();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // ����� ����� � ����������� � ������ 2D
            cameraManager.ActivatePseudo2DCamera();
        }
    }
}
