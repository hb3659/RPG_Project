using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    // ��ȭ ��ü(NPC)
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
