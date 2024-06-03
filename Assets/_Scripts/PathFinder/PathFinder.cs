using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int _startCoordinates;
    [SerializeField] private Vector2Int _destinationCoordinates;
    
    private Node _startNode;
    private Node _destinationNode;
    private Node _currentSearchNode;

    private Queue<Node> _frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>();
    private Vector2Int[] _directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    private GridManager _gridManager;
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();

    private Unit _unit;
    
    public Unit Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public Vector2Int StartCoordinates
    {
        get { return _startCoordinates; }
        set
        {
            _startCoordinates = value;
            _startNode = _grid[_startCoordinates];
        }
    }
    
    public Vector2Int DestinationCoordinates
    {
        get { return _destinationCoordinates; }
        set 
        {
            _destinationCoordinates = value;
            _destinationNode = _grid[_destinationCoordinates]; 
        }
    }
    
    public void CopyFrom(PathFinder other)
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
    
    private void Awake()
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

    // public PathFinder(GridManager gridManager)
    // {
    //     if (gridManager != null)
    //     {
    //         _gridManager = gridManager;
    //         _grid = _gridManager.Grid;
    //         _startNode = _grid[_startCoordinates];
    //         _destinationNode = _grid[_destinationCoordinates];
    //     }
    //     
    // }

   
    void Start()
    {
        GetNewPath();
    }

    // public List<Node> FindPath(GridManager gridManager, Vector2Int startCoordinates, Vector2Int destinationCoordinates)
    // {
    //     _gridManager = gridManager;
    //     _grid = _gridManager.Grid;
    //     StartCoordinates = startCoordinates;
    //     DestinationCoordinates = destinationCoordinates;
    //     
    //     return GetNewPath();
    // }
    
    private void ShuffleDirections()
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
    public List<Node> GetNewPath()
    {
        return GetNewPath(_startCoordinates);
    }
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        ShuffleDirections();
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
        
    private void ExploreNeighbors()
    {
        // Debug.Log($"{gameObject} ::::: {_currentSearchNode.coordinates} || {_grid} || {_unit}");

        if (_currentSearchNode == null || _grid == null || _unit == null)
        {
            return;
        }
        
        List<Node> neighbors = new List<Node>();

        foreach (var direction in _directions)
        {
            Vector2Int neighborCoord = _currentSearchNode.coordinates + direction;
            
            if (_grid.ContainsKey(neighborCoord))
            {
                neighbors.Add(_grid[neighborCoord]);
            }
        }

        foreach (var neighbor in neighbors)
        {
            if (!_reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable && 
                (LayersManager.HasLayer(neighbor.requiredLayer, _unit.UnitMask) 
                 || neighbor.requiredLayer == ProjectLayers.Default))
            {
                neighbor.connectedTo = _currentSearchNode;
                _reached.Add(neighbor.coordinates,neighbor);
                _frontier.Enqueue(neighbor);
            }
        }
        
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        _startNode.isWalkable = true;
        _destinationNode.isWalkable = true;
        
        _frontier.Clear();
        _reached.Clear();
        
        bool isRunning = true;
        _frontier.Enqueue(_grid[coordinates]);
        _reached.Add(coordinates, _grid[coordinates]);

        while (_frontier.Count > 0 && isRunning == true)
        {
            _currentSearchNode = _frontier.Dequeue();
            _currentSearchNode.isExplored = true;
            ExploreNeighbors();
            // Додайте перевірку шарів перед додаванням клітинки до шляху
                if (_currentSearchNode.coordinates == _destinationCoordinates)
                {
                    isRunning = false;
                }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = _destinationNode;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            bool previuosState = _grid[coordinates].isWalkable;
            
            _grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            _grid[coordinates].isWalkable = previuosState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
            
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath",false,SendMessageOptions.DontRequireReceiver);
    }
    
    public void ResetPath()
    {
        foreach (var node in _grid.Values)
        {
            if (node.isPath)
            {
                node.isPath = false;
            }
        }
    }
    
    public Node GetNode(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            return _grid[coordinates];
        }

        return null;
    }
    
}