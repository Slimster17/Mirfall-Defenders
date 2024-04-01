using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;


[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _blockedColor = Color.gray;
    
    private TextMeshPro _label; // Displays above tile 

    private Vector2Int _coordinates = new Vector2Int();
    private Waypoint _waypoint;
    
    private InputReader _inputReader;

    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
        _label = GetComponent<TextMeshPro>();
        _waypoint = GetComponentInParent<Waypoint>();
        
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
        }

        ColorCoordinates();
    }

    private void ColorCoordinates()
    {
        if (_waypoint.IsPlaceable)
        {
            _label.color = _defaultColor;
        }
        else
        {
            _label.color = _blockedColor;
        }
    }

    private void ToggleLabels()
    {
        _label.enabled = !_label.IsActive();
    }

    private void DisplayCoordinates()
    {
        var parent = transform.parent;
        
        _coordinates.x = Mathf.RoundToInt(parent.position.x / EditorSnapSettings.move.x);
        _coordinates.y = Mathf.RoundToInt(parent.position.z / EditorSnapSettings.move.z);

        _label.text = $"{_coordinates.x}, {_coordinates.y}";
    }


    private void UpdateObjectName()
    {
        transform.parent.name = _label.text;
    }
}
