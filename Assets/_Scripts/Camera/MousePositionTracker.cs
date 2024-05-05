using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private InputReader _inputReader;
    private Vector2 _position;

    private SelectedTile _selectedTile;
    
    
    private void Awake()
    {
        _selectedTile = FindObjectOfType<SelectedTile>();
        _inputReader = FindObjectOfType<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.mousePositionEvent += OnCameraPosition ;
    }
    
    private void OnDisable()
    {
        _inputReader.mousePositionEvent -= OnCameraPosition;
    }
    
    void OnCameraPosition(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null && tile.IsPlaceable)
            {
                _selectedTile.transform.position = hit.collider.gameObject.transform.position + _offset;
            }
        }
    }
}
