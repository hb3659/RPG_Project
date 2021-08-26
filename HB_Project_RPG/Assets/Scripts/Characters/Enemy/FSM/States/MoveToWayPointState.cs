using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class IdleState : State<EnemyController>
//public class IdleState : State<EnemyController_FOV>
//public class MoveToWayPointState : State<EnemyController_Patrol>
public class MoveToWayPointState : State<EnemyController_Attackable>
{
    private Animator animator;
    private CharacterController controller;
    private NavMeshAgent agent;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        if(context.targetWayPoint == null)
            context.FindNextWayPoint();

        if (context.targetWayPoint)
        {
            agent?.SetDestination(context.targetWayPoint.position);
            animator?.SetBool(hashMove, true);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        // 계속해서 주변에 적이 있는지를 검색해야한다.
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // 적을 벌견하고 그 적이 공격할 수 있는 적이라면 공격 상태로 전환
            if (context.IsAvailableAttack)
                stateMachine.ChangeState<AttackState>();
            // 아니라면 이동 상태로 전환한다.
            else
                stateMachine.ChangeState<MoveState>();
        }
        else
        {
            // agent 가 이동해야할 경로가 존재하는지 확인하기 위함
            if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {
                Transform nextDest = context.FindNextWayPoint();
                if (nextDest)
                    agent.SetDestination(nextDest.position);

                stateMachine.ChangeState<IdleState>();
            }
            else
            {
                controller.Move(agent.velocity * deltaTime);
                animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, deltaTime);
            }
        }
    }

    public override void OnExit()
    {
        animator?.SetBool(hashMove, false);
        agent.ResetPath();
    }
}
