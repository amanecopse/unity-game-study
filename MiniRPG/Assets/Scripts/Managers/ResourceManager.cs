using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }
        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        int idx = go.name.IndexOf("(Clone)");
        if (idx > 0)
        {
            go.name = go.name.Substring(0, idx);
        }
        return go;
    }

    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;
        UnityEngine.Object.Destroy(go, time);
    }
}
