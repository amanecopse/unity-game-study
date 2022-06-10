using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Util
{

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static GameObject FindChild(GameObject go, string name, bool recursive = true)
    {
        if (recursive)
        {
            return FindChild<Transform>(go, name, true).gameObject;
        }
        else
        {
            return go.transform.Find(name).gameObject;
        }
    }

    public static T FindChild<T>(GameObject go, string name, bool recursive = true) where T : UnityEngine.Object
    {
        if (recursive)
        {
            T[] components = go.GetComponentsInChildren<T>();
            foreach (T component in components)
            {
                if (component.name == name)
                    return component;
            }
        }
        else
        {
            return go.transform.Find(name).gameObject.GetComponent<T>();
        }
        return null;
    }
}
