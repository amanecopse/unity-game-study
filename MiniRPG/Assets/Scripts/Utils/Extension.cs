using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindUIEvent(this GameObject go, Action<PointerEventData> handler, Define.UIEvent eventEnum)
    {
        UI_EventHandler evn = Util.GetOrAddComponent<UI_EventHandler>(go);
        switch (eventEnum)
        {
            case Define.UIEvent.Click:
                evn.OnClickHandler -= handler;
                evn.OnClickHandler += handler;
                break;
            case Define.UIEvent.Drag:
                evn.OnDragHandler -= handler;
                evn.OnDragHandler += handler;
                break;
        }
    }
}
