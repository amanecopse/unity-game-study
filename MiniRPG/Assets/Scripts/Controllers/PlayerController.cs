using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    [SerializeField]
    PlayerStat _stat;
    int _mouseMoveMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);
    bool _stopSkill;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        if (GetComponentInChildren<UI_HPBar>() == null)
            Manager.UI.MakeWorldSpaceUI<UI_HPBar>(parent: transform);

        Manager.Input.KeyAction -= OnKeyBoard;
        Manager.Input.KeyAction += OnKeyBoard;
        Manager.Input.MouseAction -= OnMouseEvent;
        Manager.Input.MouseAction += OnMouseEvent;
    }

    protected override void UpdateDie()
    {

    }

    protected override void UpdateMoving()
    {
        if (_targetObject != null)
        {
            _dest = _targetObject.transform.position;
            if ((_dest - transform.position).magnitude <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        float dist = (_dest - transform.position).magnitude;
        Vector3 dir = (_dest - transform.position).normalized;

        if (dist < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {

            // NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
            // nav.Move(moveDist * dir);

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {//건물이나 벽 위로 이동 방지
                if (Input.GetMouseButton(0) == false)//마우스 누른 상태라면 이동 애니메이션 유지
                    State = Define.State.Idle;//클릭만 했으면 그냥 대기 상태로
                return;
            }
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dist);
            transform.position += moveDist * dir;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateIdle()
    {
    }

    protected override void UpdateSkill()
    {
        if (_targetObject != null)
        {
            Vector3 dir = _targetObject.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_targetObject != null)
        {
            Stat targetStat = _targetObject.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }


        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
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
            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
            // transform.Translate(Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 10f * Time.deltaTime);
            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 10f * Time.deltaTime);
            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 10f * Time.deltaTime);
            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
            //transform.Translate(Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
        }
    }

    void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (State == Define.State.Die)
            return;

        switch (State)
        {
            case Define.State.Die:
                break;
            case Define.State.Idle:
                OnMouseEvent_IdleMoving(mouseEvent);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleMoving(mouseEvent);
                break;
            case Define.State.Skill:
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
                    State = Define.State.Moving;
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