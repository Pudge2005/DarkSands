//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/DarkSands/_source/Inputs/PlayerCharacterControls.inputactions
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

namespace Game.Inputs
{
    public partial class @PlayerCharacterControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerCharacterControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerCharacterControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""0bff7652-c2fe-46e9-b14d-923f9d4af627"",
            ""actions"": [
                {
                    ""name"": ""MoveInDirection"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f2356bac-b720-4782-9514-7f1e9e4e56ac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveToPoint"",
                    ""type"": ""Button"",
                    ""id"": ""5eca29a7-f258-4e12-ad5e-6d7c88253efb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PointerScreenPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""408e9b60-2e01-4d7e-87d8-590668c8a3db"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WSAD"",
                    ""id"": ""ac3a3bdf-cb2e-4f96-8269-6a39828f306b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveInDirection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e57e8eaa-04d7-415d-8aed-a8f89abd7da0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveInDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""936709f9-8de5-4cf1-9352-96990f5de01a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveInDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2c2b7396-4dbc-44a5-9fa5-cfb018254202"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveInDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7d3803b0-9952-4838-a5bd-95275eb9cc80"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveInDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fc165def-dcdf-4c72-a339-b95ef742ca44"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveToPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""984abc9f-f41a-4f3e-ae18-dd9cd62d66a7"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerScreenPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Default
            m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
            m_Default_MoveInDirection = m_Default.FindAction("MoveInDirection", throwIfNotFound: true);
            m_Default_MoveToPoint = m_Default.FindAction("MoveToPoint", throwIfNotFound: true);
            m_Default_PointerScreenPosition = m_Default.FindAction("PointerScreenPosition", throwIfNotFound: true);
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

        // Default
        private readonly InputActionMap m_Default;
        private List<IDefaultActions> m_DefaultActionsCallbackInterfaces = new List<IDefaultActions>();
        private readonly InputAction m_Default_MoveInDirection;
        private readonly InputAction m_Default_MoveToPoint;
        private readonly InputAction m_Default_PointerScreenPosition;
        public struct DefaultActions
        {
            private @PlayerCharacterControls m_Wrapper;
            public DefaultActions(@PlayerCharacterControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveInDirection => m_Wrapper.m_Default_MoveInDirection;
            public InputAction @MoveToPoint => m_Wrapper.m_Default_MoveToPoint;
            public InputAction @PointerScreenPosition => m_Wrapper.m_Default_PointerScreenPosition;
            public InputActionMap Get() { return m_Wrapper.m_Default; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
            public void AddCallbacks(IDefaultActions instance)
            {
                if (instance == null || m_Wrapper.m_DefaultActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_DefaultActionsCallbackInterfaces.Add(instance);
                @MoveInDirection.started += instance.OnMoveInDirection;
                @MoveInDirection.performed += instance.OnMoveInDirection;
                @MoveInDirection.canceled += instance.OnMoveInDirection;
                @MoveToPoint.started += instance.OnMoveToPoint;
                @MoveToPoint.performed += instance.OnMoveToPoint;
                @MoveToPoint.canceled += instance.OnMoveToPoint;
                @PointerScreenPosition.started += instance.OnPointerScreenPosition;
                @PointerScreenPosition.performed += instance.OnPointerScreenPosition;
                @PointerScreenPosition.canceled += instance.OnPointerScreenPosition;
            }

            private void UnregisterCallbacks(IDefaultActions instance)
            {
                @MoveInDirection.started -= instance.OnMoveInDirection;
                @MoveInDirection.performed -= instance.OnMoveInDirection;
                @MoveInDirection.canceled -= instance.OnMoveInDirection;
                @MoveToPoint.started -= instance.OnMoveToPoint;
                @MoveToPoint.performed -= instance.OnMoveToPoint;
                @MoveToPoint.canceled -= instance.OnMoveToPoint;
                @PointerScreenPosition.started -= instance.OnPointerScreenPosition;
                @PointerScreenPosition.performed -= instance.OnPointerScreenPosition;
                @PointerScreenPosition.canceled -= instance.OnPointerScreenPosition;
            }

            public void RemoveCallbacks(IDefaultActions instance)
            {
                if (m_Wrapper.m_DefaultActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IDefaultActions instance)
            {
                foreach (var item in m_Wrapper.m_DefaultActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_DefaultActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public DefaultActions @Default => new DefaultActions(this);
        public interface IDefaultActions
        {
            void OnMoveInDirection(InputAction.CallbackContext context);
            void OnMoveToPoint(InputAction.CallbackContext context);
            void OnPointerScreenPosition(InputAction.CallbackContext context);
        }
    }
}