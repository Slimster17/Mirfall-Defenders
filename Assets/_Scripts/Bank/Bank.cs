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
    
    private int _currentBalance;
    public UnityEvent onBalanceChanged = new UnityEvent();

    private void Awake()
    {
        _currentBalance = _startingBalance;
        UpdateDisplay();
    }

    public int CurrentBalance
    {
        get { return _currentBalance; }
    }

    public void Deposit(int amount)
    {
        _currentBalance += Math.Abs(amount);
        UpdateDisplay();
    }
    
    public void Withdraw(int amount)
    {
        _currentBalance -= Math.Abs(amount);
        
        if (_currentBalance < 0)
        {
           //Game over; 
           RealadScene();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _displayBalance.text = $"Gold: {_currentBalance}";
        onBalanceChanged.Invoke();
    }

    void RealadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
