using System;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace DevourDev.Unity.Inputs
{
    public abstract class NetworkControlsHandlerBase<TControls> : NetworkBehaviour
        where TControls : class, IInputActionCollection, IDisposable, new()
    {
        private TControls _controls;


        protected TControls Controls => _controls;


        protected virtual void OnEnable()
        {
            if (!IsOwner)
            {
                return;
            }

            EnableControls();
        }

        protected virtual void OnDisable()
        {
            if (!IsOwner)
            {
                return;
            }

            DisableControls();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                return;
            }

            CreateControls();

            if (enabled)
                EnableControls();
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
