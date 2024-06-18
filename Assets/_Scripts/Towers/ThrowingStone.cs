using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingStone : MonoBehaviour
{
    [Tooltip("Transform of the container holding the stone.")] 
    [SerializeField] private Transform _stoneContainer;
    
    [Tooltip("Particle system for the fireball effect.")]
    [SerializeField] private ParticleSystem _fireBallEffect;
    
    [Tooltip("Particle system for the effect triggered on collision.")]
    [SerializeField] private ParticleSystem _onTriggerEffect;
    
    [Tooltip("Trail renderer for the stone.")]
    [SerializeField] private TrailRenderer _trail;
    
    [Tooltip("Radius within which the attack affects targets.")]
    [SerializeField] private float _attackRadius = 10f;
    
    [Tooltip("Damage dealt by the stone.")]
    [SerializeField] private float _damage = 10f;
    
    private Rigidbody _rb; // Reference to the rigidbody component.
    private ParticleSystem _onTriggerEffectInstance; // Reference to the particle system instance.
    public LayerMask TargetMask { get; set; } // Mask of the layer the stone is on.
    
    private void Awake()
    { 
        _rb = GetComponent<Rigidbody>();
        
        if (_onTriggerEffect!= null)
        {
            _onTriggerEffectInstance = Instantiate(_onTriggerEffect, transform.position, Quaternion.identity);
            _onTriggerEffectInstance.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayOnTriggerEffect(other);
        CreateAreaOfAttack(other);
        FinishThrow();
    }

    private void PlayOnTriggerEffect(Collider other) // Plays the on-trigger effect at the position of the collision
    {
        _onTriggerEffectInstance.transform.position = other.transform.position;

        _onTriggerEffectInstance.Play();
        SoundManager.PlaySound(SoundType.StoneHit);
    }

    // Creates an area of attack at the position of the collision, affecting all targets within the radius
    private void CreateAreaOfAttack(Collider other) 
    {
        Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, _attackRadius, TargetMask);
        foreach (var hitCollider in hitColliders)
        {
            UnitHealth hitUnitHealth = hitCollider.GetComponent<UnitHealth>();

            for (int i = 0; i < _damage; i++)
            {
                if (hitUnitHealth._isDead)
                {
                    break;
                }
                hitUnitHealth.ProcessHit();
            }

            // TODO: Attack units
        }
    }
    private void OnDrawGizmosSelected() // Drawing sphere for debugging
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
    
    public void PrepareForThrow() // Prepares the stone for throwing
    {
        _trail.emitting = true;
        _rb.isKinematic = false;
        transform.parent = null;
    }

    public void FinishThrow() // Finishes the throwing process and resets the stone.
    {
        SetFireBallEffectsEnabled(false);
        _trail.Clear();
        _rb.isKinematic = true;
        transform.parent = _stoneContainer;
        transform.position = _stoneContainer.position;
        transform.rotation = _stoneContainer.rotation;
    }
    
    public void SetFireBallEffectsEnabled(bool enabled) // Enables or disables the fireball effects
    {
        _fireBallEffect.gameObject.SetActive(enabled);
        _trail.gameObject.SetActive(enabled);
    }
    
    
    
}
