using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingStone : MonoBehaviour
{
    [SerializeField] private Transform _stoneContainer;
    [SerializeField] private ParticleSystem _fireBallEffect;
    [SerializeField] private ParticleSystem _onTriggerEffect;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float _attackRadius = 10f;
    private Rigidbody _rb;
    private ParticleSystem _onTriggerEffectInstance;
    
    

    public LayerMask TargetMask { get; set; }
    

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody>();
        
        if (_onTriggerEffect!= null)
        {
            _onTriggerEffectInstance = Instantiate(_onTriggerEffect, transform.position, Quaternion.identity);
            _onTriggerEffectInstance.Stop();
            // _onTriggerEffectInstance.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"{other.name} - {other.transform.position}");
       
        // Instantiate(_onTriggerEffect, other.transform.position, Quaternion.identity);
        PlayOnTriggerEffect(other);
        CreateAreaOfAttack(other);
        FinishThrow();
    }

    private void PlayOnTriggerEffect(Collider other)
    {

        _onTriggerEffectInstance.transform.position = other.transform.position;

        _onTriggerEffectInstance.Play();
    }

    private void CreateAreaOfAttack(Collider other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, _attackRadius, TargetMask);
        foreach (var hitCollider in hitColliders)
        {
            // TODO: Attack units
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
    
    public void PrepareForThrow()
    {
        _trail.emitting = true;
        // SetFireBallEffectEmission(true);
        _rb.isKinematic = false;
        transform.parent = null;
    }

    public void FinishThrow()
    {
        // _onTriggerEffectInstance.gameObject.SetActive(false);
        SetFireBallEffectsEnabled(false);
        _rb.isKinematic = true;
        transform.parent = _stoneContainer;
        transform.position = _stoneContainer.position;
        transform.rotation = _stoneContainer.rotation;
    }
    
    public void SetFireBallEffectsEnabled(bool enabled)
    {
        _fireBallEffect.gameObject.SetActive(enabled);
        _trail.gameObject.SetActive(enabled);
    }
    
    
    
}
