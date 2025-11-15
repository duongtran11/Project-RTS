using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameInput _input;
    private Camera _mainCamera;
    [SerializeField] private CinemachineOrbitalFollow _cameraOrbitFollow;

    [Header("Camera Settings")]
    [SerializeField] private float PanSpeed;
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float ZoomSpeed;
    [SerializeField] private bool _invertHorizontal;
    [SerializeField] private bool _invertVertical;
    [SerializeField] private bool _invertZoom;
    [SerializeField] private float _edgePadding;
    private Vector2 _panInput;
    private float _mouseLeftInput;
    private float _mouseRightInput;
    private float _mouseMiddleInput;
    private Vector2 _mouseDeltaInput;
    private Vector2 _mousePositionInput;
    private float _mouseZoomInput;
    private bool _hasFocus;

    void Awake()
    {
        _input = new GameInput();
        _input.Camera.Pan.performed += ctx => _panInput = ctx.ReadValue<Vector2>();
        _input.Camera.Pan.canceled += ctx => _panInput = Vector2.zero;
        _input.Camera.Zoom.performed += ctx => _mouseMiddleInput = ctx.ReadValue<float>();
        _input.Camera.Zoom.canceled += ctx => _mouseMiddleInput = 0f;
        _input.Camera.MouseLeft.performed += ctx => _mouseLeftInput = ctx.ReadValue<float>();
        _input.Camera.MouseLeft.canceled += ctx => _mouseLeftInput = 0f;
        _input.Camera.MouseRight.performed += ctx => _mouseRightInput = ctx.ReadValue<float>();
        _input.Camera.MouseRight.canceled += ctx => _mouseRightInput = 0f;
        _input.Camera.MouseDelta.performed += ctx => _mouseDeltaInput = ctx.ReadValue<Vector2>();
        _input.Camera.MouseDelta.canceled += ctx => _mouseDeltaInput = Vector2.zero;
        _input.Camera.MousePosition.performed += ctx => _mousePositionInput = ctx.ReadValue<Vector2>();
        _input.Camera.MousePosition.canceled += ctx => _mousePositionInput = Vector2.zero;
        _input.Camera.Zoom.performed += ctx => _mouseZoomInput = ctx.ReadValue<float>();
        _input.Camera.Zoom.canceled += ctx => _mouseZoomInput = 0f;
        _input.Enable();
        _mainCamera = Camera.main;
    }
    void Update()
    {
        if (!_hasFocus)
        {
            return;
        }
        HandleCameraPan();
        HandleCameraRotate();
        HandleCameraZoom();
    }
    void HandleCameraPan()
    {
        transform.position += Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * new Vector3(_panInput.x, 0f, _panInput.y) * PanSpeed * Time.deltaTime;

        // Edge pan
        if (_mousePositionInput.x <= _edgePadding
            || _mousePositionInput.x >= Screen.width - _edgePadding)
        {
            var direction = _mousePositionInput.x > Screen.width / 2f ? 1f : -1f;
            transform.position += Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * transform.right * direction * PanSpeed * Time.deltaTime;
        }

        if (_mousePositionInput.y <= _edgePadding
            || _mousePositionInput.y >= Screen.height - _edgePadding)
        {
            var direction = _mousePositionInput.y > Screen.height / 2f ? 1f : -1f;
            transform.position += Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * transform.forward * direction * PanSpeed * Time.deltaTime;
        }
    }
    void HandleCameraRotate()
    {
        if (_mouseRightInput != 0f)
        {
            _cameraOrbitFollow.HorizontalAxis.Value += (_invertHorizontal ? -1f : 1f) * _mouseDeltaInput.x * RotateSpeed * Time.deltaTime;
            _cameraOrbitFollow.VerticalAxis.Value += (_invertVertical ? -1f : 1f) * _mouseDeltaInput.y * RotateSpeed * Time.deltaTime;
            _cameraOrbitFollow.VerticalAxis.Value = Mathf.Clamp(_cameraOrbitFollow.VerticalAxis.Value, _cameraOrbitFollow.VerticalAxis.Range.x, _cameraOrbitFollow.VerticalAxis.Range.y);
        }
    }
    void HandleCameraZoom()
    {
        _cameraOrbitFollow.RadialAxis.Value += (_invertZoom ? -1f : 1f) * _mouseZoomInput * ZoomSpeed * Time.deltaTime;
        _cameraOrbitFollow.RadialAxis.Value = Mathf.Clamp(_cameraOrbitFollow.RadialAxis.Value, _cameraOrbitFollow.RadialAxis.Range.x, _cameraOrbitFollow.RadialAxis.Range.y);
    }
    void OnApplicationFocus(bool hasFocus)
    {
        _hasFocus = hasFocus;
        if (hasFocus)
            Cursor.lockState = CursorLockMode.Confined;
    }
}
