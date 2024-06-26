using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public CinemachineVirtualCamera pseudo2DCamera;
    public CinemachineVirtualCamera battleCamera;

    private void Start()
    {
        // Активируем псевдо 2D камеру при старте
        ActivatePseudo2DCamera();
    }

    public void ActivatePseudo2DCamera()
    {
        pseudo2DCamera.Priority = 10;
        battleCamera.Priority = 0;
    }

    public void ActivateBattleCamera()
    {
        pseudo2DCamera.Priority = 0;
        battleCamera.Priority = 10;
    }
}
