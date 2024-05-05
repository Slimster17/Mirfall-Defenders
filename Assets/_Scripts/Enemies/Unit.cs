using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int goldReward = 25;
    [SerializeField] private int goldPenalty = 25;
    [SerializeField] private ProjectLayers _unitMask;

    private Bank _bank;
    
    public ProjectLayers UnitMask
    {
        get { return _unitMask; }
    }

    private void Awake()
    {
        _unitMask = (ProjectLayers)LayersManager.GetLayerIndex(gameObject);
        _bank = FindObjectOfType<Bank>();
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
