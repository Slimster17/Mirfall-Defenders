using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEmissionController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _projectileParticles;
    
    private TargetLocator _targetLocator;

    private void Awake()
    {
        _targetLocator = GetComponentInParent<TargetLocator>();
        
    }

    void Start()
    {
        // var emissionModule = _projectileParticles.emission;
        // emissionModule.rateOverTime = _targetLocator.FireRate;
    }

    public void ShotArrow()
    {
        var emissionModule = _projectileParticles.emission;
        emissionModule.enabled = true;
        
        _projectileParticles.Play();
    }

   
}
