using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : MonoBehaviour
{
    private UnitSpawnSelector unitSpawnSelector; // Reference to the UnitSpawnSelector script
    public List<GameObject> unitPrefabs; // List of the unit prefabs
    public List<ObjectPool> _unitPools; // List of the unit pools
    public Vector2Int spawnPoint; // The spawn point of the unit
    private SelectedTile selectedTile; // Curently selected tile
    
    private InputReader _inputReader; // Reference to the InputReader script
    private GridManager _gridManager; // Reference to the GridManager script
    private PathFinder[] _pathFinders; // All the path finders
    private MousePositionTracker _mousePositionTracker; // Reference to the MousePositionTracker script
    private Bank _bank; // Reference to the Bank script
    
    public GridManager GridManager => _gridManager; // Getter for the GridManager script

    private void Awake()
    {
        unitSpawnSelector = FindObjectOfType<UnitSpawnSelector>();
        _gridManager = FindObjectOfType<GridManager>();
        // _pathFinders = FindObjectsOfType<PathFinder>();
        _mousePositionTracker = FindObjectOfType<MousePositionTracker>();
        _inputReader = FindObjectOfType<InputReader>();
        selectedTile = FindObjectOfType<SelectedTile>();
        _bank = FindObjectOfType<Bank>();
       
    }
    private void OnEnable() // Called when the script is enabled
    {
        _inputReader.clickEvent += OnClickInput;
    }
    private void OnDisable() // Called when the script is disabled
    {
        _inputReader.clickEvent -= OnClickInput;
    }
    private void FindPathFinders() // Find all the path finders
    {
        _pathFinders = PathFinderRegistry.GetAllPathFindersArray();
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(FindPathFinders),1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (unitSpawnSelector.SelectedUnit == SelectableUnits.None) // If no unit is selected
        {
            return;
        }
         if (unitSpawnSelector.SelectedUnit == SelectableUnits.Crusader) // If the crusader is selected
        {
            ObjectPool pool = _unitPools.Find(p => p.UnitID == unitSpawnSelector.SelectedUnit);
            if (pool != null)
            {
                pool.EnableObjectInPool();
                // NotifyAllPathFinders();
                _bank.Withdraw(unitSpawnSelector.SelectedUnitCost);
                unitSpawnSelector.ResetSelectedUnit();
            }
            else
            {
                Debug.LogError($"No pool found for unit: {unitSpawnSelector.SelectedUnit}");
            }
        }
    }
    private void OnClickInput(Vector3 direction, GameObject clickedObject) // Click event handler
    {
        if (selectedTile == null)
        {
            // Debug.LogError("SelectedTile is null");
            return;
        }
        
        var selected = selectedTile.Selected;
        if (selected == null)
        {
            // Debug.LogError("Selected tile is null");
            return;
        }
        
        if (unitSpawnSelector.SelectedUnit == SelectableUnits.TowerArcher)
        {
            // Debug.Log($"Unit spawner click");
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        
            if (CheckAllPathFindersBlocks(selectedTile.Selected.Coordinates) && _gridManager.GetNode(selectedTile.Selected.Coordinates).isWalkable)
            {
                Tower tower = unitPrefabs[0].GetComponent<Tower>();
                bool isSuccessful = tower.CreateTower(tower,selectedTile.Selected.transform.position);
                if (isSuccessful)
                {
                    _gridManager.BlockNode(selectedTile.Selected.Coordinates);
                    NotifyAllPathFinders();
                   
                }
            }
        }
        if (unitSpawnSelector.SelectedUnit == SelectableUnits.Trebuchet)
        {
            // Debug.Log("Trebuchet fire click");{}
            Trebuchet trebuchet = FindObjectOfType<Trebuchet>();
            trebuchet.SetTargetPosition(selectedTile.Selected.transform);
            trebuchet.Attack();
        }
        _bank.Withdraw(unitSpawnSelector.SelectedUnitCost); // Withdraw the cost of the selected unit
        unitSpawnSelector.ResetSelectedUnit();
    }
    public bool CheckAllPathFindersBlocks(Vector2Int coordinates) // Check if all path finders are blocking the path
    {
        foreach (var pathFinder in _pathFinders)
        {
            if (pathFinder.WillBlockPath(coordinates))
            {
                return false;
            }
        }
        return true;
    }
    public void NotifyAllPathFinders() // Notify all path finders that the selected tile has been selected
    {
        foreach (var pathFinder in _pathFinders)
        {
            pathFinder.NotifyReceivers();
        }
    }
}
