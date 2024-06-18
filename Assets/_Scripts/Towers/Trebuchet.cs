using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Trebuchet : MonoBehaviour
{
    [Tooltip("Indicates if the trebuchet is attacking.")]
    [SerializeField] private bool isAttacking = false;
    
    [Tooltip("Reference to the stone to be thrown.")]
    [SerializeField] private ThrowingStone _stone;
    
    [Tooltip("Transform of the trebuchet platform.")]
    [SerializeField] private Transform _trebuchetPlatform;
    
    [Tooltip("Speed at which the trebuchet rotates towards the target.")]
    [SerializeField] private float _rotationSpeed = 0.5f;
    
    [Tooltip("Layer mask to identify the targets.")]
    [SerializeField] private LayerMask _targetMask;
    
    public Transform attackPoint; // The point at which the trebuchet will throw the stone.
    
    [Header("Settings")]
    [Tooltip("Total number of throws the trebuchet can perform.")] public int totalThrows = 100;
    [Tooltip("Cooldown time between throws.")] public float throwCooldown = 0;

    [Header("Throwing")]
    [Tooltip("Force applied to the stone when throwing.")] public float throwForce = 10;
    [Tooltip("Upward force applied to the stone when throwing.")] public float throwUpwardForce = 2;
    
    [Tooltip("Minimum force applied to the stone when throwing.")] public float minThrowForce = 10f;
    [Tooltip("Maximum force applied to the stone when throwing.")] public float maxThrowForce = 20f;
    [Tooltip("Maximum distance the stone can be thrown.")] public float maxDistance = 50f;
    
    public bool _hasThrown = false; // Has the stone been thrown?
    bool readyToThrow; // Is the trebuchet ready to throw?
    private bool throwStone; // Is the trebuchet currently throwing?

    private Animator _animator; // Reference to the animator component
    private bool hasPlayedRotationSound = false; // Has the trebuchet played the rotation sound yet?

    private AudioSource rotationAudioSource; // Reference to the audio source component


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        rotationAudioSource = gameObject.AddComponent<AudioSource>();
       
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
    private void ResetThrow() // Resets the trebuchet to its original state
    {
        throwStone = false;
        readyToThrow = true;
        isAttacking = false;
        _hasThrown = false;
        _animator.SetBool("Attacking", false);
        hasPlayedRotationSound = false;
    }
    private IEnumerator RotateToTarget() // Rotates the trebuchet towards the target
    {
        if (!hasPlayedRotationSound)
        {
            SoundManager.PlayLoopingSound(SoundType.TrebuchetRotation, rotationAudioSource);
            hasPlayedRotationSound = true; // Set the flag to true
        }
        
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
        // Debug.Log("RotationCompleted");
        _animator.SetBool("Attacking", true);
        readyToThrow = false;
        
        rotationAudioSource.Stop();
        hasPlayedRotationSound = false;
        // _animator.SetBool("Attacking", false);
    }
    public void Throw() // Throws the stone towards the target
    {
        // SoundManager.StopSound();
        // Debug.Log($"Throwing");
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
    public void SetTargetPosition(Transform target) // Sets the target position for the trebuchet to attack
    {
        attackPoint = target;
    }
    public void Attack() // Initiates an attack
    {
        isAttacking = true;
    }
}
