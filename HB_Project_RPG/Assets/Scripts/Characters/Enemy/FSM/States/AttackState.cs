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
        // ���� ������ ���°� �ƴϰų� (IAttackable �� ��ӵ��� �ʾҴ�) 
        // CurrentAttackBehaviour �� ����ٸ� (��Ÿ���� �������� �ʴ´�)
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

    // ���� ���¿� ���� �ÿ��� ���� ���� ������ ���� ���� �ÿ��� �ٽ� IdleState �� ���ư��� �Ѵ�.
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
