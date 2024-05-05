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

    public void ProcessHit()
    {
        // Debug.Log($"{gameObject.name} being attacked - current hit point  {_currentHitPoint}");
        _currentHitPoint--;
        float progress = (_currentHitPoint > 0) ? (float)_currentHitPoint / (float)_maxHitPoint : 0.01f;
        // Debug.Log(progress);
        _healthBar.SetProgress(progress,3);
        if (_currentHitPoint < 0)
        {
            gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(false);
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
