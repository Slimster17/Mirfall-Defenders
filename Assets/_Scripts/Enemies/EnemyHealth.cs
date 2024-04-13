using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHitPoint = 5;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")][SerializeField] private int _difficultyRamp = 1;
    [SerializeField] private ProgressBar _healthBar;

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
        float progress = (_currentHitPoint > 0) ? (float)_currentHitPoint / (float)_maxHitPoint : 0.01f;
        Debug.Log(progress);
        _healthBar.SetProgress(progress,3);
        if (_currentHitPoint < 0)
        {
            gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(false);
            _maxHitPoint += _difficultyRamp;
            _enemy.RewardGold();
        }
    }

    
    void OnEnable()
    {
        _currentHitPoint = _maxHitPoint;
        _healthBar.ResetProgress();
        _healthBar.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false); 
        }
       
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        _healthBar.transform.SetParent(canvas.transform);
        // _healthBar.gameObject.SetActive(true);
        
    }
  
}
