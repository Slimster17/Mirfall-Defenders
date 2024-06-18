using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitHealth : MonoBehaviour
{
    [Tooltip("Maximum hit points of the unit.")]
    [SerializeField] private int _maxHitPoint = 5;
    
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] private int _difficultyRamp = 1;
    
    [Tooltip("Progress bar to display health.")]
    [SerializeField] private ProgressBar _healthBar;
    
    [Tooltip("Canvas to display the health bar.")]
    [SerializeField] private Canvas _healthBarCanvas;
    
    [Tooltip("Indicates if the unit has difficulty ramp.")]
    [SerializeField] private bool _hasDifficultyRamp = false;

    private int _currentHitPoint = 0; // Current hit point of the unit
    public bool _isDead = false; // Indicates if the unit is dead

    private Unit _unit; // Reference to the unit
    
    // Properties
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
    void OnEnable() // Initializes the unit's health
    {
        _currentHitPoint = _maxHitPoint;
        _healthBar.ResetProgress();
        _healthBar.gameObject.SetActive(true);
        _isDead = false;
    }
    private void OnDisable() // Hides the health bar
    {
        if (_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false); 
        }
       
    }
    private bool IsEnemy() // Checks if the unit is an enemy based on its layer
    {
        if (LayersManager.HasLayer(_unit.UnitMask, ProjectLayers.Enemy))
        {
            return true;
        }
        return false;
    }
    private void OnParticleCollision(GameObject other) // Called when the unit collides with a particle
    {
        if (IsEnemy())
        {
            ProcessHit();
        }
    }
    public void ProcessHit() // Process hit point of the unit
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
        // If the unit's hit points drop below zero, it handles the unit's death
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
    public void Die() // / Handles the unit's death
    {
        _unit.UnitAnimator.StopAnimations();
        gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(false);
    }
    public void SetupHealthBar(Canvas canvas, Camera camera) // Sets up the health bar by attaching it to a specified canvas
    {
        _healthBarCanvas = canvas;
        _healthBar.transform.SetParent(_healthBarCanvas.transform);
        // _healthBar.gameObject.SetActive(true);
        
    }
    public void SetupHealthBar() // Sets up the health bar by attaching it to the previously assigned canvas
    {
        _healthBar.transform.SetParent(_healthBarCanvas.transform);
    }
}