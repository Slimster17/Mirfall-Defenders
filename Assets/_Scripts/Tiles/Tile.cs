using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower _placeableObject;
    [SerializeField] private bool _isPlaceable = true;
    [SerializeField] private ProjectLayers _tileLayerMask;
    private InputReader _inputReader;
    private GridManager _gridManager;
    private PathFinder[] _pathFinders;
    private Vector2Int _coordinates;
    
    public Vector2Int Coordinates => _coordinates;

    public bool IsPlaceable
    {
        get { return _isPlaceable; }
    }
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinders = FindObjectsOfType<PathFinder>();
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
            _gridManager.SetNodeLayerMask(_coordinates,_tileLayerMask);
        }
    }


    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     return;
        // }
        
        // if (clickedObject == gameObject && 
        //     _gridManager.GetNode(_coordinates).isWalkable && 
        //     CheckAllPathFindersBlocks())
        // {
        //     bool isSuccessful = _placeableObject.CreateTower(_placeableObject,transform.position);
        //     if (isSuccessful)
        //     {
        //         _gridManager.BlockNode(_coordinates);
        //         NotifyAllPathFinders();
        //     }
        // }
    }

    private bool CheckAllPathFindersBlocks()
    {
        foreach (var pathFinder in _pathFinders)
        {
            if (pathFinder.WillBlockPath(_coordinates))
            {
                return false;
            }
        }
        return true;
    }

    private void NotifyAllPathFinders()
    {
        foreach (var pathFinder in _pathFinders)
        {
            pathFinder.NotifyReceivers();
        }
    }
    

}