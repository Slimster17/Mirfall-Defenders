using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        if (_target.gameObject.activeSelf)
        {
            transform.position = _target.position + _offset;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
