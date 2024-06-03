using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int _maxHitPoint = 5;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")][SerializeField] private int _difficultyRamp = 1;
    [SerializeField] private ProgressBar _healthBar;
    [SerializeField] private Canvas _healthBarCanvas;
    [SerializeField] private bool _hasDifficultyRamp = false;

    private int _currentHitPoint = 0;
    public bool _isDead = false;

    private Unit _unit;
    
    public int MaxHitPoint
    {
        get { return _maxHitPoint; }
    }
    
    public int CurrentHitPoint
    {
        get { return _currentHitPoint; }
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (IsEnemy())
        {
            ProcessHit();
        }
    }

    private void Update()
    {
        Debug.Log($"{gameObject.name} hit point: {_currentHitPoint}");
    }

    public void ProcessHit()
    {
        if (_isDead)
        {
            return; // Exit if the unit is already dead
        }

        // Debug.Log($"{gameObject.name} being attacked - current hit point  {_currentHitPoint}");
        _currentHitPoint--;
        float progress = (_currentHitPoint > 0) ? (float)_currentHitPoint / (float)_maxHitPoint : 0.01f;
        // Debug.Log(progress);
        _healthBar.SetProgress(progress,3);
        if (_currentHitPoint < 0)
        {
            _isDead = true;
            _unit.UnitMover.IsFollowingPath = false;
            _unit.UnitAnimator.PlayDeathAnimation();
            
            // Die();
            if (_hasDifficultyRamp)
            {
                _maxHitPoint += _difficultyRamp;
            }

            if (IsEnemy())
            {
                _unit.RewardGold();
            }
            
           
        }
    }

    public void Die()
    {
        _unit.UnitAnimator.StopAnimations();
        gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(false);
    }


    void OnEnable()
    {
        _currentHitPoint = _maxHitPoint;
        _healthBar.ResetProgress();
        _healthBar.gameObject.SetActive(true);
        _isDead = false;
    }

    private void OnDisable()
    {
        if (_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false); 
        }
       
    }

    private bool IsEnemy()
    {
        if (LayersManager.HasLayer(_unit.UnitMask, ProjectLayers.Enemy))
        {
            return true;
        }
        return false;
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        _healthBarCanvas = canvas;
        _healthBar.transform.SetParent(_healthBarCanvas.transform);
        // _healthBar.gameObject.SetActive(true);
        
    }
    
    public void SetupHealthBar()
    {
        _healthBar.transform.SetParent(_healthBarCanvas.transform);
    }
  
}