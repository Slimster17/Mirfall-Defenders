using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private Transform _targetEnemy;
    private Vector3 _lastEnemyPosition;
    private Unit _unit;
    private float _pathRecalculateDelay = 1f;
    private float _lastPathRecalculateTime;
    
    public Transform TargetEnemy{get{return _targetEnemy;} set{_targetEnemy = value;}}
    
    private void FindNearestEnemy()
    {
        if (_targetEnemy == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 50, _unit.UnitAttack.TargetMask);
            if (colliders.Length > 0)
            {
                _targetEnemy = colliders[0].transform;
                _lastEnemyPosition = _targetEnemy.position;

            }
            else
            {
                _targetEnemy = null;
            }
            
        }
    }
    
    private Vector2Int GetEnemyCoordinates()
    {
        if (_targetEnemy == null)
        {
            return _unit.GridManager.GetCoordinatesFromPosition(transform.position);
        }
    
        return _unit.GridManager.GetCoordinatesFromPosition(_targetEnemy.transform.position);
    }
    
    private void Awake()
    {
       
        _unit = GetComponent<Unit>();
    }
    
    
    private void Update()
    {
        if (_targetEnemy == null)
        {
            FindNearestEnemy();
            
        }
        else
        {
            if (Vector3.Distance(_targetEnemy.position, _lastEnemyPosition) > 4f &&
                Time.time - _lastPathRecalculateTime > _pathRecalculateDelay)
            {
                _lastEnemyPosition = _targetEnemy.position; // Update last position to avoid continuous recalculations
                _unit.PathFinder.DestinationCoordinates = GetEnemyCoordinates();
                _unit.UnitMover.RecalculatePath(false);
                _lastPathRecalculateTime = Time.time;
            }
        }

       
    }
}
