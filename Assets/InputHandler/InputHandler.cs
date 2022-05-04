using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerAction input;
    public bool pressLeft = false;
    public bool pressRight = false;
    public bool pressUp = false;
    public bool pressDown = false;

    public void LeftInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressLeft = true;
        }
    }

    public void RightInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressRight = true;
        }
    }

    public void UpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressUp = true;
        }
    }

    public void DownInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressDown = true;
        }
    }

    public void UsedLeftInput()
    {
        pressLeft = false;
    }

    public void UsedRightInput()
    {
        pressRight = false;
    }
}
