using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격에 대한 행동을 구현하는 컴포넌트
// 애니메이터에서 공격 애니메이션의 상태를 알 수 있도록 한다.
public class AttackStateController : MonoBehaviour
{
    // delegate 를 통해 EventTrigger 를 조정
    public delegate void OnEnterAttakState();
    public delegate void OnExitAttackState();

    // delegate 를 조정할 수 있는 함수
    public OnEnterAttakState enterAttackStateHandler;
    public OnExitAttackState exitAttackStateHandler;

    // SubStateMachine 내에 애니메이터 상태가 존재하는지 식별하기 위한 프로퍼티
    // 외부에서 값 변경 불가능
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
    // 아래의 두 함수는 애니메이터에서 SubStateMachine 으로 진입 시 호출되도록 한다.
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
    // 애니메이션에서 호출될 함수
    // 여러 공격 애니메이션에서 동일한 이름의 함수를 사용
    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAttackable>()?.OnExecuteAttack(attackIndex);
    }
    #endregion Helper Methods
}
