using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Species
{
    Slime,
    Bat,
    Skeleton,
}

public class EnemyStat : Stat
{
    public Species species;

    protected int _gold;
    protected float _exp;

    public int Gold { get { return _gold; } set { _gold = value; } }
    public float Exp { get { return _exp; } set { _exp = value; } }

    public void Start()
    {
        _HP = 100;
        _maxHP = 100;
        _offensivePower = 60;
        _defensivePower = 40;

        MonsterSpecies(species);
    }

    public void MonsterSpecies(Species species)
    {
        switch (species)
        {
            case Species.Slime:
                _offensivePower = _offensivePower - 20;
                _defensivePower = _defensivePower - 20;
                Gold = Random.Range(5, 12);
                Exp = Random.Range(3f, 10f);

                break;

            case Species.Bat:
                _HP = _HP + 20;
                _maxHP = _maxHP + 20;
                _offensivePower = _offensivePower - 10;
                _defensivePower = _defensivePower - 10;

                break;

            case Species.Skeleton:
                _HP = _HP + 50;
                _maxHP = _maxHP + 50;
                break;
        }
    }
}
