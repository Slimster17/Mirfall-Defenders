using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelector : MonoBehaviour
{
    private InputReader _inputReader;
    
    public UnityEvent<CrusaderMover> onUnitSelected = new UnityEvent<CrusaderMover>();
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
        CrusaderMover crusaderMover = clickedObject.GetComponent<CrusaderMover>();
        if (crusaderMover != null)
        {
            onUnitSelected?.Invoke(crusaderMover);
        }
        
    }
    
}
