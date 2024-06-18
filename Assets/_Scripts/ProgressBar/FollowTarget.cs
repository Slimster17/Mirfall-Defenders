using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowTarget : MonoBehaviour
{
    [Tooltip("The target that this object will follow.")]
    [SerializeField] private Transform _target;
   
    [Tooltip("The offset from the target's position.")]
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        if (_target.gameObject.activeSelf) // Check if the target is active
        {
            // Update the position of this object to follow the target with the specified offset
            transform.position = _target.position + _offset;
        }
        else
        {
            // Deactivate this object if the target is inactive
            this.gameObject.SetActive(false);
        }
    }
}
