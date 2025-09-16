using UnityEngine;

namespace Infrastructure.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _zoomSpeed = 2f;

        [Header("Zoom limits")]
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 50f;

        private Vector3 _moveInput;
        private float _zoomInput;
        private Transform _cameraTransform;

        private void Awake()
        {
            this._cameraTransform = UnityEngine.Camera.main.transform;
        }

        private void Update()
        {
            this.HandleMovement();
            this.HandleZoom();
        }

        public void SetMoveInput(Vector2 input) => this._moveInput = new Vector3(input.x, 0, input.y);
        public void SetZoomInput(float input) => this._zoomInput = input;

        private void HandleMovement()
        {
            if (this._moveInput.magnitude > 0.1f)
            {
                Vector3 moveDirection = this._cameraTransform.TransformDirection(this._moveInput);
                moveDirection.y = 0; // Оставляем только горизонтальное движение
                moveDirection.Normalize();

                this._cameraTransform.position += moveDirection * this._moveSpeed * Time.deltaTime;
            }
        }

        private void HandleZoom()
        {
            if (Mathf.Abs(this._zoomInput) > 0.1f)
            {
                float currentZoom = this._cameraTransform.localPosition.y;
                float newZoom = Mathf.Clamp(currentZoom - this._zoomInput * this._zoomSpeed, this._minZoom, this._maxZoom);
                this._cameraTransform.localPosition = new Vector3(0, newZoom, 0);
            }
        }
    }
}
