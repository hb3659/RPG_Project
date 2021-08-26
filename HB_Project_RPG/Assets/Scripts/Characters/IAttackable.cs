using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    // 프로퍼티 인터페이스
    // 여러 개의 공격 중 우선순위나 쿨타임에 대해 선택된 공격 행동을 CurrentAttackBehaviour 에 가지고 있는다.
    AttackBehaviour CurrentAttackBehaviour
    {
        get;
    }

    // 애니메이터에서 설정된 AttackTirgger 가 발동될 때 AttackIndex 에 따라 공격 모션을 결정한다.
    void OnExecuteAttack(int attackIndex);
}
