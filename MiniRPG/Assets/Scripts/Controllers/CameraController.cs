using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    GameObject _player = null;

    [SerializeField]
    Vector3 _delta = new Vector3(0, 6, -5);

    public void SetCamera(GameObject player) { _player = player; }

    void Start()
    {

    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
                return;

            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {
                Vector3 dist = (hit.point - _player.transform.position) * 0.8f;
                transform.position = _player.transform.position + dist;
                transform.LookAt(_player.transform);
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }

        }

    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
