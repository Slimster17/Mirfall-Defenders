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
    private Transform _enemyTransform;
    private UnitHealth _enemyUnitHealth;
    private Coroutine _attackRoutine;
    
    public Transform EnemyTransform
    {
        get { return _enemyTransform; }
        set { _enemyTransform = value; }
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public bool CheckEnemiesNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _angerRadius, _targetMask);
        if (colliders.Length > 0)
        {
            _enemyTransform = colliders[0].transform;
            return true;
        }
        else
        {
            _enemyTransform = null;
            return false;
        }
    }
    
    public void ManageAttackRoutine(bool start)
    {
        // Debug.Log(_attackRoutine);
        if (start && _attackRoutine == null)
        {
            Debug.Log($"Starting attack routine");
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
        _attackRoutine = null;
       
    }

    private IEnumerator AttackRoutine()
    {

            if (_enemyTransform != null)
            {
                yield return StartCoroutine(Attack(_enemyTransform));
            }
            else
            {
                _attackRoutine = null; // Reset the attack routine
                yield break; // No enemy found, exit the loop
            }
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

    private IEnumerator Attack(Transform enemyTransform)
    {
        transform.LookAt(enemyTransform);
        _enemyUnitHealth = enemyTransform.GetComponent<UnitHealth>();
        _animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(1f);
        
        while (_enemyUnitHealth.CurrentHitPoint > -1)
        {
            _enemyUnitHealth.ProcessHit();
            // Debug.Log("Hit ");
            yield return new WaitForSeconds(1f); // Delay between hits
        }

        _animator.SetBool("Attacking", false);
    }
}
