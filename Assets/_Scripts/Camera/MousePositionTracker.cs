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
        if (_selectedTile == null)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null && tile.IsPlaceable && tile.TileLayerMask == ProjectLayers.Default)
            {
                _selectedTile.transform.position = hit.collider.gameObject.transform.position + _offset;
                _selectedTile.Selected = tile;
                // Trebuchet trebuchet = FindObjectOfType<Trebuchet>();
                // trebuchet.RotateToTarget(hit.collider.gameObject.transform.position);
            }
            else
            {
                _selectedTile.Selected = null;
                _selectedTile.transform.position = new Vector3(0, -10, 0);
            }
        }
        
        
    }
}