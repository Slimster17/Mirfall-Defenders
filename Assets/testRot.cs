using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRot : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        FindClosestTarget();
        transform.LookAt(_target);
    }
    
    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (maxDistance > targetDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        _target = closestTarget;
    }
    
   
}
