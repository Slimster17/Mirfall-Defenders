using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private GameObject _placeableObject;
    [SerializeField] private bool _isPlaceable = true;
    private InputReader _inputReader;

    public bool IsPlaceable
    {
        get { return _isPlaceable; }
    }
    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
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
        if (clickedObject == gameObject && IsPlaceable)
        {
            // Debug.Log("Clicked on: " + gameObject.name);
            GameObject.Instantiate(_placeableObject, transform.position, Quaternion.identity);
            _isPlaceable = false;
        }
    }

}
