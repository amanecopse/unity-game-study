using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int _speed = 10;
    Vector3 _dest;
    PlayerState _state = PlayerState.Idle;
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    void Start()
    {
        Manager.Input.KeyAction -= OnKeyBoard;
        Manager.Input.KeyAction += OnKeyBoard;
        Manager.Input.MouseAction -= OnMouseClicked;
        Manager.Input.MouseAction += OnMouseClicked;
    }

    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        float dist = (_dest - transform.position).magnitude;
        Vector3 dir = (_dest - transform.position).normalized;

        if (dist < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            Vector3 moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dist) * dir;
            NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
            nav.Move(moveDist);

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _speed);
    }
    void UpdateIdle()
    {
        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
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
        // if (mouseEvent != Define.MouseEvent.Click)
        //     return;

        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        RaycastHit raycastHit;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        if (Physics.Raycast(ray, out raycastHit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _dest = raycastHit.point;
            _state = PlayerState.Moving;
        }
    }
}