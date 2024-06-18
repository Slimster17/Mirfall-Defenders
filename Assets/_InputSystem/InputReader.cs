using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, 
    PlayerInputActions.ICameraMovementActions, 
    PlayerInputActions.ITilesInteractionActions
{
    // Events to notify other components about different input actions
    public event Action<Vector3> moveEvent;
    public event Action<Vector3, GameObject> clickEvent;
    public event Action toggleEvent;
    public event Action<Vector2> mousePositionEvent;
    private PlayerInputActions input;
    private void Awake()  // Awake is called when the script instance is being loaded
    {
        // Initialize the input actions and set callbacks for camera movement and tile interaction
        input = new PlayerInputActions();
        input.CameraMovement.SetCallbacks(this);
        input.TilesInteraction.SetCallbacks(this);
        
        // Enable the input actions
        input.CameraMovement.Enable();
        input.TilesInteraction.Enable();
    }
    private void OnDisable() // OnDisable is called when the behaviour becomes disabled
    {
        input.CameraMovement.Disable();
        input.TilesInteraction.Disable();
    }
    public void OnMove(InputAction.CallbackContext context) // Called when the move input action is performed
    {
        moveEvent?.Invoke(context.ReadValue<Vector3>());
    }
    public void OnMousePosition(InputAction.CallbackContext context) // Called when the mouse position input action is performed
    {
        mousePositionEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnClick(InputAction.CallbackContext context) // Called when the click input action is performed
    {
        if (context.performed)
        {
            // If the pointer is over a UI element, ignore the click
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // If the ray hits an object, invoke the clickEvent to notify listeners about the click and the object hit
            if (Physics.Raycast(ray, out hit))
            {
                clickEvent?.Invoke(Vector3.zero, hit.collider.gameObject);
            }
        }
    }
    public void OnToggleLabels(InputAction.CallbackContext context) // Called when the toggle labels input action is performed
    {
        toggleEvent?.Invoke();
    }
    
}
