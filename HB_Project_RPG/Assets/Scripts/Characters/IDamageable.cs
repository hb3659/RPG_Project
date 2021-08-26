using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // 공격받을 대상이 살아있는 상태인지 체크 하기 위한 프로퍼티
    bool IsAlive
    {
        get;
    }

    // 타격을 입을 시 발생되는 이펙트 프리팹을 변수로 넣는다.
    void TakeDamage(float damage, GameObject hitEffectPrefabs);
}
