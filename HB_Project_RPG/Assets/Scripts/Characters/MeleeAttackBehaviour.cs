using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehaviour : AttackBehaviour
{
    public ManualCollision attackCollision;
    public HitEffectSpawner hitSpawner;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        // attackCollision 에서 박스 안에 들어온 타겟들에 대한 콜라이더를 담는다.
        Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

        foreach (Collider collider in colliders)
        {
            if (collider.name == target.name)
            {
                collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);

                effectPrefab = hitSpawner.Attack();
            }
        }
    }
}
