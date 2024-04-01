//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_InputSystem/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Tiles Interaction"",
            ""id"": ""91eb2864-ec11-4cde-802b-196260958af1"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""94f6fea8-3c1d-41f7-94c5-68247cfeba78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleLabels"",
                    ""type"": ""Button"",
                    ""id"": ""8339c3d9-70b1-492b-a07f-e7aecd12acfe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""da6cb5d0-d063-4323-8495-f8dedc31d19a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4409957-b597-4a78-af90-d0fc602e544e"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleLabels"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera Movement"",
            ""id"": ""d0f14043-e91c-4e23-a3c7-d16c1d1cd40f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5880ccc5-9767-4d3e-a395-9b5939b1adf3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""adf0236a-6269-4ec1-9e16-6e29b7ef2737"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""327cb7b4-faa9-407b-811f-e843964dac99"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8fb55aba-e4cd-4685-bcff-2f244752e84a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""38177d56-aee0-47fe-9b18-3181df76b5dc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9e8a3392-489f-4c5c-9b4e-8b85e3b383ce"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""ed321b62-ad6c-4190-a90e-886939a8027b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""4e61d8d3-c2f0-44af-b9f2-dd72531840bb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""09806141-6293-4489-a40c-7e18176226e4"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fe363ada-869c-4ed2-a81b-b91742c9289e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9bc0ec41-da49-4899-a578-fb06d15f3019"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""39b2b086-d84f-4322-a69e-7886d6ca7050"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""29faa6dd-193b-467f-acd0-a4c2f896dd80"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""ad4670a0-10ed-46e1-8360-7979dd348d17"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""333e1a6a-9b5f-4eb6-979c-1e2bcf903e0e"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Tiles Interaction
        m_TilesInteraction = asset.FindActionMap("Tiles Interaction", throwIfNotFound: true);
        m_TilesInteraction_Click = m_TilesInteraction.FindAction("Click", throwIfNotFound: true);
        m_TilesInteraction_ToggleLabels = m_TilesInteraction.FindAction("ToggleLabels", throwIfNotFound: true);
        // Camera Movement
        m_CameraMovement = asset.FindActionMap("Camera Movement", throwIfNotFound: true);
        m_CameraMovement_Move = m_CameraMovement.FindAction("Move", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Tiles Interaction
    private readonly InputActionMap m_TilesInteraction;
    private List<ITilesInteractionActions> m_TilesInteractionActionsCallbackInterfaces = new List<ITilesInteractionActions>();
    private readonly InputAction m_TilesInteraction_Click;
    private readonly InputAction m_TilesInteraction_ToggleLabels;
    public struct TilesInteractionActions
    {
        private @PlayerInputActions m_Wrapper;
        public TilesInteractionActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_TilesInteraction_Click;
        public InputAction @ToggleLabels => m_Wrapper.m_TilesInteraction_ToggleLabels;
        public InputActionMap Get() { return m_Wrapper.m_TilesInteraction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TilesInteractionActions set) { return set.Get(); }
        public void AddCallbacks(ITilesInteractionActions instance)
        {
            if (instance == null || m_Wrapper.m_TilesInteractionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TilesInteractionActionsCallbackInterfaces.Add(instance);
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @ToggleLabels.started += instance.OnToggleLabels;
            @ToggleLabels.performed += instance.OnToggleLabels;
            @ToggleLabels.canceled += instance.OnToggleLabels;
        }

        private void UnregisterCallbacks(ITilesInteractionActions instance)
        {
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @ToggleLabels.started -= instance.OnToggleLabels;
            @ToggleLabels.performed -= instance.OnToggleLabels;
            @ToggleLabels.canceled -= instance.OnToggleLabels;
        }

        public void RemoveCallbacks(ITilesInteractionActions instance)
        {
            if (m_Wrapper.m_TilesInteractionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITilesInteractionActions instance)
        {
            foreach (var item in m_Wrapper.m_TilesInteractionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TilesInteractionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TilesInteractionActions @TilesInteraction => new TilesInteractionActions(this);

    // Camera Movement
    private readonly InputActionMap m_CameraMovement;
    private List<ICameraMovementActions> m_CameraMovementActionsCallbackInterfaces = new List<ICameraMovementActions>();
    private readonly InputAction m_CameraMovement_Move;
    public struct CameraMovementActions
    {
        private @PlayerInputActions m_Wrapper;
        public CameraMovementActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CameraMovement_Move;
        public InputActionMap Get() { return m_Wrapper.m_CameraMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraMovementActions set) { return set.Get(); }
        public void AddCallbacks(ICameraMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraMovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(ICameraMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(ICameraMovementActions instance)
        {
            if (m_Wrapper.m_CameraMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraMovementActions @CameraMovement => new CameraMovementActions(this);
    public interface ITilesInteractionActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnToggleLabels(InputAction.CallbackContext context);
    }
    public interface ICameraMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
