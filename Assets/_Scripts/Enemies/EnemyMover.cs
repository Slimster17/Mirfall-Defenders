using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;
    [SerializeField] [Range(0f,5f)] private float _rotationSpeed = 1f;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowWaypoints());
    }

    // Update is called once per frame
    
    void Update()
    {
        
    }
    
    IEnumerator FollowWaypoints()
    {
        if (_waypoints != null)
        {
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                Waypoint currentWaypoint = _waypoints[i];
                Waypoint nextWaypoint = _waypoints[i + 1];

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
        
    }
}
