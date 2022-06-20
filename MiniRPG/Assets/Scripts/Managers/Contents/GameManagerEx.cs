using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GameManagerEx
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Manager.Resource.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Player:
                _player = go;
                break;
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                break;
        }

        return go;
    }

    Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;
        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        switch (GetWorldObjectType(go))
        {
            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
            case Define.WorldObject.Monster:
                if (_monsters.Contains(go))
                    _monsters.Remove(go);
                break;
        }
        Manager.Resource.Destroy(go);
    }
}
