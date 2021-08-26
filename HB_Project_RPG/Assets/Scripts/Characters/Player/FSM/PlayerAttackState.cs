using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : State<PlayerController>
{
    //private Animator animator;
    //private int hashAttack = Animator.StringToHash("Attack");
    //private int attackIndexHash = Animator.StringToHash("AttackIndex");
    //private float rotateSpeed = 5f;

    //public override void OnInitialized()
    //{
    //    animator = context.GetComponent<Animator>();
    //}
    //public override void OnEnter()
    //{
    //    if (context.IsAttackable)
    //    {
    //        attackIndexHash = Random.Range(0, 2);

    //        animator.SetInteger("AttackIndex", attackIndexHash);
    //        animator?.SetTrigger(hashAttack);
    //    }
    //    else
    //        stateMachine.ChangeState<PlayerIdleState>();
    //}

    //public override void OnUpdate(float deltaTime)
    //{
    //    if (context.hitTransform)
    //    {
    //        Vector3 dir = context.hitTransform.position - context.transform.position;
    //        context.transform.rotation =
    //            Quaternion.Lerp(context.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
    //    }
    //    Transform targetTransform = context.clickToInteract();
    //    Vector3 targetPoint = context.ClickToMove();

    //    if (targetPoint != context.transform.position)
    //        context.hitTransform = null;
    //}

    //public override void OnExit()
    //{
    //    animator.ResetTrigger(hashAttack);
    //}

    private Animator animator;
    private int hashAttack = Animator.StringToHash("Attack");
    private int attackIndexHash = Animator.StringToHash("AttackIndex");
    private AttackStateController attackStateController;
    private IAttackable attackable;
    private float rotateSpeed = 5f;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

    public override void OnEnter()
    {
        if (attackable == null || attackable.CurrentAttackBehaviour == null)
        {
            stateMachine.ChangeState<PlayerIdleState>();
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

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<PlayerIdleState>();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (context.hitTransform)
        {
            Vector3 dir = context.hitTransform.position - context.transform.position;
            context.transform.rotation =
                Quaternion.Lerp(context.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
        Transform targetTransform = context.clickToInteract();
        Vector3 targetPoint = context.ClickToMove();

        if (targetPoint != context.transform.position)
            context.hitTransform = null;
    }

    public override void OnExit()
    {
        animator.ResetTrigger(hashAttack);
    }
}
