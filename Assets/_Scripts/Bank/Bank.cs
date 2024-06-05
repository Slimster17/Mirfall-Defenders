using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Bank : MonoBehaviour
{
    [SerializeField] private int _startingBalance = 150;
    [SerializeField] private TextMeshProUGUI _displayBalance;
    [SerializeField] private int _enemiesToKill = 30;
    [SerializeField] private TextMeshProUGUI _displayEnemiesLeft;
    [SerializeField] private Canvas _victoryCanvas;
    
    
    private int _currentBalance;
    private int _currentEnemiesLeft;
    public UnityEvent onBalanceChanged = new UnityEvent();

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _currentBalance = _startingBalance;
        _currentEnemiesLeft = _enemiesToKill;
        _sceneLoader = FindObjectOfType<SceneLoader>();
        UpdateDisplay();
    }

    private void Start()
    {
        
        _victoryCanvas.gameObject.SetActive(false);
    }

    public int CurrentBalance
    {
        get { return _currentBalance; }
    }

    public void Deposit(int amount)
    {
        _currentBalance += Math.Abs(amount);
        _currentEnemiesLeft--;
        UpdateDisplay();
        if (_currentEnemiesLeft == 0)
        {
            _victoryCanvas.gameObject.SetActive(true);
            SceneLoader.instance.PauseGameWithoutMenu();
        }
        
    }
    
    public void Withdraw(int amount)
    {
        _currentBalance -= Math.Abs(amount);
        
        if (_currentBalance < 0)
        {
           //Game over; 
           // RealadScene();
           _sceneLoader.RestartScene();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_displayBalance != null)
        {
            _displayBalance.text = $"Gold: {_currentBalance}";
            _displayEnemiesLeft.text = $"Enemies Left: {_currentEnemiesLeft}";
            onBalanceChanged?.Invoke();
        }
    }
    
}
