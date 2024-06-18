using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Tooltip("Gold reward for certain actions.")]
    [SerializeField] private int goldReward = 25;
    
    [Tooltip("Gold penalty for certain actions.")]
    [SerializeField] private int goldPenalty = 25;
    
    [Tooltip("Mask representing the unit's layer.")]
    [SerializeField] private ProjectLayers _unitMask;
    
    [Tooltip("Indicates if the unit has separated paths.")]
    [SerializeField] private bool _hasSeparatedPaths;

    // References
    private Bank _bank; 
    private GridManager _gridManager; 
    private UnitAnimator _unitAnimator; 
    private UnitHealth _unitHealth; 
    private UnitAttack _unitAttack; 
    private UnitMover _unitMover; 
    private PathFinder _pathFinder; 
   
    // Properties
    public GridManager GridManager { get { return _gridManager; } }
    public UnitAnimator UnitAnimator {get { return _unitAnimator; } }
    public UnitHealth UnitHealth {get { return _unitHealth; } }
    public UnitAttack UnitAttack {get { return _unitAttack; } }
    public UnitMover UnitMover {get { return _unitMover; } }
    public PathFinder PathFinder {get { return _pathFinder; } set { _pathFinder = value; } }
    public ProjectLayers UnitMask {get { return _unitMask; } }

    private void Awake()
    {
        _unitMask = (ProjectLayers)LayersManager.GetLayerIndex(gameObject);
        
        _bank = FindObjectOfType<Bank>();
        _gridManager = FindObjectOfType<GridManager>();
        
        _unitAnimator = GetComponent<UnitAnimator>();
        _unitHealth = GetComponent<UnitHealth>();
        _unitAttack = GetComponent<UnitAttack>();
        _unitMover = GetComponent<UnitMover>();

        if (_hasSeparatedPaths) // Check if the unit should have separated paths
        {
            PathFinder parentPathFinder = GetComponentInParent<PathFinder>();
            
            PathFinder = gameObject.AddComponent<PathFinder>();
            PathFinder.CopyFrom(parentPathFinder);

        }
        else
        {
            // Use the parent's PathFinder component
            PathFinder = GetComponentInParent<PathFinder>();;
        }
        // Register the PathFinder in the registry
        PathFinderRegistry.RegisterPathFinder(PathFinder);
        PathFinder.Unit = this;
    }

    private void OnDestroy()
    {
        PathFinderRegistry.UnregisterPathFinder(PathFinder);
    }

    public void RewardGold() // Reward the player with gold
    {
        if (_bank == null)
        {
            return;
        }
        _bank.Deposit(goldReward);
    }
    
    public void WithdrawGold() // Withdraw gold from the player
    {
        if (_bank == null)
        {
            return;
        }
        _bank.Withdraw(goldPenalty);
    }
    
}
