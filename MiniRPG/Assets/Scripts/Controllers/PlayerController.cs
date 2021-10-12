using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int _speed = 10;

    bool _moveToDest = false;
    Vector3 _dest;

    void Start()
    {
        Manager.Input.KeyAction -= OnKeyBoard;
        Manager.Input.KeyAction += OnKeyBoard;
        Manager.Input.MouseAction -= OnMouseClicked;
        Manager.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        if (_moveToDest)
        {
            float dist = (_dest - transform.position).magnitude;
            Vector3 dir = (_dest - transform.position).normalized;
            Vector3 move = Mathf.Clamp(_speed * Time.deltaTime, 0, dist) * dir;

            if (dist < 0.0001f)
            {
                _moveToDest = false;
            }
            else
            {
                transform.position += move;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            }
        }
    }

    void OnKeyBoard()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0f, 1f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0f, -1f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 10f * Time.deltaTime);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
            // transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 10f * Time.deltaTime);
            transform.position += Vector3.back * Time.deltaTime * _speed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 10f * Time.deltaTime);
            transform.position += Vector3.right * Time.deltaTime * _speed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 10f * Time.deltaTime);
            transform.position += Vector3.left * Time.deltaTime * _speed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
    }

    void OnMouseClicked(Define.MouseEvent mouseEvent)
    {
        if (mouseEvent != Define.MouseEvent.Click)
            return;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        RaycastHit raycastHit;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        if (Physics.Raycast(ray, out raycastHit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _dest = raycastHit.point;
            _moveToDest = true;
        }
    }
}