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
        // Ÿ������ �̵�
        //agent?.SetDestination(context.target.position);
        agent?.SetDestination(context.Target.position);
        animator?.SetBool(hashMove, true);
    }
    public override void OnUpdate(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // ���� ��ġ�� �������� ����
            //agent.SetDestination(context.target.position);
            agent.SetDestination(context.Target.position);

            // �̵��� �Ÿ��� ���� ���Ҵٸ�
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
        // Move ���¸� ��� ��
        animator?.SetBool(hashMove, false);
        animator?.SetFloat(hashMoveSpeed, 0);
        agent.ResetPath();
    }
}
