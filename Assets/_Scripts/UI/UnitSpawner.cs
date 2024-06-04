using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : MonoBehaviour
{
   private UnitSpawnSelector unitSpawnSelector;
    public List<GameObject> unitPrefabs;
    public List<ObjectPool> _unitPools;
    public Vector2Int spawnPoint;
    private SelectedTile selectedTile;
    
    private InputReader _inputReader;
    private GridManager _gridManager;
    private PathFinder[] _pathFinders;
    private MousePositionTracker _mousePositionTracker;
    private Bank _bank;
    
    public GridManager GridManager => _gridManager;

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
    
    private void OnEnable()
    {
        _inputReader.clickEvent += OnClickInput;
    }

    private void OnDisable()
    {
        _inputReader.clickEvent -= OnClickInput;
    }
    
    private void FindPathFinders()
    {
        _pathFinders = PathFinderRegistry.GetAllPathFindersArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(FindPathFinders),1f);
        // Ensure the list of prefabs matches the enum length minus 1 (for None)
        // if (unitPrefabs.Count != System.Enum.GetValues(typeof(SelectableUnits)).Length - 1)
        // {
        //     Debug.LogError("The number of unit prefabs does not match the number of selectable units.");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // if (unitSpawnSelector.SelectedUnit != SelectableUnits.None) // Ensure the SelectedUnit is not None
        // {
        //     // Check path blockers before spawning
        //     if (CheckAllPathFindersBlocks())
        //     {
        //         // Instantiate the selected prefab at the spawn point
        //         Vector3 spawnPosition = new Vector3(spawnPoint.x, 0, spawnPoint.y);
        //         Instantiate(unitPrefabs[(int)unitSpawnSelector.SelectedUnit - 1], spawnPosition, Quaternion.identity);
        //         
        //         // Notify all path finders
        //         NotifyAllPathFinders();
        //
        //         // Reset the selected unit after spawning
        //         unitSpawnSelector.ResetSelectedUnit();
        //     }
        // }

        if (unitSpawnSelector.SelectedUnit == SelectableUnits.None)
        {
            return;
        }
        
         if (unitSpawnSelector.SelectedUnit == SelectableUnits.Crusader)
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
    
    public bool CheckAllPathFindersBlocks(Vector2Int coordinates)
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

    public void NotifyAllPathFinders()
    {
        foreach (var pathFinder in _pathFinders)
        {
            pathFinder.NotifyReceivers();
        }
    }
    
    private void OnClickInput(Vector3 direction, GameObject clickedObject)
    {
        if (selectedTile == null)
        {
            // Debug.LogError("SelectedTile is null");
            return;
        }

        // if (selectedTile.Selected.TileLayerMask == ProjectLayers.Enemy)
        // {
        //     return;
        // }

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
        _bank.Withdraw(unitSpawnSelector.SelectedUnitCost);
        unitSpawnSelector.ResetSelectedUnit();
        
    }
}
