using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class AttackState : State<EnemyController>
//public class AttackState : State<EnemyController_FOV>
//public class AttackState : State<EnemyController_Patrol>
//public class AttackState : State<EnemyController_Attackable>
//{
//    private Animator animator;

//    private int hashAttack = Animator.StringToHash("Attack");

//    public override void OnInitialized()
//    {
//        animator = context.GetComponent<Animator>();
//    }

//    public override void OnEnter()
//    {
//        if (context.IsAvailableAttack)
//            animator?.SetTrigger(hashAttack);
//        else
//            stateMachine.ChangeState<IdleState>();
//    }

//    public override void OnUpdate(float deltaTime)
//    {
        
//    }
//}

public class AttackState : State<EnemyController_Attackable>
{
    private Animator animator;
    private AttackStateController attackStateController;
    private IAttackable attackable;

    private int hashAttack = Animator.StringToHash("Attack");
    private int attackIndexHash = Animator.StringToHash("AttackIndex");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

    public override void OnEnter()
    {
        // 공격 가능한 상태가 아니거나 (IAttackable 이 상속되지 않았다) 
        // CurrentAttackBehaviour 가 비었다면 (쿨타임이 만족하지 않는다)
        if (attackable == null || attackable.CurrentAttackBehaviour == null)
        {
            stateMachine.ChangeState<IdleState>();
            return;
        }

        attackStateController.enterAttackStateHandler += OnEnterAttackState;
        attackStateController.exitAttackStateHandler += OnExitAttackState;

        animator?.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
        animator?.SetTrigger(hashAttack);
    }

    public void OnEnterAttackState()
    {

    }

    // 공격 상태에 진입 시에는 해줄 일이 없지만 빠져 나올 시에는 다시 IdleState 로 돌아가야 한다.
    public void OnExitAttackState()
    {
        stateMachine.ChangeState<IdleState>();
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnExit()
    {
        attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        attackStateController.exitAttackStateHandler -= OnExitAttackState;
    }
}
