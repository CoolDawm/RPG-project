using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestingButtons : MonoBehaviour
{
    private Battle _battle;
    private void Start()
    {
        _battle = GameObject.FindAnyObjectByType<Battle>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Battle Should Start");
            _battle.StartBattle();
        }

    }
}
