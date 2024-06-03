using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] GameObject _parentGameObject;

    public void CallDieMethod()
    {
        if (_parentGameObject != null)
        {
            Unit targetScript = _parentGameObject.GetComponent<Unit>();
            if (targetScript != null)
            {
                targetScript.UnitHealth.Die();
            }
            else
            {
                Debug.LogError("TargetScript component not found on target GameObject.");
            }
        }
        else
        {
            Debug.LogError("Target GameObject not assigned.");
        }
    }
}
