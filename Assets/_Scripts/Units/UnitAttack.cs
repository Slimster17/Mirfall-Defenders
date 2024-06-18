using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitAttack : MonoBehaviour
{
   
    [Tooltip("Mask to identify target layers.")]
    [SerializeField] private LayerMask _targetMask;
    
    [Tooltip("Radius within which the unit will detect and become angry at enemies.")]
    [SerializeField] private float _angerRadius = 5f;
    

    private Transform _enemyTransform; // Transform of the enemy unit
    private UnitHealth _enemyUnitHealth; // Health of the enemy unit
    private Coroutine _attackRoutine; // Coroutine to manage the attacking of the unit
    private bool _isAttacking; // Boolean to know if the unit is attacking or not
    
    // References
    private Animator _animator;
    private EnemyTracker _enemyTracker;
    private Unit _unit;

    // References
    public LayerMask TargetMask 
    {
        get { return _targetMask; }
    }
    public Transform EnemyTransform
    {
        get { return _enemyTransform; }
        set { _enemyTransform = value; }
    }
    public bool IsAttacking { get { return _isAttacking; } }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _unit = GetComponent<Unit>();
        _enemyTracker = GetComponent<EnemyTracker>();
    }
    private void OnEnable()
    {
        _enemyTransform = null;
        _enemyUnitHealth = null;
    }
    private IEnumerator AttackRoutine() // Attack routine
    {
        while (CheckEnemiesNearby())
        {
            if (_enemyTransform != null && !_unit.UnitHealth._isDead)
            {
                _isAttacking = true;
                yield return StartCoroutine(Attack(_enemyTransform));
            }
            else
            {
                _isAttacking = false;
                yield break; // No enemy found, exit the loop
            }
        }
    }
    private IEnumerator Attack(Transform enemyTransform) // Attack the enemy unit
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
        
        if (_enemyTracker != null)
        {
            _enemyTracker.TargetEnemy = null;
        }
        
        _isAttacking = false;
    }
    public bool CheckEnemiesNearby() // Returns true if there is an enemy nearby
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
    public void ManageAttackRoutine(bool start) // Start or stop the attack routine
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
}