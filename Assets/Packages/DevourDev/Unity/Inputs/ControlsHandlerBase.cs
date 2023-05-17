using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DevourDev.Unity.Inputs
{
    public abstract class ControlsHandlerBase<TControls> : MonoBehaviour
        where TControls : class, IInputActionCollection, IDisposable, new()
    {
        private TControls _controls;


        protected TControls Controls => _controls;


        protected virtual void Awake()
        {
            CreateControls();

            if (enabled)
                EnableControls();
        }

        protected virtual void OnEnable()
        {
            EnableControls();
        }

        protected virtual void OnDisable()
        {
            DisableControls();
        }


        private void CreateControls()
        {
            if (_controls != null)
            {
                UnityEngine.Debug.LogError("Attempt to create controls when old controls are exist");
                DisposeControls();
            }

            _controls = new();
            SubscribeToInputActions(_controls);
        }


        private void EnableControls()
        {
            if (_controls != null)
            {
                _controls.Enable();
            }
        }

        private void DisableControls()
        {
            if (_controls != null)
            {
                _controls.Disable();
            }
        }

        private void DisposeControls()
        {
            if (_controls != null)
            {
                _controls.Dispose();
                _controls = null;
            }
        }


        protected abstract void SubscribeToInputActions(TControls controls);
    }
}
