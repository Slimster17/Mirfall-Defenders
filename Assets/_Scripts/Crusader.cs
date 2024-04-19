using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusader : MonoBehaviour
{
    // [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    // [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;
    [SerializeField] private TestingPathChecker _targetEnemyMover;
    
    [SerializeField] private List<Node> _path = new List<Node>();
    private PathFinder _pathFinder;
    private GridManager _gridManager;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = GetComponent<PathFinder>();
        // FindNearestEnemyMover();
        
    }
    
    private void OnEnable()
    {
        FindNearestEnemyMover();
        if (_targetEnemyMover != null)
        {
           _targetEnemyMover.onPossitionChanged.AddListener(RecalculatePath);
        }
    }

    private void OnDisable()
    {
        if (_targetEnemyMover != null)
        {
            _targetEnemyMover.onPossitionChanged.RemoveListener(RecalculatePath);
        }
    }

    private void Start()
    {
        // FindNearestEnemyMover();
        
        // RecalculatePath(false);
    }

    private void Update()
    {
        if (_targetEnemyMover == null)
        {
            FindNearestEnemyMover();
        }
       // Debug.Log(GetEnemyCoordinates());
      
        transform.LookAt(_targetEnemyMover.transform);
    }
    
    private IEnumerator RecalculatePathCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); 
            RecalculatePath(true);
        }
    }
    

    private void FindNearestEnemyMover()
    {
        TestingPathChecker[] enemyMovers = FindObjectsOfType<TestingPathChecker>();
        float closestDistance = Mathf.Infinity;

        foreach (var enemyMover in enemyMovers)
        {
            float distance = Vector3.Distance(transform.position, enemyMover.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _targetEnemyMover = enemyMover;
            }
        }
    }

    private Vector2Int GetEnemyCoordinates()
    {
        return _gridManager.GetCoordinatesFromPosition(_targetEnemyMover.transform.position);
    }
    
    
    
    private void RecalculatePath(bool resetPath)
    {
        FindNearestEnemyMover();
        Debug.Log("Recalculating");
        Vector2Int coordinates = new Vector2Int();
       
        _pathFinder.DestinationCoordinates = _gridManager.GetCoordinatesFromPosition(_targetEnemyMover.transform.position);
        
        if (resetPath)
        {
            coordinates = _pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        _path = _pathFinder.GetNewPath(coordinates);
        
        // _pathFinder.ResetPath(); 
        // // StopAllCoroutines();
        // _path.Clear();
        // _path = _pathFinder.GetNewPath(coordinates);
        // Debug.Log(_path);
        // StartCoroutine(FollowPath());
    }
    
}
