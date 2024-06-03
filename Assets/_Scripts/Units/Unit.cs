using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int goldReward = 25;
    [SerializeField] private int goldPenalty = 25;
    [SerializeField] private ProjectLayers _unitMask;
    [SerializeField] private bool _hasSeparatedPaths;

    private Bank _bank;

    private GridManager _gridManager;

    private UnitAnimator _unitAnimator;
    private UnitHealth _unitHealth;
    private UnitAttack _unitAttack;
    private UnitMover _unitMover;
    private PathFinder _pathFinder;
    
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

        if (_hasSeparatedPaths)
        {
            PathFinder parentPathFinder = GetComponentInParent<PathFinder>();
            
            PathFinder = gameObject.AddComponent<PathFinder>();
            // PathFinder.StartCoordinates = parentPathFinder.StartCoordinates;
            // PathFinder.DestinationCoordinates = parentPathFinder.DestinationCoordinates;
            PathFinder.CopyFrom(parentPathFinder);

        }
        else
        {
            PathFinder = GetComponentInParent<PathFinder>();;
            
        }
        PathFinderRegistry.RegisterPathFinder(PathFinder);
        PathFinder.Unit = this;
    }
    
    public void RewardGold()
    {
        if (_bank == null)
        {
            return;
        }
        _bank.Deposit(goldReward);
    }
    
    public void WithdrawGold()
    {
        if (_bank == null)
        {
            return;
        }
        _bank.Withdraw(goldPenalty);
    }
    
}