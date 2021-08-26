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
        // ����ؼ� �ֺ��� ���� �ִ����� �˻��ؾ��Ѵ�.
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // ���� �����ϰ� �� ���� ������ �� �ִ� ���̶�� ���� ���·� ��ȯ
            if (context.IsAvailableAttack)
                stateMachine.ChangeState<AttackState>();
            // �ƴ϶�� �̵� ���·� ��ȯ�Ѵ�.
            else
                stateMachine.ChangeState<MoveState>();
        }
        else
        {
            // agent �� �̵��ؾ��� ��ΰ� �����ϴ��� Ȯ���ϱ� ����
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
