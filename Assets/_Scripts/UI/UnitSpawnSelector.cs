using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UnitSpawnSelector : MonoBehaviour
{
    private Bank _bank;
    public SelectableUnits SelectedUnit;
    public ButtonUnitSpawner[] unitButtons;
    public int SelectedUnitCost;

    private void Awake()
    {
        // _bank = FindObjectOfType<Bank>();
    }

    private void Start()
    {
        if (_bank == null)
        {
            _bank = FindObjectOfType<Bank>();
            if (_bank != null)
            {
                _bank.onBalanceChanged.AddListener(RecalculateInteractivity);
            }
            else
            {
                Debug.LogWarning("Bank object not found!");
            }
        }
    }

    // private void OnEnable()
    // {
    //     _bank.onBalanceChanged.AddListener(RecalculateInteractivity);
    // }
    //
    // private void OnDisable()
    // {
    //     _bank.onBalanceChanged.RemoveListener(RecalculateInteractivity);
    // }


    public void SetCurrentUnit(SelectableUnits unit)
    {
       
        SelectedUnit = unit;
    }
    
    public void SetCurrentUnitFromInt(int unitIndex)
    {
        
        // if (!RecalculateButtonInteractivity(unitIndex))
        // {
        //     return;
        // }
        // Debug.Log(unitButtons[unitIndex-1]);
        if (System.Enum.IsDefined(typeof(SelectableUnits), unitIndex))
        {
            SelectableUnits unit = (SelectableUnits)unitIndex;
            SetCurrentUnit(unit);
            StartCoroutine(ButtonCooldown(unitButtons[unitIndex-1]));
        }
        else
        {
            Debug.LogWarning("Invalid unit index: " + unitIndex);
        }
    }
    
    public void ResetSelectedUnit()
    {
        SelectedUnit = SelectableUnits.None;
        SelectedUnitCost = 0;
    }
    
    private IEnumerator ButtonCooldown(ButtonUnitSpawner button)
    {
        SelectedUnitCost = button.Cost;
        button.Button.interactable = false;
        float countDown = button.CountDown;
        button.IsOnCooldown = true;

        while (countDown > 0)
        {
            button.CountDownText.text = countDown.ToString("F0") + " s";
            yield return new WaitForSeconds(1f);
            countDown--;
        }

        button.CountDownText.text = " ";
        button.Button.interactable = true;
        button.IsOnCooldown = false;
        SelectedUnit = SelectableUnits.None;
        RecalculateInteractivity();

    }

    private void Update()
    {
        // Debug.Log($"Selected unis is - {SelectedUnit}");
    }

    private bool RecalculateButtonInteractivity(int unitIndex)
    {
        if (_bank.CurrentBalance < unitButtons[unitIndex-1].Cost)
        {
            unitButtons[unitIndex-1].Button.interactable = false;
            return false;
        }
        unitButtons[unitIndex-1].Button.interactable = true;
        return true;
    }
    
    public void RecalculateInteractivity()
    {
        foreach (var button in unitButtons)
        {
            if (button.IsOnCooldown)
            {
                button.Button.interactable = false;
                continue;
            }
            
            if (_bank.CurrentBalance < button.Cost)
            {
                button.Button.interactable = false;
            }
            else if (_bank.CurrentBalance >= button.Cost)
            {
                button.Button.interactable = true;
            }
        }
    }
}