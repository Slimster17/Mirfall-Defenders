using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _path = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;

    private Animator animator;
    private Enemy _enemy;
    
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _enemy = GetComponent<Enemy>();
    }
    
    private void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowWaypoints());
    }

    private void FindPath()
    {
        _path.Clear();
        
        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in parent.transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();

            if (waypoint != null)
            {
                _path.Add(waypoint);  
            }
                    
        }
    }

    private void ReturnToStart()
    {
        transform.position = _path[0].transform.position;
    }

    void FinishPath()
    {
        _enemy.StealGold();
        gameObject.SetActive(false);
    }
   
    IEnumerator FollowWaypoints()
    {
        if (_path != null)
        {
            for (int i = 0; i < _path.Count - 1; i++)
            {
                Waypoint currentWaypoint = _path[i];
                Waypoint nextWaypoint = _path[i + 1];

                if (Vector3.Distance(currentWaypoint.transform.position, nextWaypoint.transform.position) < 0.01f)
                    continue; // Skip waypoints that are too close

                Vector3 startPosition = transform.position;
                Vector3 endPosition = nextWaypoint.transform.position;

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
