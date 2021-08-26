using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachineBehaviour : StateMachineBehaviour
{
    // AttackTrigger 가 발동하게 되면 AttackStateMachine 으로 진입하게 되는데
    // 진입 시에 해당 이벤트가 발생된다.
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
