using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Infrastructure.Camera
{
    public class CameraInputHandler : IInitializable
    {
        private readonly PlayerInputActions _playerInputActions;
        private readonly CameraController _cameraController;

        public CameraInputHandler(PlayerInputActions playerInputActions, CameraController cameraController)
        {
            this._playerInputActions = playerInputActions;
            this._cameraController = cameraController;
        }

        public void Initialize()
        {
            this.EnableInput();
            this.SetupInput();
        }

        private void EnableInput()
        {
            this._playerInputActions.Enable();
        }

        private void SetupInput()
        {
            this._playerInputActions.Player.Move.performed += this.OnMovePerformed;
            this._playerInputActions.Player.Move.canceled += this.OnMoveCanceled;

            this._playerInputActions.Player.Zoom.performed += this.OnZoomPerformed;
            this._playerInputActions.Player.Zoom.canceled += this.OnZoomCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            this._cameraController.SetMoveInput(input);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            this._cameraController.SetMoveInput(Vector2.zero);
        }

        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            this._cameraController.SetZoomInput(input.y);
        }

        private void OnZoomCanceled(InputAction.CallbackContext context)
        {
            this._cameraController.SetZoomInput(0f);
        }
    }
}

