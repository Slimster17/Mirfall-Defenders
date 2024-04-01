using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    private InputReader _inputReader;
    [SerializeField] private float moveSpeed = 50f;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.moveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.moveEvent -= OnMoveInput;
    }


    void OnMoveInput(Vector3 direction)
    {
        _moveDirection = new Vector3(direction.x, direction.y, direction.z);
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(_moveDirection.x, _moveDirection.z, _moveDirection.y).normalized;
        
        transform.Translate(_moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
