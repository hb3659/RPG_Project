using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehaviour : AttackBehaviour
{
    public ManualCollision attackCollision;
    public HitEffectSpawner hitSpawner;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        // attackCollision ���� �ڽ� �ȿ� ���� Ÿ�ٵ鿡 ���� �ݶ��̴��� ��´�.
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
