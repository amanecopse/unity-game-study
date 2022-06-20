using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path.Substring(path.LastIndexOf("/") + 1);
            GameObject go = Manager.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Manager.Pool.Pop(original, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Manager.Pool.Push(poolable);
            return;
        }
        UnityEngine.Object.Destroy(go);
    }
}
