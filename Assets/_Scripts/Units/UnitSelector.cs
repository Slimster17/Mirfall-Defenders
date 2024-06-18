using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelector : MonoBehaviour
{
    private InputReader _inputReader; // Reference to the InputReader component
    
    // Event triggered when a unit is selected
    public UnityEvent<UnitMover> onUnitSelected = new UnityEvent<UnitMover>();
    private void Awake()
    {
        
        _inputReader = FindObjectOfType<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.clickEvent += OnClickInput;
    }

    private void OnDisable()
    {
        _inputReader.clickEvent -= OnClickInput;
    }
    
    // Called when a click input is detected.
    // Checks if the clicked object has a UnitMover component and
    // triggers the onUnitSelected event if it does
    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        UnitMover crusaderMover = clickedObject.GetComponent<UnitMover>();
        if (crusaderMover != null)
        {
            Debug.Log($"Unit selected {crusaderMover.name}");
            onUnitSelected?.Invoke(crusaderMover);
        }
        
    }
    
}
