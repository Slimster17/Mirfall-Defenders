using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private Transform _weaponArm;
    [SerializeField] private float _fireRange = 8f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private ParticleSystem _projectileParticles;
    [SerializeField] private Transform _target;
    
    
    
    
    [SerializeField] private float minAngleX = 10f;
    [SerializeField] private float maxAngleX = 12f;
    // [SerializeField] private float maxAngleXWeaponArm = 150f;
    // [SerializeField] private float minAngleXWeaponArm = 80f;
    [SerializeField] private float targetingSpeed = 100f;
    
    private Animator _animator;
    
    public float FireRange {get { return _fireRange; }}
    public float FireRate {get { return _fireRate; }}

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        FindClosestTarget();
        AimWeapon();
        
        // var emissionModule = _projectileParticles.emission;
        // emissionModule.rateOverTime = _fireRate;
    }

    private void Update()
    {
        FindClosestTarget();
    }

    void LateUpdate()
    {
        AimWeapon();
    }
    
    private void FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _fireRange, LayerMask.GetMask("Enemy"));
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            float targetDistance = Vector3.Distance(transform.position, collider.transform.position);
            if (maxDistance > targetDistance)
            {
                closestTarget = collider.transform;
                maxDistance = targetDistance;
            }
        }

        _target = closestTarget;
    }


    // private void FindClosestTarget()
    // {
    //     Unit[] enemies = FindObjectsOfType<Unit>();
    //     Transform closestTarget = null;
    //     float maxDistance = Mathf.Infinity;
    //
    //     foreach (var enemy in enemies)
    //     {
    //         float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
    //         if (maxDistance > targetDistance)
    //         {
    //             closestTarget = enemy.transform;
    //             maxDistance = targetDistance;
    //         }
    //     }
    //
    //     _target = closestTarget;
    // }

    private void AimWeapon()
    {
        if (_target != null)
        {
            UnitHealth targetHealth = _target.GetComponent<UnitHealth>();
            float targetDistance = Vector3.Distance(transform.position, _target.position);

            if (targetDistance < _fireRange && !targetHealth._isDead)
            {
                Attack(true);
                RotateCharacter();
                Vector3 targetPosition = _target.position;
                targetPosition.y += 1.0f; // Look 1 unit higher
                _weaponArm.LookAt(targetPosition);
                // _weaponArm.LookAt(_target);
            }
            else
            {
                Attack(false);
            }
        }
        else
        {
            Attack(false);
        }
    }

    private void RotateCharacter()
    {
        // Отримує напрямок до цілі
        Vector3 directionToTarget = (_target.position - _character.position).normalized;
        
        // Розраховуємо кути обертання для функції LookAt
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Плавно обертаємо персонажа в напрямку обертання до цілі
        Quaternion newCharacterRotation = Quaternion.RotateTowards(_character.rotation, targetRotation, targetingSpeed * Time.deltaTime);

        // Обмежуємо обертання по осі x для персонажа
        Vector3 euler = newCharacterRotation.eulerAngles;
        float clampedXAngle = Mathf.Clamp(euler.x, minAngleX, maxAngleX);
        Quaternion clampedRotation = Quaternion.Euler(clampedXAngle, euler.y, euler.z);

        // Застосовуємо обмежене обертання до персонажа
        _character.rotation = clampedRotation;
        
        // // Плавно обертаємо руку зі зброєю в напрямку обертання до цілі
        // Quaternion newWeaponArmRotation = Quaternion.RotateTowards(_weaponArm.rotation, targetRotation, targetingSpeed * Time.deltaTime);
        //
        // // Обмежуємо обертання по осі x для руки зі зброєю
        // euler = newWeaponArmRotation.eulerAngles;
        // float clampedXAngleWeaponArm = Mathf.Clamp(euler.x, minAngleXWeaponArm, maxAngleXWeaponArm);
        // Quaternion clampedRotationWeaponArm = Quaternion.Euler(clampedXAngleWeaponArm, euler.y, euler.z);
        //
        // // Застосовуємо обмежене обертання до руки зі зброєю
        // _weaponArm.rotation = clampedRotationWeaponArm;
    }

    private void Attack(bool isActive)
    {
        _animator.SetBool("isShooting",isActive);
        // var emissionModule = _projectileParticles.emission;
        // emissionModule.enabled = isActive;
    }
}
