using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 영향을 주는 능력치
public enum CharacterAttribute
{
    Strength,
    Intellect,
    Agility,
    Stamina,
}

[Serializable]
public class ItemBuff
{
    public CharacterAttribute stat;
    public int value;

    // 능력치의 최소값과 최대값
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    public int Min => min;
    public int Max => max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;

        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int v)
    {
        v += value;
    }
}
