using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUnitSpawner : MonoBehaviour
{
    [Tooltip("Text component displaying the cost of the unit.")]
    [SerializeField] private TextMeshProUGUI _costText;
    
    [Tooltip("Text component displaying the countdown before the button becomes active again.")]
    [SerializeField] private TextMeshProUGUI _countDownText;
    
    [Tooltip("Cost of spawning the unit.")]
    [SerializeField] private int _cost;
    
    [Tooltip("Time in seconds for the button cooldown.")]
    [SerializeField] private float _buttonCountdown = 5f;
    
    private Button _button; // Button component for the button to be spawned.
    public bool IsOnCooldown = false; // Is the button currently on cooldown?
     
    public TextMeshProUGUI CostText { get { return _costText; } } // Property for the cost text.
    public TextMeshProUGUI CountDownText { get { return _countDownText; } set { _countDownText = value; } } // Property for the countdown text.
    public float CountDown { get { return _buttonCountdown; } set { _buttonCountdown = value; } } // Property for the countdown.
    public Button Button { get { return _button; } set { _button = value; } } // Property for the button.

    public int Cost // Property for the cost.
    {
        get { return _cost; }
    }
    private void Start()
    {
        _button = GetComponent<Button>();
        CountDownText.text = " ";
        _costText.text = _cost.ToString() + " $";
    }
}