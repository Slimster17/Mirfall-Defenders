using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    [Tooltip("Number of objects in the pool.")]
    [SerializeField][Range(0,10)] private int _poolSize = 5;
    
    [Tooltip("Enable infinite spawning of objects.")]
    [SerializeField] private bool _infinitySpawn;
    
    [Tooltip("Use separated paths for each unit.")]
    [SerializeField] private bool _separatedPaths;
    
    [Tooltip("ID for the unit type to spawn.")]
    [SerializeField] private SelectableUnits _unitID;
    
    [Tooltip("Prefab of the unit to spawn.")]
    [SerializeField] private GameObject _unitPrefab;
    
    [Tooltip("Time interval between spawns.")]
    [SerializeField][Range(0.1f, 20f)] private float _spawnTimer = 1f;
    
    [Tooltip("Canvas for the health bar UI.")]
    [SerializeField] private Canvas _healthBarCanvas;
    
    [Tooltip("Camera for the health bar UI.")]
    [SerializeField] private Camera _camera;

    private GameObject[] _pool; // Array of game objects in the pool
    
    public SelectableUnits UnitID { get => _unitID; } // Get the unit ID
    
    
    private void Awake()
    {
        PopulatePool();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (_infinitySpawn)
        {
            StartCoroutine(SpawnUnit()); 
        }
    }

    private void PopulatePool() // Populate the pool with game objects
    {
        _pool = new GameObject[_poolSize];

        for (int i = 0; i < _pool.Length; i++)
        {
            _pool[i] = Instantiate(_unitPrefab, transform);
            _pool[i].GetComponent<UnitHealth>().SetupHealthBar(_healthBarCanvas, _camera);
            
            // Set enemy game object inactive
            _pool[i].SetActive(false);

            // Set health bar game object inactive
            _pool[i].GetComponent<UnitHealth>().gameObject.SetActive(false);
            
        }
    }
    IEnumerator SpawnUnit() // Spawn a unit in the pool
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    public void EnableObjectInPool() // Enable a game object in the pool
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (_pool[i].activeInHierarchy == false)
            {
                _pool[i].SetActive(true);
                return;
            }
        }
    }
    
}
