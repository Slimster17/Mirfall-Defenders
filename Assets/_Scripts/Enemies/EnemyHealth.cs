using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHitPoint = 5;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")][SerializeField] private int _difficultyRamp = 1;

    private int _currentHitPoint = 0;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        _currentHitPoint--;
        
        if (_currentHitPoint < 0)
        {
            gameObject.SetActive(false);
            _maxHitPoint += _difficultyRamp;
            _enemy.RewardGold();
        }
    }

    
    void OnEnable()
    {
        _currentHitPoint = _maxHitPoint;
    }

  
}
