using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public PlayerAction Input;
    private void Awake()
    {
        Input = new PlayerAction();
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
        Input.Player.Enable();
    }

    private void OnDisable()
    {
        Input.Player.Disable();
    }
}
