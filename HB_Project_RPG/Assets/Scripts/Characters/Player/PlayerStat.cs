using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected float _mana;
    [SerializeField]
    protected float _maxMana;
    [SerializeField]
    protected float _exp;
    [SerializeField]
    protected int _gold;


    public int Level { get { return _level; } set { _level = value; } }
    public float Mana { get { return _mana; } set { _mana = value; } }
    public float MaxMana { get { return _maxMana; } set { _maxMana = value; } }
    public float Exp { get { return _exp; } set { _exp = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Awake()
    {
        _level = 1;
        _HP = 300;
        _maxHP = 300;
        _mana = 200;
        _maxMana = 200;
        _offensivePower = 50;
        _defensivePower = 30;
        _exp = 0f;
        _gold = 0;
    }
}
