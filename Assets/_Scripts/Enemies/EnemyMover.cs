using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Unit))]
public class EnemyMover : MonoBehaviour
{
     
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;
    
    private List<Node> _path = new List<Node>();
    
    private Animator animator;
    private Unit _unit;
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    
    public UnityEvent<bool> onPossitionChanged = new UnityEvent<bool>();
    private Vector3 _lastPosition;
    private UnitAttack _unitAttack;
    
    // private Transform _enemyTransform;
   
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

 
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = GetComponentInParent<PathFinder>();
        _unit = GetComponent<Unit>();
        _unitAttack = GetComponent<UnitAttack>();
    }
    
    private void OnEnable()
    {
        _pathFinder.Unit = _unit;
        ReturnToStart();
        RecalculatePath(true);
        _lastPosition = transform.position;
    }

    private void OnValidate()
    {
        if (IsFollowingPath)
        {
           ResumeFollowingPath();
        }
        else
        {
            StopFollowingPath();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _lastPosition) > 3f)
        {
            _lastPosition = transform.position;
            onPossitionChanged.Invoke(false);
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
        
        // Collider[] colliders = Physics.OverlapSphere(transform.position, 6, LayerMask.GetMask("Friendly"));
        // if (colliders.Length > 0)
        // {
        //     _enemyTransform = colliders[0].transform;
        //     IsFollowingPath = false;
        // }
        // else
        // {
        //     IsFollowingPath = true;
        // }
        
        
        // Collider[] colliders = Physics.OverlapSphere(transform.position, 6, LayerMask.GetMask("Friendly"));
        // if (colliders != null)
        // {
        //     foreach (var collider in colliders)
        //     {
        //         // if (collider.CompareTag("Friendly"))
        //         // {
        //             _enemyTransform = collider.transform;
        //             // onPossitionChanged.Invoke(false); // Можливо, потрібно оновити позицію ворога
        //             break;
        //         // }
        //     }
        //     AttackEnemy();
        // }
        // else
        // {
        //     isAttacking = false;
        // }
    }

    private void RecalculatePath(bool resetPath)
    {
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
            for (int i = 1; i < _path.Count - 1; i++)
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