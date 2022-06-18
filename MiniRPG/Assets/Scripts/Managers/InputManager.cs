using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputManager
{
    public Action KeyAction;
    public Action<Define.MouseEvent> MouseAction;

    bool _mousePressed = false;
    float _pressedTime;

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }
        if (MouseAction != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (Input.GetMouseButton(0))
            {
                if (!_mousePressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);

                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _mousePressed = true;
            }
            else
            {
                if (_mousePressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    else
                    {
                        MouseAction.Invoke(Define.MouseEvent.PointerUp);
                    }

                }
                _pressedTime = 0;
                _mousePressed = false;
            }
        }
    }

    public void Close()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
