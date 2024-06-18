using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Bank : MonoBehaviour
{
    [Tooltip("Initial balance of the bank.")] 
    [SerializeField] private int _startingBalance = 150;
    
    [Tooltip("Text element to display the current balance.")]
    [SerializeField] private TextMeshProUGUI _displayBalance;
   
    [Tooltip("Number of enemies required to be killed for victory.")]
    [SerializeField] private int _enemiesToKill = 30;
    
    [Tooltip("Text element to display the number of enemies left.")]
    [SerializeField] private TextMeshProUGUI _displayEnemiesLeft;
    
    [Tooltip("Canvas to display upon victory.")]
    [SerializeField] private Canvas _victoryCanvas;
    
    // Local variables to track state of the game
    private int _currentBalance;
    private int _currentEnemiesLeft;
    
    private SceneLoader _sceneLoader; // Reference to the SceneLoader to manage scene transitions
    
    public UnityEvent onBalanceChanged = new UnityEvent(); // Event to notify other components when the balance changes
  
    public int CurrentBalance // Property to get the current balance
    {
        get { return _currentBalance; }
    }
    
    private void Awake()   // Awake is called when the script instance is being loaded
    {
        // Initialize current balance and enemies left to the starting values
        _currentBalance = _startingBalance;
        _currentEnemiesLeft = _enemiesToKill;
        _sceneLoader = FindObjectOfType<SceneLoader>();
        UpdateDisplay();
    }
    private void Start() // Start is called before the first frame update
    {
        // Hide the victory canvas at the start of the game
        _victoryCanvas.gameObject.SetActive(false);
    }
    private void UpdateDisplay()  // Method to update the UI display with the current balance and enemies left
    {
        if (_displayBalance != null)
        {
            _displayBalance.text = $"Gold: {_currentBalance}";
            _displayEnemiesLeft.text = $"Enemies Left: {_currentEnemiesLeft}";
            onBalanceChanged?.Invoke();
        }
    }
    public void Deposit(int amount) // Method to deposit an amount into the bank
    {
        // Increase the current balance and decrease the enemies left
        _currentBalance += Math.Abs(amount);
        _currentEnemiesLeft--;
        UpdateDisplay();
        
        if (_currentEnemiesLeft == 0) // Check if all enemies have been killed
        {
            // Show the victory canvas and pause the game
            _victoryCanvas.gameObject.SetActive(true);
            SceneLoader.instance.PauseGameWithoutMenu();
        }
        
    }
    public void Withdraw(int amount)  // Method to withdraw an amount from the bank
    {
        // Decrease the current balance
        _currentBalance -= Math.Abs(amount);
        
        if (_currentBalance < 0) // If balance is negative, restart the scene (game over)
        {
           _sceneLoader.RestartScene(); 
        }
        UpdateDisplay();
    }
}
