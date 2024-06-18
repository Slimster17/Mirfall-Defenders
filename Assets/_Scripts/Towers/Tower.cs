using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Tooltip("Cost of building the tower.")] 
    [SerializeField] private int _cost = 75;
    // Creates a tower at the specified position if the player has enough resources
    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            return false;
        }
        
        if (bank.CurrentBalance >= _cost)
        {
            Instantiate(tower.gameObject, position, Quaternion.identity);
            return true;
        }
        
        return false;
    }
}
