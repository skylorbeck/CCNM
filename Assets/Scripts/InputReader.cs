using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Global/InputReader")]
public class InputReader : ScriptableObject, CCNM.IPlayerActions, CCNM.IUIActions
{
    public event UnityAction ClickEvent = delegate { };
    public event UnityAction PushAnyButton = delegate { };
    
    private CCNM _ccnm;

    private void OnEnable()
    {
        if (_ccnm == null)
        {
            _ccnm = new CCNM();
            _ccnm.Player.SetCallbacks(this);
            _ccnm.UI.SetCallbacks(this);
        }
        // EnableUI();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
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

    public void OnAny(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PushAnyButton();
        }
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
    public void EnablePlayer()
    {
        _ccnm.Player.Enable();
    }
    public void DisablePlayer()
    {
        _ccnm.Player.Disable();
    }

    public void OnDisable()
    {
        DisablePlayer();
        DisableUI();
    }
}
