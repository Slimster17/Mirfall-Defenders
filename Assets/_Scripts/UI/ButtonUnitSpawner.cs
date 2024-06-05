using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUnitSpawner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private int _cost;
    [SerializeField] private float _buttonCountdown = 5f;
    private Button _button;
    public bool IsOnCooldown = false;
     
    public TextMeshProUGUI CostText { get { return _costText; } }
    public TextMeshProUGUI CountDownText { get { return _countDownText; } set { _countDownText = value; } }
    public float CountDown { get { return _buttonCountdown; } set { _buttonCountdown = value; } }
    public Button Button { get { return _button; } set { _button = value; } }

    public int Cost
    {
        get { return _cost; }
    }
    // private void Awake()
    // {
    //     _button = GetComponent<Button>();
    //     CountDownText.text = " ";
    //     _costText.text = _cost.ToString() + " $";
    // }

    private void Start()
    {
        _button = GetComponent<Button>();
        CountDownText.text = " ";
        _costText.text = _cost.ToString() + " $";
    }
}