using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    private Animator animator;
    private CharacterController controller;

    private int hashMove = Animator.StringToHash("Move");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
    }
    public override void OnEnter()
    {
        animator?.SetBool(hashMove, false);
        controller?.Move(Vector3.zero);
    }

    public override void OnUpdate(float deltaTime)
    {
        Vector3 targetPoint = context.ClickToMove();
        Transform targetTransform = context.clickToInteract();

        // Click to move
        if (targetPoint != context.transform.position)
        {
            targetTransform = null;
            context.hitTransform = null;

            stateMachine.ChangeState<PlayerMoveState>();
        }

        // Click to interaction
        if (targetTransform && targetTransform.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            if (targetTransform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (context.IsAttackable)
                    stateMachine.ChangeState<PlayerAttackState>();
                else
                    stateMachine.ChangeState<PlayerMoveState>();
            }
            else
                stateMachine.ChangeState<PlayerMoveState>();
        }
    }
}