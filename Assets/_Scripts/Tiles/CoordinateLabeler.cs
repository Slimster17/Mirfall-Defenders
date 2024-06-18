using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[RequireComponent(typeof(TextMeshPro))]
[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [Tooltip("Default color for the label.")][SerializeField] private Color _defaultColor = Color.blue;
    [Tooltip("Color for blocked tiles.")][SerializeField] private Color _blockedColor = Color.black;
    [Tooltip("Color for explored tiles.")][SerializeField] private Color _exploredColor = Color.magenta;
    [Tooltip("Color for path tiles.")][SerializeField] private Color _pathColor = Color.red;
    
    private TextMeshPro _label; // Displays above tile 
    private Vector2Int _coordinates = new Vector2Int(); // Coordinates of the tile
    
    private GridManager _gridManager; // Reference to the grid manager
    private Tile _tile; // Reference to the tile
    private InputReader _inputReader; // Reference to the input reader
    private UnitSelector _unitSelector; // Reference to the unit selector
    private UnitMover _selectedCrusaderMover; // Reference to the selected crusader mover
    
    private void Awake() // Called when the script instance is being loaded
    {
        _inputReader = FindObjectOfType<InputReader>();
        _label = GetComponent<TextMeshPro>();
        _gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
        _tile = GetComponentInParent<Tile>();
        _label.enabled = false;
        _unitSelector = FindObjectOfType<UnitSelector>();
    }
    private void OnEnable() // Called when the object becomes enabled and active
    {
        _inputReader.toggleEvent += ToggleLabels;
        if (_unitSelector != null)
        {
            _unitSelector.onUnitSelected.AddListener(OnUnitSelected);
        }
    }
    private void OnDisable() // Called when the object becomes disabled or inactive
    {
        _inputReader.toggleEvent -= ToggleLabels;
        if (_unitSelector != null)
        {
            _unitSelector.onUnitSelected.RemoveListener(OnUnitSelected);
        }
        
    }
    void Update() // Called once per frame
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            // SetLabelColor(_pathFinder);
            _label.enabled = true;
        }
        
        if (_label.IsActive())
        {
            DisplayCoordinates();
            if (_selectedCrusaderMover != null)
            {
                // SetColorLabels();
            }
        }
    }
    private void OnUnitSelected(UnitMover crusaderMover) // Called when a unit is selected
    {
        if (_selectedCrusaderMover != null)
        {
            _selectedCrusaderMover.onPathRecalculated -= SetColorLabels; // Відписка від попереднього об'єкту
        }
    
        _selectedCrusaderMover = crusaderMover; 
        if (_selectedCrusaderMover != null)
        {
            _selectedCrusaderMover.onPathRecalculated += SetColorLabels; // Підписка на новий об'єкт
        }
    }

    private void SetColorLabels() // Sets the color of the labels based on the node status
    {
        if (!_label.IsActive())
        {
            return;
        }
        Debug.Log("Setting ColorS");
        if (_gridManager == null)
        {
            Debug.LogError("PathFinder is null.");
            return;
        }

        Node currentNode;
        bool hasMatchingNode = false;
        foreach (var node in _selectedCrusaderMover.Path)     
        {
            if (node.coordinates == _coordinates)
            {
                hasMatchingNode = true;
                currentNode = node;
                break;  // Exit the loop early since we found a matching node
            }
        }

        if (hasMatchingNode)
        {
            _label.color = _pathColor;
        }
        else if(!_tile.IsPlaceable)
        {
            _label.color = _blockedColor;
        }
        else
        {
            _label.color = _defaultColor;
        }
    }
    private void ToggleLabels() // Toggles the visibility of the labels
    {
        _label.enabled = !_label.IsActive();
    }
    private void DisplayCoordinates() // Displays the coordinates on the label
    {
        if (_gridManager == null)
        {
            return;
        }
        
        var parent = transform.parent;
        _coordinates.x = Mathf.RoundToInt(parent.position.x / _gridManager.UnityGridSize);
        _coordinates.y = Mathf.RoundToInt(parent.position.z / _gridManager.UnityGridSize);

        _label.text = $"{_coordinates.x}, {_coordinates.y}";
    }
    private void UpdateObjectName() // Updates the object name to the current coordinates
    {
        transform.parent.name = _label.text;
    }
    
    // private void SetLabelColor()
    // {
    //     if (_gridManager == null)
    //     {
    //         return;
    //     }
    //     
    //     if (_tile.IsPlaceable)
    //     {
    //         _label.color = Color.black;
    //     }
    //     else
    //     {
    //         _label.color = Color.gray;
    //     }
    //     Node node = _gridManager.GetNode(_coordinates);
    //     
    //     if (node == null)
    //     {
    //         return;
    //     }
    //     
    //     if (!node.isWalkable)
    //     {
    //         _label.color = _blockedColor;
    //     }
    //     else if (node.isPath)
    //     {
    //         _label.color = _pathColor;
    //     }
    //     else if (node.isExplored)
    //     {
    //         _label.color = _exploredColor;
    //     }
    //     else
    //     {
    //         _label.color = _defaultColor;
    //     }
    //     
    // }
}
