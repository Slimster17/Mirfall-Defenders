using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.ICameraMovementActions, PlayerInputActions.ITilesInteractionActions
{
    // public static InputReader Instance { get; private set; }

    public event Action<Vector3> moveEvent;
    public event Action<Vector3, GameObject> clickEvent;

    private PlayerInputActions input;

    private void Awake()
    {
        // if (Instance != null && Instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        // }

        input = new PlayerInputActions();
        input.CameraMovement.SetCallbacks(this);
        input.TilesInteraction.SetCallbacks(this);

        input.CameraMovement.Enable();
        input.TilesInteraction.Enable();
    }

    private void OnDisable()
    {
        input.CameraMovement.Disable();
        input.TilesInteraction.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector3>());
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                clickEvent?.Invoke(Vector3.zero, hit.collider.gameObject); // Передаємо об'єкт, на якому відбувся клік
            }
        }
    }
}
