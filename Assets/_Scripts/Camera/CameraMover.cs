using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{


    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float minXpos = 1f;
    [SerializeField] private float maxXpos = 10f;
    [SerializeField] private float minYpos = 1f;
    [SerializeField] private float maxYpos = 60f;
    [SerializeField] private float minZpos = 1f;
    [SerializeField] private float maxZpos = 10f;
    
    private Vector3 _moveDirection;
    private GridManager _gridManager;
    private InputReader _inputReader;
    
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _inputReader = FindObjectOfType<InputReader>();
    }

    private void Start()
    {
        int gridCount = _gridManager.GridSize.x - 5;
        int gridTileSize = _gridManager.UnityGridSize;
        
        minXpos = -_gridManager.UnityGridSize;
        maxXpos = gridCount * gridTileSize;
        
        minZpos = -_gridManager.UnityGridSize;
        maxZpos = gridCount * gridTileSize;
    }

    private void OnEnable()
    {
        _inputReader.moveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.moveEvent -= OnMoveInput;
    }


    void OnMoveInput(Vector3 direction)
    {
        _moveDirection = new Vector3(direction.x, direction.y, direction.z);
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(_moveDirection.x, _moveDirection.y, _moveDirection.z).normalized;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minXpos, maxXpos);
        newPosition.y = Mathf.Clamp(newPosition.y, minYpos, maxYpos);
        newPosition.z = Mathf.Clamp(newPosition.z, minZpos, maxZpos);

        transform.position = newPosition;
    }
}
