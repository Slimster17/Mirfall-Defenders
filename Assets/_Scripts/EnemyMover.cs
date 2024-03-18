using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)] private float _movementSpeed = 1f;

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
            foreach (var w in _waypoints)
            {
                Vector3 startPosition = transform.position;
                Vector3 endPosition = w.transform.position;
                
                Quaternion startRotation = transform.rotation;
                Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

                float travelPercent = 0f;
                while (travelPercent < 1)
                {
                    travelPercent += Time.deltaTime * _movementSpeed;
                    
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, travelPercent);
                    
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
