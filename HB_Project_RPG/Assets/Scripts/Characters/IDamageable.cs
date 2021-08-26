using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // ���ݹ��� ����� ����ִ� �������� üũ �ϱ� ���� ������Ƽ
    bool IsAlive
    {
        get;
    }

    // Ÿ���� ���� �� �߻��Ǵ� ����Ʈ �������� ������ �ִ´�.
    void TakeDamage(float damage, GameObject hitEffectPrefabs);
}
