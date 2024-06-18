using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("Size of the grid in terms of the number of nodes.")]
    [SerializeField] private Vector2Int _gridSize;
    
    [Tooltip("Size of each grid unit in Unity units.")]
    [SerializeField] private int _unityGridSize = 4;
    
    // Dictionary to store the grid nodes with their coordinates as keys
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
    
    public Vector2Int GridSize // Property to get the grid size
    {
        get { return _gridSize; }
    }
    public int UnityGridSize // Property to get the Unity grid size
    {
        get { return _unityGridSize; }
    }
    public Dictionary<Vector2Int,Node> Grid  // Property to get the grid dictionary
    {
        get { return _grid; }
    }
    private void Awake()  // Awake is called when the script instance is being loaded
    {
        CreateGrid();
    }
    private void CreateGrid() // Method to create the grid with nodes
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                // Debug.Log("Adding node at coordinates: " + coordinates);
                _grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
    public Node GetNode(Vector2Int coordinates)  // Method to get a node from the grid using its coordinates
    {
        if (_grid.ContainsKey(coordinates))
        {
            return _grid[coordinates];
        }

        return null;
    }
    public void BlockNode(Vector2Int coordinates) // Method to block a node in the grid
    {
        if (_grid.ContainsKey(coordinates))
        {
            _grid[coordinates].isWalkable = false;
        }
    }
    public void SetNodeLayerMask(Vector2Int coordinates,ProjectLayers layer)  // Method to set the layer mask of a node in the grid
    {
        if (_grid.ContainsKey(coordinates))
        {
            _grid[coordinates].requiredLayer = layer;
        }
    }
    public void ResetNodes() // Method to reset all nodes in the grid
    {
        foreach (var entry in _grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    public Vector2Int GetCoordinatesFromPosition(Vector3 position) // Method to get grid coordinates from a Unity world position
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / UnityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / UnityGridSize);
        
        return coordinates;
    }
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates) // Method to get Unity world position from grid coordinates
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * UnityGridSize;
        position.z = coordinates.y * UnityGridSize;
        
        return position;
    }
}
