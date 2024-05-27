using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelector : MonoBehaviour
{
    private InputReader _inputReader;
    
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
