using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Trebuchet : MonoBehaviour
{
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private ThrowingStone _stone;
    [SerializeField] private Transform _trebuchetPlatform;
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private LayerMask _targetMask;
    public Transform attackPoint;
    
    
    [Header("Settings")]
    public int totalThrows = 100;
    public float throwCooldown = 0;

    [Header("Throwing")]
    public float throwForce = 10;
    public float throwUpwardForce = 2;
    
    public float minThrowForce = 10f;
    public float maxThrowForce = 20f;
    public float maxDistance = 50f;
    
    public bool _hasThrown = false;
    bool readyToThrow;
    private bool throwStone;

    private Animator _animator;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
       
    }
    
    private void Start()
    {
        readyToThrow = true;
    }

    private void OnValidate()
    {
        if (_animator == null)
        {
            return;
        }
        if (isAttacking)
        {
            // _animator.SetBool("Attacking", true);
        }
        else
        {
            _animator.SetBool("Attacking", false);
        }
    }

    private void Update()
    {
        if (isAttacking && !_hasThrown)
        {
            _stone.SetFireBallEffectsEnabled(true);
            StopAllCoroutines();
            StartCoroutine(RotateToTarget());
        }

        if (throwStone & _hasThrown)
        {
            Throw();
        }
    }

    private void FixedUpdate()
    {
        // if (!_animator.IsInTransition(0) && 
        //     _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.trebuchet_Attack") 
        //     && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f )
        // {
        //     throwStone = true;
        // }
    }

    public void Throw()
    {
        Debug.Log($"Throwing");

        _stone.TargetMask = _targetMask;
        _stone.PrepareForThrow();

        Transform projectile = _stone.transform;
        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        // projectileRb.isKinematic = false;

        Vector3 forceDirection = (attackPoint.position - _stone.transform.position).normalized;

        RaycastHit hit;

        if(Physics.Raycast(_stone.transform.position, _stone.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        float distance = Vector3.Distance(attackPoint.position, _stone.transform.position);
        float throwForce = minThrowForce + (distance / maxDistance) * (maxThrowForce - minThrowForce);
        Vector3 forceToAdd = forceDirection * throwForce + Vector3.up * throwUpwardForce;

        _stone.transform.parent = null;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        throwStone = false;
        readyToThrow = true;
        isAttacking = false;
        _hasThrown = false;
        _animator.SetBool("Attacking", false);
    }

    
    private IEnumerator RotateToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(attackPoint.position.x, startPosition.y, attackPoint.position.z); // Ensure the end position is at the same height as the start position

        Quaternion startRotation = _trebuchetPlatform.transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

        float rotationPercent = 0f;
        while (rotationPercent < 1)
        {
            rotationPercent += Time.deltaTime * _rotationSpeed;
            Quaternion currentRotation = Quaternion.Lerp(startRotation, endRotation, rotationPercent);
            _trebuchetPlatform.transform.rotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0); // Constrain rotation to Y-axis only

            // If the difference between the current angle and the target angle is very small, finish the coroutine
            if (Quaternion.Angle(_trebuchetPlatform.transform.rotation, endRotation) < 5f)
            {
                _trebuchetPlatform.transform.rotation = Quaternion.Euler(0, endRotation.eulerAngles.y, 0);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        _hasThrown = true;
        Debug.Log("RotationCompleted");
        _animator.SetBool("Attacking", true);
        readyToThrow = false;
        // _animator.SetBool("Attacking", false);
    }


    public void SetTargetPosition(Transform target)
    {
        attackPoint = target;
    }

    public void Attack()
    {
        isAttacking = true;
    }
}
