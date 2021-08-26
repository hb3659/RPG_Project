using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : State<PlayerController>
{
    private Animator animator;
    private CharacterController controller;
    private NavMeshAgent agent;

    private int hashMove = Animator.StringToHash("Move");

    private Vector3 targetPoint;
    private Transform targetTransform;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }
    public override void OnEnter()
    {
        animator?.SetBool(hashMove, true);

        if (!context.hitTransform)
            agent?.SetDestination(context.hitPoint);
        else
            agent?.SetDestination(context.hitTransform.position);
    }

    public override void OnUpdate(float deltaTime)
    {
        targetPoint = context.ClickToMove();
        targetTransform = context.clickToInteract();

        float stoppingDistance = agent.stoppingDistance;

        // 새로운 지점이 클릭됨
        if (targetPoint != context.transform.position)
        {
            context.hitTransform = null;
            targetTransform = null;

            agent.SetDestination(targetPoint);

            if (agent.remainingDistance > stoppingDistance)
                controller.Move(agent.velocity * Time.deltaTime);
        }

        // 적이 클릭됨
        if (targetTransform)
        {
            agent.SetDestination(targetTransform.position);
            stoppingDistance = context.attackRange;

            if (agent.remainingDistance > stoppingDistance)
                controller.Move(agent.velocity * Time.deltaTime);
        }

        // 목적지에 도착했을 때
        if (agent.remainingDistance <= stoppingDistance)
        {
            context.pointSpawner.ReturnPointer(context.pointer);
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void OnExit()
    {
        animator.SetBool(hashMove, false);
        agent.ResetPath();
    }
}
