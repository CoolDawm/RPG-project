using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f; // Скорость передвижения
    private Rigidbody _rb;
    private Vector3 _movement;
    private bool _inBattle;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        MovePlayer();
    }

    

    void MovePlayer()
    {
        if (_inBattle) return;
        float moveX = Input.GetAxis("Horizontal"); 
        float movey = Input.GetAxis("Vertical");   

        _movement = new Vector3(-moveX, movey, 0f);  
        _rb.MovePosition(transform.position + _movement * _moveSpeed * Time.fixedDeltaTime);
    }
    public void ChangeBattleStatus()
    {
        _inBattle = !_inBattle;
    }
}
