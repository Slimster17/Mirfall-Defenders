using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int _unityGridSize = 4;
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
    
    public int UnityGridSize
    {
        get { return _unityGridSize; }
    }
    public Dictionary<Vector2Int,Node> Grid
    {
        get { return _grid; }
    }

    private void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            return _grid[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            _grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach (var entry in _grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / UnityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / UnityGridSize);
        
        return coordinates;
    }
    
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * UnityGridSize;
        position.z = coordinates.y * UnityGridSize;
        
        return position;
    }
    
    private void CreateGrid()
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
}
