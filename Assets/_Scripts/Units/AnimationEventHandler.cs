using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationEventHandler : MonoBehaviour
{
    [Tooltip("The parent GameObject that contains the Unit script.")]
    [SerializeField] GameObject _parentGameObject;

    public void CallDieMethod() // Call the Die method on the UnitHealth component
    {
        
        if (_parentGameObject != null)
        {
            Unit targetScript = _parentGameObject.GetComponent<Unit>();
            if (targetScript != null)  // Try to get the Unit component from the parent GameObject
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
