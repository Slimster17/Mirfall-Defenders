using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower _placeableObject;
    [SerializeField] private bool _isPlaceable = true;
    private InputReader _inputReader;
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    private Vector2Int _coordinates;

    public bool IsPlaceable
    {
        get { return _isPlaceable; }
    }
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = FindObjectOfType<PathFinder>();
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
    
    private void Start()
    {
        if (_gridManager != null)
        {
            _coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
            if (!IsPlaceable)
            {
                _gridManager.BlockNode(_coordinates);
            }
        }
    }


    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        if (clickedObject == gameObject && 
            _gridManager.GetNode(_coordinates).isWalkable && 
            !_pathFinder.WillBlockPath(_coordinates))
        {
            bool isSuccessful = _placeableObject.CreateTower(_placeableObject,transform.position);
            if (isSuccessful)
            {
                _gridManager.BlockNode(_coordinates);
                _pathFinder.NotifyReceivers();
            }
        }
    }

}
