using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private Transform _weaponArm;
    [SerializeField] private Transform _target;
    
    [SerializeField] private float minAngleX = 10f;
    [SerializeField] private float maxAngleX = 12f;
    [SerializeField] private float maxAngleXWeaponArm = 150f;
    [SerializeField] private float minAngleXWeaponArm = 80f;
    [SerializeField] private float targetingSpeed = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        _target = FindObjectOfType<EnemyMover>().transform;
        AimWeapon();
    }


    void LateUpdate()
    {
        AimWeapon();
    }

    private void AimWeapon()
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

        // Плавно обертаємо руку зі зброєю в напрямку обертання до цілі
        Quaternion newWeaponArmRotation = Quaternion.RotateTowards(_weaponArm.rotation, targetRotation, targetingSpeed * Time.deltaTime);
        
        // Обмежуємо обертання по осі x для руки зі зброєю
        euler = newWeaponArmRotation.eulerAngles;
        float clampedXAngleWeaponArm = Mathf.Clamp(euler.x, minAngleXWeaponArm, maxAngleXWeaponArm);
        Quaternion clampedRotationWeaponArm = Quaternion.Euler(clampedXAngleWeaponArm, euler.y, euler.z);
        
        // Застосовуємо обмежене обертання до руки зі зброєю
        _weaponArm.rotation = clampedRotationWeaponArm;

    }
}
