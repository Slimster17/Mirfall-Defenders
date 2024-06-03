using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    [SerializeField][Range(0,10)] private int _poolSize = 5;
    [SerializeField] private bool _infinitySpawn;
    [SerializeField] private bool _separatedPaths;
    [SerializeField] private SelectableUnits _unitID;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField][Range(0.1f, 20f)] private float _spawnTimer = 1f;
    [SerializeField] private Canvas _healthBarCanvas;
    [SerializeField] private Camera _camera;

    private GameObject[] _pool;
    
    public SelectableUnits UnitID { get => _unitID; }
    
    
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

    private void PopulatePool()
    {
        _pool = new GameObject[_poolSize];

        for (int i = 0; i < _pool.Length; i++)
        {
            _pool[i] = Instantiate(_unitPrefab, transform);
            _pool[i].GetComponent<UnitHealth>().SetupHealthBar(_healthBarCanvas, _camera);
            
            // Unit unitComponent = _pool[i].GetComponent<Unit>();
            //
            // if (_separatedPaths)
            // {
            //     unitComponent.PathFinder = unitComponent.gameObject.AddComponent<PathFinder>();
            //  
            //     gameObject.GetComponent<PathFinder>().enabled = false;
            //
            // }
            // else
            // {
            //     unitComponent.PathFinder = GetComponent<PathFinder>();
            // }
            // unitComponent.PathFinder.Unit = unitComponent;
            
            // Set enemy game object inactive
            _pool[i].SetActive(false);

            // Set health bar game object inactive
            _pool[i].GetComponent<UnitHealth>().gameObject.SetActive(false);
            
        }
    }
    IEnumerator SpawnUnit()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    public void EnableObjectInPool()
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (_pool[i].activeInHierarchy == false)
            {
                _pool[i].SetActive(true);
                // _pool[i].GetComponent<EnemyHealth>().gameObject.SetActive(true);
                return;
            }
        }
    }
    
}
