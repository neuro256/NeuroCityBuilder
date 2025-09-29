using UnityEngine;

namespace NeuroCityBuilder.Infrastructure.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _dragSpeed = 5f;
        [SerializeField] private float _smoothTime = 0.15f;

        [Header("Zoom limits")]
        [SerializeField] private float _zoomSpeed = 2f;
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 50f;

        private Vector3 _moveInput;
        private Vector2 _dragInput;
        private float _zoomInput;

        private Transform _cameraTransform;
        private Vector3 _targetPosition;
        private float _targetZoom;
        private Vector3 _currentVelocity;
        private float _zoomVelocity;

        private void Awake()
        {
            this._cameraTransform = UnityEngine.Camera.main.transform;
            this._targetPosition = this._cameraTransform.position;
            this._targetZoom = this._cameraTransform.localPosition.y;
        }

        private void Update()
        {
            this.HandleMovement();
            this.HandleDrag();
            this.HandleZoom();
        }

        public void SetMoveInput(Vector2 input) => this._moveInput = new Vector3(input.x, 0, input.y);
        public void SetZoomInput(float input) => this._zoomInput = input;
        public void SetDragInput(Vector2 input) => this._dragInput = input;

        private void HandleMovement()
        {
            if (this._moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 moveDir = this._cameraTransform.TransformDirection(this._moveInput);
                moveDir.y = 0;
                moveDir.Normalize();
                this._targetPosition += moveDir * this._moveSpeed * Time.deltaTime;
            }

            this._cameraTransform.position = Vector3.SmoothDamp(
                this._cameraTransform.position,
                this._targetPosition,
                ref this._currentVelocity,
                this._smoothTime
            );
        }

        private void HandleDrag()
        {
            if (this._dragInput.sqrMagnitude > 0.01f)
            {
                Vector3 dragMove = new Vector3(-this._dragInput.x, 0, -this._dragInput.y) * this._dragSpeed;
                this._targetPosition += dragMove * Time.deltaTime;
            }
        }

        private void HandleZoom()
        {
            if (Mathf.Abs(this._zoomInput) > 0.01f)
            {
                this._targetZoom = Mathf.Clamp(
                    this._targetZoom - this._zoomInput * this._zoomSpeed,
                    this._minZoom,
                    this._maxZoom
                );
            }

            Vector3 currentPos = this._cameraTransform.localPosition;

            float newY = Mathf.SmoothDamp(
                currentPos.y,
                this._targetZoom,
                ref this._zoomVelocity,
                this._smoothTime
             );

            this._cameraTransform.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
        }
    }
}
