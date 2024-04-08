using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField][Range(0,10)] private int _poolSize = 5;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField][Range(0.1f, 20f)] private float _spawnTimer = 1f;

    private GameObject[] _pool;


    private void Awake()
    {
        PopulatePool();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void PopulatePool()
    {
        _pool = new GameObject[_poolSize];

        for (int i = 0; i < _pool.Length; i++)
        {
            _pool[i] = Instantiate(_enemyPrefab, transform);
            _pool[i].SetActive(false);
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    private void EnableObjectInPool()
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