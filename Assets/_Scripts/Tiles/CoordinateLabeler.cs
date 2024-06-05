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
    [SerializeField] private Color _defaultColor = Color.blue;
    [SerializeField] private Color _blockedColor = Color.black;
    [SerializeField] private Color _exploredColor = Color.magenta;
    [SerializeField] private Color _pathColor = Color.red;
    
    private TextMeshPro _label; // Displays above tile 

    private Vector2Int _coordinates = new Vector2Int();
    private GridManager _gridManager;
    // private PathFinder _pathFinder;
    private Tile _tile;
    
    private InputReader _inputReader;

    private UnitSelector _unitSelector;
    
    private UnitMover _selectedCrusaderMover;
    
    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
        _label = GetComponent<TextMeshPro>();
        _gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
        _tile = GetComponentInParent<Tile>();
        _label.enabled = false;
        _unitSelector = FindObjectOfType<UnitSelector>();
    }
    private void OnEnable()
    {
        _inputReader.toggleEvent += ToggleLabels;
        if (_unitSelector != null)
        {
            _unitSelector.onUnitSelected.AddListener(OnUnitSelected);
        }
    }

    private void OnDisable()
    {
        _inputReader.toggleEvent -= ToggleLabels;
        if (_unitSelector != null)
        {
            _unitSelector.onUnitSelected.RemoveListener(OnUnitSelected);
        }
        
    }


    void Update()
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
    
    private void OnUnitSelected(UnitMover crusaderMover)
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

    private void SetColorLabels()
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

    private void ToggleLabels()
    {
        _label.enabled = !_label.IsActive();
    }

    private void DisplayCoordinates()
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
    
    private void UpdateObjectName()
    {
        transform.parent.name = _label.text;
    }
}
