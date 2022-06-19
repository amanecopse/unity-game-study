using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    [SerializeField]
    int _speed = 10;
    Vector3 _dest;
    GameObject _targetObject = null;
    int _mouseMoveMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);

    bool _stopSkill;

    PlayerState _state = PlayerState.Idle;
    PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (value)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }




    void Start()
    {
        Manager.Input.KeyAction -= OnKeyBoard;
        Manager.Input.KeyAction += OnKeyBoard;
        Manager.Input.MouseAction -= OnMouseEvent;
        Manager.Input.MouseAction += OnMouseEvent;
    }

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        if (_targetObject != null)
        {
            _dest = _targetObject.transform.position;
            if ((_dest - transform.position).magnitude <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        float dist = (_dest - transform.position).magnitude;
        Vector3 dir = (_dest - transform.position).normalized;

        if (dist < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            Vector3 moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dist) * dir;
            NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
            nav.Move(moveDist);

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateIdle()
    {
    }

    void UpdateSkill()
    {
        if (_targetObject != null)
        {
            Vector3 dir = _targetObject.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
        }
    }

    void Update()
    {
        switch (State)
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
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }
    }

    void OnHitEvent()
    {
        if (_stopSkill)
        {
            State = PlayerState.Idle;
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

    void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (State == PlayerState.Die)
            return;

        switch (State)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Idle:
                OnMouseEvent_IdleMoving(mouseEvent);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleMoving(mouseEvent);
                break;
            case PlayerState.Skill:
                if (mouseEvent == Define.MouseEvent.PointerUp)
                    _stopSkill = true;
                break;
        }
    }

    void OnMouseEvent_IdleMoving(Define.MouseEvent mouseEvent)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        RaycastHit raycastHit;
        bool isHit = Physics.Raycast(ray, out raycastHit, 100.0f, _mouseMoveMask);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerDown:
                if (isHit)
                {
                    if (raycastHit.transform.gameObject.layer == (int)Define.Layer.Ground)
                        _targetObject = null;
                    else if (raycastHit.transform.gameObject.layer == (int)Define.Layer.Monster)
                        _targetObject = raycastHit.transform.gameObject;
                    _dest = raycastHit.point;
                    State = PlayerState.Moving;
                    _stopSkill = false;
                }
                break;
            case Define.MouseEvent.Press:
                if ((_targetObject == null) && isHit)
                    _dest = raycastHit.point;
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}