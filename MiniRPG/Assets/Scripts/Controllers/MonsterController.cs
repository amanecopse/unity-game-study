using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    [SerializeField]
    Stat _stat;
    [SerializeField]
    float _scanRange = 10.0f;
    [SerializeField]
    float _attackRange = 2.0f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetOrAddComponent<Stat>();
        if (GetComponentInChildren<UI_HPBar>() == null)
            Manager.UI.MakeWorldSpaceUI<UI_HPBar>(parent: transform);
    }

    protected override void UpdateIdle()
    {
        _targetObject = Manager.Game.GetPlayer();
        if (!_targetObject.IsValid())
            return;
        if ((_targetObject.transform.position - transform.position).magnitude < _scanRange)
        {
            State = Define.State.Moving;
        }
    }

    protected override void UpdateMoving()
    {
        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
        if (_targetObject != null)
        {
            _dest = _targetObject.transform.position;
            if ((_dest - transform.position).magnitude <= _attackRange)
            {
                nav.destination = transform.position;
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = (_dest - transform.position).normalized;
        nav.destination = _targetObject.transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
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

            _dest = _targetObject.transform.position;
            if (targetStat.Hp > 0)
            {
                if ((_dest - transform.position).magnitude > _attackRange)
                {
                    State = Define.State.Moving;
                }
                else
                {
                    State = Define.State.Skill;
                }
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
