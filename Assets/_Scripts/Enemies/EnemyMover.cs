using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
     
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;
    
    private List<Node> _path = new List<Node>();
    
    private Animator animator;
    private Enemy _enemy;
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = GetComponentInParent<PathFinder>();
        _enemy = GetComponent<Enemy>();
    }
    
    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = _pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        StopAllCoroutines();
        _path.Clear();
        _path = _pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        _enemy.StealGold();
        gameObject.SetActive(false);
    }
   
    IEnumerator FollowPath()
    {
        if (_path != null)
        {
            for (int i = 1; i < _path.Count - 1; i++)
            {
                Node currentTile = _path[i];
                Node nextTile = _path[i + 1];

                if (Vector3.Distance(_gridManager.GetPositionFromCoordinates(currentTile.coordinates),
                        _gridManager.GetPositionFromCoordinates(nextTile.coordinates)) < 0.01f)
                    continue; // Skip waypoints that are too close

                Vector3 startPosition = transform.position;
                Vector3 endPosition = _gridManager.GetPositionFromCoordinates(nextTile.coordinates);

                Quaternion startRotation = transform.rotation;
                Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

                float travelPercent = 0f;
                while (travelPercent < 1)
                {
                    travelPercent += Time.deltaTime * _movementSpeed;
    
                    float rotationTravelPercent = Mathf.Min(travelPercent * _rotationSpeed, 1f);
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationTravelPercent);
    
                    animator.SetInteger("Walking", 1);
    
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        else
        {
            yield return null;
        }

        animator.SetInteger("Attacking", 1);
        animator.SetInteger("Walking", 0);
        
      FinishPath();
      
    }
}