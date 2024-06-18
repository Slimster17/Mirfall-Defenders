using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UnitSpawnSelector : MonoBehaviour
{
    private Bank _bank; // Reference to the bank object.
    public SelectableUnits SelectedUnit; // The currently selected unit.
    public ButtonUnitSpawner[] unitButtons; // The buttons for each unit.
    public int SelectedUnitCost; // The cost of the currently selected unit.

    private void Start()
    {
        if (_bank == null)
        {
            _bank = FindObjectOfType<Bank>();
            if (_bank != null)
            {
                // Recalculate interactivity when the bank balance changes
                _bank.onBalanceChanged.AddListener(RecalculateInteractivity); 
            }
            else
            {
                Debug.LogWarning("Bank object not found!");
            }
        }
    }
    private IEnumerator ButtonCooldown(ButtonUnitSpawner button) // Sets the cooldown for the button
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
    public void SetCurrentUnit(SelectableUnits unit) // Sets the currently selected unit
    {
        SelectedUnit = unit;
    }
    public void SetCurrentUnitFromInt(int unitIndex) // Sets the currently selected unit from the int value
    {
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
    public void ResetSelectedUnit() // Resets the currently selected unit
    {
        SelectedUnit = SelectableUnits.None;
        SelectedUnitCost = 0;
    }
    public void RecalculateInteractivity() // Recalculates the interactivity for all buttons
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