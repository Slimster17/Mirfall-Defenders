using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class PathFinder : MonoBehaviour
{
    [Tooltip("Start coordinates for pathfinding")] 
    [SerializeField] private Vector2Int _startCoordinates;
    
    [Tooltip("Destination coordinates for pathfinding")]
    [SerializeField] private Vector2Int _destinationCoordinates;
    
    private Node _startNode;  // Node representing the start position
    private Node _destinationNode; // Node representing the destination position
    private Node _currentSearchNode; // Node currently being explored

    private Queue<Node> _frontier = new Queue<Node>(); // Queue for nodes to be explored
    private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>(); // Dictionary of reached nodes
    private Vector2Int[] _directions = 
        { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down }; // Possible movement directions

    private GridManager _gridManager; // Reference to the grid manager
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>(); // Dictionary representing the grid

    private Unit _unit; // Reference to the unit that will use this pathfinder
    
    public Unit Unit // Property for unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public Vector2Int StartCoordinates // Property for start coordinates
    {
        get { return _startCoordinates; }
        set
        {
            _startCoordinates = value;
            _startNode = _grid[_startCoordinates];
        }
    }
    
    public Vector2Int DestinationCoordinates // Property for destination coordinates
    {
        get { return _destinationCoordinates; }
        set 
        {
            _destinationCoordinates = value;
            _destinationNode = _grid[_destinationCoordinates]; 
        }
    }
    
    public void CopyFrom(PathFinder other) // Copies the state from another PathFinder instance
    {
        if (other == null)
        {
            Debug.LogError("Cannot copy from a null PathFinder object.");
            return;
        }

        _startCoordinates = other._startCoordinates;
        _destinationCoordinates = other._destinationCoordinates;
        
        _startNode = other._startNode;
        _destinationNode = other._destinationNode;
        _currentSearchNode = other._currentSearchNode;

        _frontier = new Queue<Node>(other._frontier);
        _reached = new Dictionary<Vector2Int, Node>(other._reached);
        _directions = (Vector2Int[])other._directions.Clone();

        _gridManager = other._gridManager;
        _grid = new Dictionary<Vector2Int, Node>(other._grid);

        _unit = other._unit;
    }
    
    private void Awake() // Awake is called when the script instance is being loaded
    {
        _gridManager = FindObjectOfType<GridManager>();
        if (_gridManager != null)
        {
            _grid = _gridManager.Grid;
            // Debug.Log(_startCoordinates);
            _startNode = _grid[_startCoordinates];
            _destinationNode = _grid[_destinationCoordinates];
        }
    }
   
    void Start() // Start is called on the frame when a script is enabled
    {
        GetNewPath();
    }
    
    private void ShuffleDirections() // Shuffle the directions array for random exploration
    {
        Random random = new Random();
        int n = _directions.Length;
        while (n > 1)
        {
            int k = random.Next(n--);
            Vector2Int temp = _directions[n];
            _directions[n] = _directions[k];
            _directions[k] = temp;
        }
    }
    private void ExploreNeighbors() // Explore neighboring nodes
    {
        // Debug.Log($"{gameObject} ::::: {_currentSearchNode.coordinates} || {_grid} || {_unit}");

        if (_currentSearchNode == null || _grid == null || _unit == null)
        { return; }
        
        List<Node> neighbors = new List<Node>();
        
        // Iterate through each possible direction
        foreach (var direction in _directions) 
        {
            // Calculate neighbor coordinates
            Vector2Int neighborCoord = _currentSearchNode.coordinates + direction;
            
            // Check if the neighbor exists in the grid
            if (_grid.ContainsKey(neighborCoord))
            {
                neighbors.Add(_grid[neighborCoord]);
            }
        }

        // Process each neighbor
        foreach (var neighbor in neighbors)
        {
            // Check if the neighbor has not been reached, is walkable, and the unit can move through the required layer
            if (!_reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable && 
                (LayersManager.HasLayer(neighbor.requiredLayer, _unit.UnitMask) 
                 || neighbor.requiredLayer == ProjectLayers.Default))
            {
                // Set the current search node as the connected node
                neighbor.connectedTo = _currentSearchNode;
                // Add neighbor to reached nodes
                _reached.Add(neighbor.coordinates,neighbor);
                // Enqueue the neighbor for further exploration
                _frontier.Enqueue(neighbor);
            }
        }
        
    }
    void BreadthFirstSearch(Vector2Int coordinates) // Perform a breadth-first search
    {
        _startNode.isWalkable = true;
        _destinationNode.isWalkable = true;
        
        _frontier.Clear();
        _reached.Clear();
        
        bool isRunning = true;
        // Enqueue the starting node
        _frontier.Enqueue(_grid[coordinates]);
        // Add the starting node to reached nodes
        _reached.Add(coordinates, _grid[coordinates]);

        while (_frontier.Count > 0 && isRunning == true)
        {
            // Dequeue the next node to explore
            _currentSearchNode = _frontier.Dequeue();
            _currentSearchNode.isExplored = true;
            // Explore the neighbors of the current node
            ExploreNeighbors();
            // Check if the destination node has been reached
                if (_currentSearchNode.coordinates == _destinationCoordinates)
                {
                    isRunning = false;
                }
        }
    }
    List<Node> BuildPath() // Build the path from destination to start node
    {
        List<Node> path = new List<Node>();
        Node currentNode = _destinationNode;
        
        // Add the destination node to the path
        path.Add(currentNode);
        currentNode.isPath = true;

        // Traverse back from the destination to the start node
        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        // Reverse the path to start from the start node
        path.Reverse();

        return path;
    }
    public List<Node> GetNewPath() // Get a new path from the start coordinates
    {
        return GetNewPath(_startCoordinates);
    }
    public List<Node> GetNewPath(Vector2Int coordinates) // Get a new path from specified coordinates
    {
        ShuffleDirections();
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
    public bool WillBlockPath(Vector2Int coordinates) // Check if blocking a node will block the path
    {
        if (_grid.ContainsKey(coordinates))
        {
            bool previuosState = _grid[coordinates].isWalkable;
            
            // Temporarily block the node
            _grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            // Restore the node's walkable state
            _grid[coordinates].isWalkable = previuosState;

            // If the new path is invalid, blocking the node will block the path
            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }
    public void NotifyReceivers() // Notify other components to recalculate paths
    {
        BroadcastMessage("RecalculatePath",false,SendMessageOptions.DontRequireReceiver);
    }
}