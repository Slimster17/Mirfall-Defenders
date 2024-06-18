using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Tooltip("Speed at which the camera moves.")][SerializeField] private float moveSpeed = 50f;
    [Tooltip("Minimum X position for the camera.")][SerializeField] private float minXpos = 1f;
    [Tooltip("Maximum X position for the camera.")][SerializeField] private float maxXpos = 10f;
    [Tooltip("Minimum Y position for the camera.")][SerializeField] private float minYpos = 1f;
    [Tooltip("Maximum Y position for the camera.")][SerializeField] private float maxYpos = 60f;
    [Tooltip("Minimum Z position for the camera.")][SerializeField] private float minZpos = 1f;
    [Tooltip("Maximum Z position for the camera.")][SerializeField] private float maxZpos = 10f;
    
    private Vector3 _moveDirection;  // Direction in which the camera should move
   
    private GridManager _gridManager; // Reference to the GridManager
    private InputReader _inputReader; // Reference to the InputReader
    
    private void Awake() // Awake is called when the script instance is being loaded
    {
        _gridManager = FindObjectOfType<GridManager>();
        _inputReader = FindObjectOfType<InputReader>();
    }
    private void Start()  // Start is called before the first frame update
    {
        // Calculate and set the boundaries for the camera movement based on the grid size
        int gridCount = _gridManager.GridSize.x - 5;
        int gridTileSize = _gridManager.UnityGridSize;
        
        minXpos = -_gridManager.UnityGridSize;
        maxXpos = gridCount * gridTileSize;
        
        minZpos = -_gridManager.UnityGridSize;
        maxZpos = gridCount * gridTileSize;
    }
    private void OnEnable()  // OnEnable is called when the object becomes enabled and active
    {
        _inputReader.moveEvent += OnMoveInput; // Subscribe to the moveEvent from the InputReader
    }
    private void OnDisable()  // OnDisable is called when the object becomes disabled or inactive
    {
        _inputReader.moveEvent -= OnMoveInput;  // Unsubscribe from the moveEvent to avoid memory leaks
    }
    private void OnMoveInput(Vector3 direction) // Method to handle the move input event
    {
        // Set the move direction based on the input
        _moveDirection = new Vector3(direction.x, direction.y, direction.z);
    }
    private void Update()  // Update is called once per frame
    {
        // Normalize the move direction and calculate the new position
        Vector3 moveDirection = new Vector3(_moveDirection.x, _moveDirection.y, _moveDirection.z).normalized;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        
        // Clamp the new position within the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minXpos, maxXpos);
        newPosition.y = Mathf.Clamp(newPosition.y, minYpos, maxYpos);
        newPosition.z = Mathf.Clamp(newPosition.z, minZpos, maxZpos);
        
        // Apply the new position to the camera
        transform.position = newPosition;
    }
}
