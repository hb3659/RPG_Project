using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ݿ� ���� �ൿ�� �����ϴ� ������Ʈ
// �ִϸ����Ϳ��� ���� �ִϸ��̼��� ���¸� �� �� �ֵ��� �Ѵ�.
public class AttackStateController : MonoBehaviour
{
    // delegate �� ���� EventTrigger �� ����
    public delegate void OnEnterAttakState();
    public delegate void OnExitAttackState();

    // delegate �� ������ �� �ִ� �Լ�
    public OnEnterAttakState enterAttackStateHandler;
    public OnExitAttackState exitAttackStateHandler;

    // SubStateMachine ���� �ִϸ����� ���°� �����ϴ��� �ĺ��ϱ� ���� ������Ƽ
    // �ܺο��� �� ���� �Ұ���
    public bool IsInAttackState
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        enterAttackStateHandler = new OnEnterAttakState(EnterAttackState);
        exitAttackStateHandler = new OnExitAttackState(ExitAttackState);
    }

    #region Helper Methods
    // �Ʒ��� �� �Լ��� �ִϸ����Ϳ��� SubStateMachine ���� ���� �� ȣ��ǵ��� �Ѵ�.
    public void OnStartOfAttackState()
    {
        IsInAttackState = true;
        enterAttackStateHandler();
    }
    public void OnEndOfAttackState()
    {
        IsInAttackState = false;
        exitAttackStateHandler();
    }
    
    private void EnterAttackState()
    {

    }
    private void ExitAttackState()
    {

    }
    // �ִϸ��̼ǿ��� ȣ��� �Լ�
    // ���� ���� �ִϸ��̼ǿ��� ������ �̸��� �Լ��� ���
    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAttackable>()?.OnExecuteAttack(attackIndex);
    }
    #endregion Helper Methods
}
