using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingPathChecker : MonoBehaviour
{
    private Vector3 lastPosition;
    // private PathFinder[] _pathFinder;
    public UnityEvent<bool> onPossitionChanged = new UnityEvent<bool>();

    private void Awake()
    {
        // _pathFinder = FindObjectsOfType<PathFinder>();
    }

    private void Start()
    {
        // Save the initial position
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Check if the position has changed
        if (transform.position != lastPosition)
        {
            Debug.Log("Position Changed");
            lastPosition = transform.position;
            onPossitionChanged.Invoke(false);
            
        }
    }
  
}
