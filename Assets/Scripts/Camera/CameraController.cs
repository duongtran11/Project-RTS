using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameInput _input;
    [SerializeField] private CinemachineOrbitalFollow _cameraOrbitFollow;
    private Vector2 _panInput;
    private float _mouseLeftInput;
    private float _mouseRightInput;
    private float _mouseMiddleInput;
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

    }
    void HandleCameraRotate()
    {

    }
    void HandleCameraZoom()
    {

    }
}
