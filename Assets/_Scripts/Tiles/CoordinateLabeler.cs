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
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _blockedColor = Color.gray;
    [SerializeField] private Color _exploredColor = Color.magenta;
    [SerializeField] private Color _pathColor = Color.red;
    
    private TextMeshPro _label; // Displays above tile 

    private Vector2Int _coordinates = new Vector2Int();
    private GridManager _gridManager;

    
    private InputReader _inputReader;

    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
        _label = GetComponent<TextMeshPro>();
        _gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
        
        _label.enabled = false;
    }
    private void OnEnable()
    {
        _inputReader.toggleEvent += ToggleLabels;
    }

    private void OnDisable()
    {
        _inputReader.toggleEvent -= ToggleLabels;
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            SetLabelColor();
            _label.enabled = true;
        }

      
        DisplayCoordinates();
        SetLabelColor();
   
    }

    private void SetLabelColor()
    {
        if (_gridManager == null)
        {
            return;
        }

        Node node = _gridManager.GetNode(_coordinates);

        if (node == null)
        {
            return;
        }
        
        if (!node.isWalkable)
        {
            _label.color = _blockedColor;
        }
        else if (node.isPath)
        {
            _label.color = _pathColor;
        }
        else if (node.isExplored)
        {
            _label.color = _exploredColor;
        }
        else
        {
            _label.color = _defaultColor;
        }
       
    }

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
