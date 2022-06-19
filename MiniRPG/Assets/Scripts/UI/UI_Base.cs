using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objectDict = new Dictionary<Type, UnityEngine.Object[]>();//Type: 컴포넌트의 타입(Button, Text...)

    protected void Start()
    {
        Init();
    }

    protected void Bind<T>(Type enumType) where T : UnityEngine.Object//enum에 있는 이름인 컴포넌트들을 바인드한다.
    {
        string[] names = Enum.GetNames(enumType);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.GetLength(0)];
        _objectDict.Add(typeof(T), objects);
        for (int i = 0; i < names.GetLength(0); i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i]);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i]);
            }
        }
    }

    protected T Get<T>(int enumIdx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;
        if (_objectDict.TryGetValue(typeof(T), out objects) == false)
            return null;
        return objects[enumIdx] as T;
    }

    protected GameObject GetGameObject(int enumIdx)
    {
        return Get<GameObject>(enumIdx);
    }

    protected Button GetButton(int enumIdx)
    {
        return Get<Button>(enumIdx);
    }

    protected Text GetText(int enumIdx)
    {
        return Get<Text>(enumIdx);
    }

    protected Image GetImage(int enumIdx)
    {
        return Get<Image>(enumIdx);
    }

    protected void BindUIEvent(GameObject go, Action<PointerEventData> handler, Define.UIEvent eventEnum)
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

    protected abstract void Init();
}
