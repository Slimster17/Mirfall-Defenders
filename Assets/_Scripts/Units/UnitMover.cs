using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Unit))]
public class UnitMover : MonoBehaviour
{
     
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;
    [SerializeField] private bool _isFollowingPath = true;
    [SerializeField] private List<Node> _path = new List<Node>();
    
    private Animator animator;
    private Unit _unit;
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    private UnitAttack _unitAttack;
    private Vector3 _lastPosition;
    
    // private Transform _enemyTransform;
   
   
    private bool _isAttacking;
    
    public Action onPathRecalculated;
    public List<Node> Path { get { return _path; } }
    private bool _isListChanged = false;

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

 
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _gridManager = FindObjectOfType<GridManager>();
        _unit = GetComponent<Unit>();
        _pathFinder = _unit.PathFinder;
        _unitAttack = GetComponent<UnitAttack>();
    }
    
    private void OnEnable()
    {
        IsFollowingPath = true;
        _lastPosition = transform.position;
        // _pathFinder.Unit = _unit;
        ReturnToStart();
        RecalculatePath(true);
    }

    // private void OnValidate()
    // {
    //     if (IsFollowingPath)
    //     {
    //        ResumeFollowingPath();
    //     }
    //     else
    //     {
    //         StopFollowingPath();
    //     }
    // }

    private void Update()
    {
        if (_isListChanged) /////////////
        {
            onPathRecalculated?.Invoke();
            _isListChanged = false;
        }
        
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
    }

    public void RecalculatePath(bool resetPath)
    {
        // Debug.Log($"{gameObject.name} recalculating path");
        if (_gridManager == null)
        {
            return;
        }
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = _pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        StopAllCoroutines();
        _path.Clear();
        _path = _pathFinder.GetNewPath(coordinates);
        _isListChanged = true;
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathFinder.StartCoordinates);
    }

    public void FinishPath()
    {
        _unit.WithdrawGold();
        gameObject.SetActive(false);
    }
   
    IEnumerator FollowPath()
    {
        if (_path != null)
        {
            for (int i = 0; i < _path.Count - 1; i++)
            {
                Node currentTile = _path[i];
                Node nextTile = _path[i + 1];

                // if (Vector3.Distance(_gridManager.GetPositionFromCoordinates(currentTile.coordinates),
                //         _gridManager.GetPositionFromCoordinates(nextTile.coordinates)) < 0.01f)
                //     continue; // Skip waypoints that are too close

                Vector3 startPosition = transform.position;
                Vector3 endPosition = _gridManager.GetPositionFromCoordinates(nextTile.coordinates);

                Quaternion startRotation = transform.rotation;
                Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

                float travelPercent = 0f;
                while (travelPercent < 1 && _isFollowingPath)
                {
                    travelPercent += Time.deltaTime * _movementSpeed;

                    float rotationTravelPercent = Mathf.Min(travelPercent * _rotationSpeed, 1f);
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationTravelPercent);

                    animator.SetBool("Walking", true);
                    // animator.SetBool("Attacking", false);

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

        // animator.SetBool("Attacking", true);
        animator.SetBool("Walking", false);

        // FinishPath();
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