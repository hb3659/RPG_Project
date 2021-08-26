using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    // 대화 주체(NPC)
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
