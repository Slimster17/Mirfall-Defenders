using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [Tooltip("The tower that can be placed on this tile")]
    [SerializeField] private Tower _placeableObject;
    
    [Tooltip("Flag indicating if the tile can have a tower placed on it")]
    [SerializeField] private bool _isPlaceable = true;
    
    [Tooltip("The layer mask for the tile")]
    [SerializeField] private ProjectLayers _tileLayerMask;
    
    private InputReader _inputReader; // Reference to the input reader
    private GridManager _gridManager; // Reference to the grid manager
    private Vector2Int _coordinates; // Coordinates of the tile in the grid
    
    public Vector2Int Coordinates => _coordinates; // Property to get the tile's coordinates

    public bool IsPlaceable  // Property to get the tile's layer mask
    {
        get { return _isPlaceable; }
    }
    public ProjectLayers TileLayerMask => _tileLayerMask; // Property to get the tile's layer mask
   
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
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
            // Get the coordinates of the tile from its position
            _coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
            if (!IsPlaceable)
            {
                // Block the node in the grid manager if the tile is not placeable
                _gridManager.BlockNode(_coordinates);
            }
            // Set the layer mask of the node in the grid manager
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

}