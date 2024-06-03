using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitAttack : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private float _angerRadius = 5f;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private UnitHealth _enemyUnitHealth;
    private Coroutine _attackRoutine;
    private Unit _unit;

    public LayerMask TargetMask
    {
        get { return _targetMask; }
    }
    public Transform EnemyTransform
    {
        get { return _enemyTransform; }
        set { _enemyTransform = value; }
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        _enemyTransform = null;
        _enemyUnitHealth = null;
    }

    public bool CheckEnemiesNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _angerRadius, _targetMask);
        if (colliders.Length > 0)
        {
            _enemyTransform = colliders[0].transform;
            _enemyUnitHealth = _enemyTransform.GetComponent<UnitHealth>();
            return true;
        }
        else
        {
            _enemyTransform = null;
            _enemyUnitHealth = null;
            return false;
        }
    }
    
    public void ManageAttackRoutine(bool start)
    {
        if (start && _attackRoutine == null)
        {
            if (CheckEnemiesNearby())
            {
                // Debug.Log($"Starting attack routine");
                _attackRoutine = StartCoroutine(AttackRoutine());
            }
        }
        else if (!start && _attackRoutine != null)
        {
            // Debug.Log($"Stopping attack routine");
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }
    }


    private IEnumerator AttackRoutine()
    {
        while (CheckEnemiesNearby())
        {
            if (_enemyTransform != null && !_unit.UnitHealth._isDead)
            {
                yield return StartCoroutine(Attack(_enemyTransform));
            }
            else
            {
                yield break; // No enemy found, exit the loop
            }
        }
    }

    private IEnumerator Attack(Transform enemyTransform)
    {
        if (_enemyTransform == null || _enemyUnitHealth == null || _unit.UnitHealth._isDead)
        {
            yield break;
        }
        transform.LookAt(enemyTransform);
        
        _animator.SetBool("Attacking", true);

        while (_enemyUnitHealth != null && !_unit.UnitHealth._isDead)
        {
            _enemyUnitHealth.ProcessHit();
            yield return new WaitForSeconds(1f); // Delay between hits
        }
        
        _animator.SetBool("Attacking", false);
    }
    
    private Transform GetClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _angerRadius, _targetMask);
        if (colliders.Length > 0)
        {
            return colliders[0].transform;
        }
        else
        {
            return null;
        }
    }

    
}