using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    // ������Ƽ �������̽�
    // ���� ���� ���� �� �켱������ ��Ÿ�ӿ� ���� ���õ� ���� �ൿ�� CurrentAttackBehaviour �� ������ �ִ´�.
    AttackBehaviour CurrentAttackBehaviour
    {
        get;
    }

    // �ִϸ����Ϳ��� ������ AttackTirgger �� �ߵ��� �� AttackIndex �� ���� ���� ����� �����Ѵ�.
    void OnExecuteAttack(int attackIndex);
}
