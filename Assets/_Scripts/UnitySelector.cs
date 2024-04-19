using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySelector : MonoBehaviour
{
    private InputReader _inputReader;
    
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
        if (clickedObject == gameObject)
        {
            Debug.Log($"{gameObject.name}");
            NotifyLabels();
        }
    }

    public void NotifyLabels()
    {
        GridManager gridManager = GetComponent<GridManager>();
        BroadcastMessage("PaintGrid1", gridManager, SendMessageOptions.RequireReceiver);
    }
}
