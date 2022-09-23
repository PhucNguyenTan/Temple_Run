using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public PlayerAction _input;

    public UnityEvent Left;
    public UnityEvent Right;
    public UnityEvent Down;
    public UnityEvent Up;
    public UnityEvent Pause;

    Camera _mainCam;

    Vector3 _startTouchPos;
    Vector3 _endTouchPos;
    float _startTouchTime;
    float _endTouchTime;

    [SerializeField] float _minSwipeLength = 0.1f;
    [SerializeField] float _maxSwipeTime = 0.5f;
    [SerializeField] float _directionThreshold = .3f;

    private void Awake()
    {
        _input = new PlayerAction();
        _mainCam = Camera.main;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Left.performed += PressLeft;
        _input.Player.Right.performed += PressRight;
        _input.Player.Up.performed += PressUp;
        _input.Player.Down.performed += PressDown;
        _input.Player.PrimaryContact.started += StartedTouch;
        _input.Player.PrimaryContact.canceled += EndTouch;

    }

    private void StartedTouch(InputAction.CallbackContext ctx)
    {
        Vector2 touchPos2D = _input.Player.PrimaryPosition.ReadValue<Vector2>();
        _startTouchPos = Utils.ScreenToWorld(_mainCam, touchPos2D); // !!! This automatically pass Vector2 into a Vector3
        _startTouchTime = (float)ctx.startTime;
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        Vector2 touchPos2D = _input.Player.PrimaryPosition.ReadValue<Vector2>();
        _endTouchPos = Utils.ScreenToWorld(_mainCam, touchPos2D); 
        _endTouchTime = (float)ctx.time;
        DetectSwipe();
    }

    void DetectSwipe()
    {
        float swipeTime = _endTouchTime - _startTouchTime;
        Vector3 swipe = _endTouchPos - _startTouchPos;
        if (swipeTime > _maxSwipeTime || swipe.magnitude < _minSwipeLength) return;

        Vector2 swipeDir = swipe.normalized;
        DetectSwipeDir(swipeDir);
        
    }

    void DetectSwipeDir(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > _directionThreshold)
            Up?.Invoke();
        else if (Vector2.Dot(Vector2.left, direction) > _directionThreshold)
            Left?.Invoke();
        else if (Vector2.Dot(Vector2.right, direction) > _directionThreshold)
            Right?.Invoke();
        else if (Vector2.Dot(Vector2.down, direction) > _directionThreshold)
            Down?.Invoke();
    }

    private void OnDisable()
    {

        _input.Player.Left.performed -= PressLeft;
        _input.Player.Right.performed -= PressRight;
        _input.Player.Up.performed -= PressUp;
        _input.Player.Down.performed -= PressDown;
        _input.Player.Disable();
    }
    private void PressDown(InputAction.CallbackContext obj)
    {
        Down?.Invoke();
    }

    private void PressUp(InputAction.CallbackContext obj)
    {
        Up?.Invoke();
    }

    private void PressRight(InputAction.CallbackContext obj)
    {
        Right?.Invoke();
    }
    private void PressLeft(InputAction.CallbackContext obj)
    {
        Left?.Invoke();
    }

    

    
}
