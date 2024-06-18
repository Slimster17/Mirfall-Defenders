using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    [Tooltip("Offset to apply to the selected tile's position.")]
    [SerializeField] private Vector3 _offset;
    
    private Vector2 _position; // Current mouse position
    
    private InputReader _inputReader; // Reference to the InputReader
    private SelectedTile _selectedTile;  // Reference to the currently selected tile
    
    private void Awake()  // Awake is called when the script instance is being loaded
    {
        _selectedTile = FindObjectOfType<SelectedTile>();
        _inputReader = FindObjectOfType<InputReader>();
    }
    private void OnEnable()  // OnEnable is called when the object becomes enabled and active
    {
        // Subscribe to the mousePositionEvent from the InputReader
        _inputReader.mousePositionEvent += OnCameraPosition ;
    }
    private void OnDisable() // OnDisable is called when the object becomes disabled or inactive
    {
        // Unsubscribe from the mousePositionEvent to avoid memory leaks
        _inputReader.mousePositionEvent -= OnCameraPosition;
    }
    private void OnCameraPosition(Vector2 position) // Method to handle the mouse position event
    {
        if (_selectedTile == null) { return; } // If there is no selected tile, return early
        
        // Perform a raycast to check for hits
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            
            // If the hit object is a placeable tile on the default layer
            if (tile != null && tile.IsPlaceable && tile.TileLayerMask == ProjectLayers.Default)
            {
                // Move the selected tile to the hit position with an offset
                _selectedTile.transform.position = hit.collider.gameObject.transform.position + _offset;
                _selectedTile.Selected = tile;
            }
            else // If the hit object is not a placeable tile, deselect the tile and move it off-screen
            {
                _selectedTile.Selected = null;
                _selectedTile.transform.position = new Vector3(0, -10, 0);
            }
        }
        
        
    }
}