using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Global/InputReader")]
public class InputReader : ScriptableObject, CCNM.IUIActions
{
    public event UnityAction ClickEvent = delegate { };
    public event UnityAction PushAnyButton = delegate { };
    public event UnityAction ButtonUp = delegate { };
    public event UnityAction ButtonDown = delegate { };
    public event UnityAction ButtonLeft = delegate { };
    public event UnityAction ButtonRight = delegate { };
    public event UnityAction PadUp = delegate { };
    public event UnityAction PadDown = delegate { };
    public event UnityAction PadLeft = delegate { };
    public event UnityAction PadRight = delegate { };
    public event UnityAction PadAny = delegate { };

    private CCNM _ccnm;

    private void OnEnable()
    {
        if (_ccnm == null)
        {
            _ccnm = new CCNM();
            _ccnm.UI.SetCallbacks(this);
        }
        // EnableUI();
    }


    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }

        Vector2 value = context.ReadValue<Vector2>();
        if (value.y > 0.5f)
        {
            PadUp();
            PadAny();
        }
        else if (value.y < -0.5f)
        {
            PadDown();
            PadAny();
        }

        if (value.x > 0.5f)
        {
            PadRight();
            PadAny();
        }
        else if (value.x < -0.5f)
        {
            PadLeft();
            PadAny();
        }
    }

    public void OnButtonDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
            ButtonDown();
        }
    }

    public void OnButtonUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
            ButtonUp();
        }
    }

    public void OnButtonLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
            ButtonLeft();
        }
    }

    public void OnButtonRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
            ButtonRight();
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ClickEvent();
            PushAnyButton();
        }
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {

    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {

    }

    public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

    public void EnableUI()
    {
        _ccnm.UI.Enable();
    }

    public void DisableUI()
    {
        _ccnm.UI.Disable();
    }


    public void OnDisable()
    {
        DisableUI();
    }
}