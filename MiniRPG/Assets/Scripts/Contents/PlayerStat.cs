using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            int newLevel = 1;
            while (true)
            {
                Data.Stat stat;
                if (Manager.Data.StatDict.TryGetValue(newLevel + 1, out stat) == false)
                    break;
                if (_exp < stat.minExp)
                    break;
                newLevel++;
            }
            if (Level != newLevel)
            {
                Debug.Log("level up");
                Level = newLevel;
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        Level = 1;
        MoveSpeed = 10.0f;
        Exp = 0;
        Gold = 0;
        SetStat(Level);
    }

    void SetStat(int level)
    {
        Data.Stat statData = Manager.Data.StatDict[level];
        Hp = statData.maxHp;
        MaxHp = statData.maxHp;
        Attack = statData.attack;
        Defense = statData.defense;
    }
}
