using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolables = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            if (original == null)
                return;
            Root = new GameObject { name = $"{original.gameObject.name}_Root" }.transform;
            Original = original;
            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        public Poolable Create()
        {
            GameObject clone = Object.Instantiate<GameObject>(Original);
            clone.name = Original.name;
            return clone.GetOrAddComponent<Poolable>();
        }

        public Poolable Pop(Transform parent)
        {
            Poolable clone;
            if (_poolables.Count > 0)
            {
                clone = _poolables.Pop();
            }
            else
                clone = Create();
            clone.isUsing = true;
            clone.gameObject.SetActive(true);
            if (parent == null)
                clone.gameObject.transform.parent = Manager.Scene.CurrentScene.transform;//DontDestroyOnLoad에서 탈출
            clone.gameObject.transform.parent = parent;
            return clone;
        }

        public void Push(Poolable clone)
        {
            if (clone == null)
                return;
            clone.isUsing = false;
            clone.gameObject.SetActive(false);
            clone.gameObject.transform.parent = Root.transform;
            _poolables.Push(clone);
        }
    }

    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }

    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pools.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pools.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }
        return _pools[original.name].Pop(parent);
    }

    public void Push(Poolable clone)
    {
        if (_pools.ContainsKey(clone.name) == false)
        {
            GameObject.Destroy(clone.gameObject);
            return;
        }
        _pools[clone.name].Push(clone);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pools.ContainsKey(name) == false)
            return null;
        return _pools[name].Original;
    }

    public void Close()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child);
        }
        _pools.Clear();
    }
}
