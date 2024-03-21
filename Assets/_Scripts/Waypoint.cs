using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Waypoint : MonoBehaviour
{
    private InputReader _inputReader;
    private Camera _mainCamera;

    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
        _mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        _inputReader.clickEvent += OnClickInput;
    }

    private void OnDisable()
    {
        _inputReader.clickEvent -= OnClickInput;
    }

    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        if (clickedObject == gameObject)
        {
            Debug.Log("Clicked on: " + gameObject.name);
        }
    }

}
