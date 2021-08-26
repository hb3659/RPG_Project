using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected float _HP;
    [SerializeField]
    protected float _maxHP;
    [SerializeField]
    protected float _offensivePower;
    [SerializeField]
    protected float _defensivePower;

    public float HP { get { return _HP; } set { _HP = value; } }
    public float MaxHP { get { return _maxHP; } set { _maxHP = value; } }
    public float OffensivePower { get { return _offensivePower; } set { _offensivePower = value; } }
    public float DefensivePower { get { return _defensivePower; } set { _defensivePower = value; } }
}
