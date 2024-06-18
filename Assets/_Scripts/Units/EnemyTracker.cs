using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [Tooltip("The target enemy to track.")]
    [SerializeField] private Transform _targetEnemy;
    
    private Vector3 _lastEnemyPosition; // Last enemy position to avoid continuous recalculations
    private Unit _unit; // The unit that this component is attached to
    private float _pathRecalculateDelay = 1f; // Delay before recalculating path
    private float _lastPathRecalculateTime; // Last time path was recalculated
    
    public Transform TargetEnemy{get{return _targetEnemy;} set{_targetEnemy = value;}} // Get/Set the target enemy
    
    private void FindNearestEnemy() // Find the nearest enemy to the unit within a certain radius
    {
        if (_targetEnemy == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 50, _unit.UnitAttack.TargetMask);
            if (colliders.Length > 0)
            {
                // Set the first found enemy as the target
                _targetEnemy = colliders[0].transform;
                _lastEnemyPosition = _targetEnemy.position;
            }
            else
            {
                _targetEnemy = null;
            }
        }
    }
    private Vector2Int GetEnemyCoordinates() // Get the coordinates of the enemy
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
            // If the enemy has moved significantly, recalculate the path
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
