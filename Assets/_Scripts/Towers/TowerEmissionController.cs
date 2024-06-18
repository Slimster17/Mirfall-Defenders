using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEmissionController : MonoBehaviour
{
    [Tooltip("Particle system for the projectile particles.")]
    [SerializeField] private ParticleSystem _projectileParticles;
    
    private TargetLocator _targetLocator; // Reference to the target locator component.

    private void Awake()
    {
        _targetLocator = GetComponentInParent<TargetLocator>();
        
    }

    // Shoots an arrow by enabling the emission module and playing the particle system
    public void ShotArrow() 
    {
        var emissionModule = _projectileParticles.emission;
        emissionModule.enabled = true;
        
        _projectileParticles.Play();
    }

   
}
