using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionRange : MonoBehaviour
{
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float attackRange = 90f;

    public LayerMask targetMask = LayerMask.NameToLayer("Enemy") + LayerMask.NameToLayer("");
}
