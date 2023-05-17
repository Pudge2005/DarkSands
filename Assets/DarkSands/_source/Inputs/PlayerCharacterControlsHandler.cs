using DevourDev.Utility;
using DevourDev.Unity.CharacterControlling.Movement;
using DevourDev.Unity.Inputs;
using Game.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkSands.Inputs
{
    public sealed class PlayerCharacterControlsHandler : NetworkControlsHandlerBase<PlayerCharacterControls>
    {
        [SerializeField] private NavMeshMovementController _movementController;

        private Transform _camTr;


        private Transform CamTr
        {
            get
            {
                if (_camTr == null)
                {
                    _camTr = Camera.main.transform;
                }

                return _camTr;
            }
        }


        protected override void SubscribeToInputActions(PlayerCharacterControls controls)
        {
            var defMap = controls.Default;
            defMap.MoveInDirection.performed += MoveInDirection_performed;
            defMap.MoveToPoint.performed += MoveToPoint_performed;
        }

        private void MoveToPoint_performed(InputAction.CallbackContext context)
        {
            var point = Controls.Default.PointerScreenPosition.ReadValue<Vector2>();
            _movementController.MoveToPoint(point);
        }

        private void MoveInDirection_performed(InputAction.CallbackContext context)
        {
            Vector2 inputValue = context.ReadValue<Vector2>();
            var yRot = GeometryHelpers.GetRotation(CamTr.rotation, true, false, true);
            Vector3 direction = GeometryHelpers.X0Y(inputValue);
            direction = yRot * direction;
            _movementController.MoveDirection = direction;
        }
    }
}
