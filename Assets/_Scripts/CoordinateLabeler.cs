using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;


[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    private TextMeshPro _label; // Displays above tile 

    private Vector2Int _coordinates = new Vector2Int();

    private void Awake()
    {
        _label = GetComponent<TextMeshPro>();
        
        DisplayCoordinates();
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
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
