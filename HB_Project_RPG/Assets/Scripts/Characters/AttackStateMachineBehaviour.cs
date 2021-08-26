using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachineBehaviour : StateMachineBehaviour
{
    // AttackTrigger �� �ߵ��ϰ� �Ǹ� AttackStateMachine ���� �����ϰ� �Ǵµ�
    // ���� �ÿ� �ش� �̺�Ʈ�� �߻��ȴ�.
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AttackStateController>()?.OnStartOfAttackState();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AttackStateController>()?.OnEndOfAttackState();

        //animator?.GetComponent<EnemyController_Attackable>()?.StateMachine.ChangeState<IdleState>();
        //animator.gameObject.GetComponent<PlayerController>()?.StateMachine.ChangeState<PlayerIdleState>();
    }
}
