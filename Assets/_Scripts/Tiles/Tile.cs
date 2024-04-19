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
    private GridManager[] _gridManagers;
    private PathFinder _pathFinder;
    private Vector2Int _coordinates;

    public bool IsPlaceable
    {
        get { return _isPlaceable; }
    }
    private void Awake()
    {
        _gridManagers = FindObjectsOfType<GridManager>();
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
        foreach (var gridManager in _gridManagers)
        {
            if (gridManager != null)
            {
                _coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
                if (!IsPlaceable)
                {
                    gridManager.BlockNode(_coordinates);
                }
            }
        }
        
    }


    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        if (clickedObject == gameObject)
        {
            if (CheckAllGridsWalkable())
            {
                bool isSuccessful = _placeableObject.CreateTower(_placeableObject,transform.position);
                if (isSuccessful)
                {
                    BlockAllNodes(_coordinates);
                    _pathFinder.NotifyReceivers();
                
                }
            }
           
        }
    }

    public bool CheckAllGridsWalkable()
    {
        foreach (var gridManager in _gridManagers)
        {
            if (!gridManager.GetNode(_coordinates).isWalkable && _pathFinder.WillBlockPath(_coordinates))
            {
                return false;
            }
        }
        return true;
    }

    public void BlockAllNodes(Vector2Int coordinates)
    {
        foreach (var gridManager in _gridManagers)
        {
            gridManager.BlockNode(coordinates);
        }
    }

}
