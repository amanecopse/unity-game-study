using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    int _monsterCount = 0;
    int _reserveCount = 0;
    int _maxMonsterCount = 5;
    float _spawnTime = 5.0f;
    float _spawnRange = 5.0f;
    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetMaxMonsterCount(int value) { _maxMonsterCount = value; }

    void Start()
    {
        Manager.Game.OnSpawnEvent -= AddMonsterCount;
        Manager.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _maxMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));
        GameObject go = Manager.Game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>();

        Vector3 targetPos;
        while (true)
        {
            targetPos = UnityEngine.Random.insideUnitSphere * _spawnRange;
            targetPos.y = 0;

            //갈 수 있는 지 탐색
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(targetPos, path))
                break;
        }
        go.transform.position = targetPos;
        _reserveCount--;
    }
}
