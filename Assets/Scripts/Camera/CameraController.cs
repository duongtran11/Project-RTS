using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameInput _input;
    [SerializeField] private CinemachineOrbitalFollow _cameraOrbitFollow;

    [Header("Camera Settings")]
    [SerializeField] private float PanSpeed;
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float ZoomSpeed;
    [SerializeField] private float _edgePadding;
    private Vector2 _panInput;
    private float _mouseLeftInput;
    private float _mouseRightInput;
    private float _mouseMiddleInput;
    private Vector2 _mouseDeltaInput;
    private Vector2 _mousePositionInput;
    
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
        _input.Enable();
    }
    void Update()
    {
        HandleCameraPan();
        HandleCameraRotate();
        HandleCameraZoom();
    }
    void HandleCameraPan()
    {
        transform.position += PanSpeed * Time.deltaTime * new Vector3(_panInput.x, 0f, _panInput.y);

        // Edge pan
        var centerPoint = new Vector2(Screen.width, Screen.height) / 2f;
        if (_mousePositionInput.x <= _edgePadding
            || _mousePositionInput.x >= Screen.width - _edgePadding
            || _mousePositionInput.y <= _edgePadding
            || _mousePositionInput.y >= Screen.height - _edgePadding)
        {
            var panDirection = new Vector2(_mousePositionInput.x - centerPoint.x, _mousePositionInput.y - centerPoint.y).normalized;
            transform.position += PanSpeed * Time.deltaTime * new Vector3(panDirection.x, 0f, panDirection.y);
        }
    }
    void HandleCameraRotate()
    {

    }
    void HandleCameraZoom()
    {

    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            Cursor.lockState = CursorLockMode.Confined;
    }
}
