using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class MoveState : State<EnemyController>
//public class MoveState : State<EnemyController_FOV>
//public class MoveState : State<EnemyController_Patrol>
public class MoveState : State<EnemyController_Attackable>
{
    private Animator animator;
    private CharacterController controller;
    private NavMeshAgent agent;

    private int hashMove = Animator.StringToHash("Move");
    private int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        // 타겟으로 이동
        //agent?.SetDestination(context.target.position);
        agent?.SetDestination(context.Target.position);
        animator?.SetBool(hashMove, true);
    }
    public override void OnUpdate(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // 적의 위치로 목적지를 설정
            //agent.SetDestination(context.target.position);
            agent.SetDestination(context.Target.position);

            // 이동할 거리가 아직 남았다면
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                controller.Move(agent.velocity * deltaTime);
                animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, deltaTime);
            }
        }
        if (!enemy || agent.remainingDistance <= agent.stoppingDistance)
            stateMachine.ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        // Move 상태를 벗어날 때
        animator?.SetBool(hashMove, false);
        animator?.SetFloat(hashMoveSpeed, 0);
        agent.ResetPath();
    }
}
