using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CrusaderMover : MonoBehaviour
{
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;
    [SerializeField] private EnemyMover _targetEnemy;
    
    [SerializeField] private List<Node> _path = new List<Node>();
    private PathFinder _pathFinder;
    private GridManager _gridManager;
    private UnitHealth _unitHealth;
    private Animator _animator;
    private UnitAttack _unitAttack;
    
    [SerializeField] private bool _isFollowingPath = true;
    private bool _isAttacking;
    
    public bool IsFollowingPath
    {
        get { return _isFollowingPath; }
        set
        {
            if (value == _isFollowingPath)
                return;

            _isFollowingPath = value;

            if (_isFollowingPath)
            {
                ResumeFollowingPath();
            }
            else
            {
                StopFollowingPath();
            }
        }
    }

    public Action onPathRecalculated;
    public List<Node> Path
    {
        get { return _path; }
    }

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = GetComponent<PathFinder>();
        _unitHealth = GetComponent<UnitHealth>();
        _animator = GetComponentInChildren<Animator>();
        _unitAttack = GetComponent<UnitAttack>();
        // FindNearestEnemyMover();

    }
    
    private void OnEnable()
    {
        
        _unitHealth.SetupHealthBar();
        FindNearestEnemyMover();
        if (_targetEnemy != null)
        {
            RecalculatePath(false);
           _targetEnemy.onPossitionChanged.AddListener(RecalculatePath);
        }
    }

    private void OnDisable()
    {
        if (_targetEnemy != null)
        {
            _targetEnemy.onPossitionChanged.RemoveListener(RecalculatePath);
        }
    }

    private void Start()
    {
        // FindNearestEnemyMover();
        
        // RecalculatePath(false);
       
    }
    // private void OnValidate()
    // {
    //     if (IsFollowingPath)
    //     {
    //         ResumeFollowingPath();
    //     }
    //     else
    //     {
    //         StopFollowingPath();
    //     }
    // }

    private void Update()
    {
        // if (_targetEnemy == null)
        // {
            FindNearestEnemyMover();
        // }
    
        if (_unitAttack.CheckEnemiesNearby() && !_isAttacking)
        {
            IsFollowingPath = false;
            _isAttacking = true;
            _unitAttack.ManageAttackRoutine(true);
        }
        else if (!_unitAttack.CheckEnemiesNearby() && _isAttacking)
        {
            IsFollowingPath = true;
            _isAttacking = false;
            _unitAttack.ManageAttackRoutine(false);
        }
        
        // Debug.Log($"Crusade healthPoint {_unitHealth.CurrentHitPoint}");
       // Debug.Log(GetEnemyCoordinates());
      
        // transform.LookAt(_targetEnemy.transform);
    }
    

    // private void FindNearestEnemyMover()
    // {
    //     if (_targetEnemy == null)
    //     {
    //         if (_unitAttack.CheckEnemiesNearby())
    //         {
    //             _targetEnemy = _unitAttack.EnemyTransform.GetComponent<EnemyMover>();
    //         }
    //         
    //     }
    // }
    

    private void FindNearestEnemyMover()
    {
        EnemyMover[] enemyMovers = FindObjectsOfType<EnemyMover>();
        float closestDistance = Mathf.Infinity;
    
        foreach (var enemyMover in enemyMovers)
        {
            float distance = Vector3.Distance(transform.position, enemyMover.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _targetEnemy = enemyMover;
            }
        }
    }

    private Vector2Int GetEnemyCoordinates()
    {
        return _gridManager.GetCoordinatesFromPosition(_targetEnemy.transform.position);
    }
    
    
    
    private void RecalculatePath(bool resetPath)
    {
        FindNearestEnemyMover();
        Debug.Log("Recalculating");
        Vector2Int coordinates = new Vector2Int();

        _pathFinder.DestinationCoordinates = GetEnemyCoordinates();
        
        if (resetPath)
        {
            coordinates = _pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        _path = _pathFinder.GetNewPath(coordinates);
        

        StopAllCoroutines();
        _path.Clear();
        _path = _pathFinder.GetNewPath(coordinates);
        onPathRecalculated?.Invoke();
        StartCoroutine(FollowPath());
    }
    
    IEnumerator FollowPath()
    {
        if (_path != null)
        {
            for (int i = 1; i < _path.Count - 2; i++)
            {
                Node currentTile = _path[i];
                Node nextTile = _path[i + 1];

                if (Vector3.Distance(_gridManager.GetPositionFromCoordinates(currentTile.coordinates),
                        _gridManager.GetPositionFromCoordinates(nextTile.coordinates)) < 0.01f)
                    continue; // Skip waypoints that are too close

                Vector3 startPosition = transform.position;
                Vector3 endPosition = _gridManager.GetPositionFromCoordinates(nextTile.coordinates);

                Quaternion startRotation = transform.rotation;
                Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

                float travelPercent = 0f;
                while (travelPercent < 1)
                {
                    travelPercent += Time.deltaTime * _movementSpeed;
    
                    float rotationTravelPercent = Mathf.Min(travelPercent * _rotationSpeed, 1f);
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationTravelPercent);
    
                    _animator.SetBool("Walking", true);
                    // _animator.SetBool("Attacking", false);
    
                    yield return new WaitForEndOfFrame();
                }
                if (!_isFollowingPath)
                {
                    // If not following path, break out of the loop
                    break;
                }
               
            }
        }
        else
        {
            yield return null;
        }

      
        // FinishPath();
      
    }
    
    void FinishPath()
    {
        // transform.LookAt(_targetEnemy.transform);
        // _animator.SetBool("Attacking", true);
        // _animator.SetBool("Walking", false);
        // // _targetEnemy.GetComponent<EnemyMover>().StopFollowingPath();
        // _targetEnemy.GetComponent<UnitHealth>().ProcessHit();
        // _unitAttack.ManageAttackRoutine(true);
    }
    
    public void StopFollowingPath()
    {
        // transform.LookAt(_enemyTransform);
    }

 
    public void ResumeFollowingPath()
    {
        RecalculatePath(false);
    }
}



    

