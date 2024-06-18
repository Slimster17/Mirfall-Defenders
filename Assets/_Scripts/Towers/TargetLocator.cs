using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetLocator : MonoBehaviour
{
    [Tooltip("Transform of the character that will rotate to aim at the target")]
    [SerializeField] private Transform _character;
    
    [Tooltip("Transform of the weapon arm that will rotate to aim at the target")]
    [SerializeField] private Transform _weaponArm;
    
    [Tooltip("Range within which the target can be fired upon")]
    [SerializeField] private float _fireRange = 8f;
    
    [Tooltip("Rate at which the weapon fires")]
    [SerializeField] private float _fireRate = 0.5f;
    
    [Tooltip("Particle system for the projectile fired by the weapon")]
    [SerializeField] private ParticleSystem _projectileParticles;
    
    [Tooltip("Current target that the character is aiming at")]
    [SerializeField] private Transform _target;
    
    [Tooltip("Minimum angle for character rotation on the X axis")]
    [SerializeField] private float minAngleX = 10f;
    
    [Tooltip("Maximum angle for character rotation on the X axis")]
    [SerializeField] private float maxAngleX = 12f;
    // [SerializeField] private float maxAngleXWeaponArm = 150f;
    // [SerializeField] private float minAngleXWeaponArm = 80f;
   
    [Tooltip("Speed at which the character and weapon arm aim at the target")]
    [SerializeField] private float targetingSpeed = 100f;
    
    private Animator _animator; // Reference to the animator
    
    public float FireRange {get { return _fireRange; }} // Property to get the fire range
    public float FireRate {get { return _fireRate; }} // Property to get the fire rate

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        FindClosestTarget();
        AimWeapon();
    }
    private void Update()
    {
        FindClosestTarget();
    }
    void LateUpdate()
    {
        // Aim the weapon in the LateUpdate to ensure it happens after all other updates
        AimWeapon();
    }
    private void FindClosestTarget() // Find all colliders within the fire range that belong to the "Enemy" layer
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
    private void AimWeapon() // Aims the weapon at the current target and attacks if the target is within range
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
    private void RotateCharacter() // Rotates the character to face the target
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
    private void Attack(bool isActive) // Controls the attack state of the character
    {
        _animator.SetBool("isShooting",isActive);
    }
}
