using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class IdleState : State<EnemyController>
//public class IdleState : State<EnemyController_FOV>
//public class IdleState : State<EnemyController_Patrol>
public class IdleState : State<EnemyController_Attackable>
{
    #region Patrol Variables
    public bool isPatrol = false;
    private float minIdleTime = 0.0f;
    private float maxIdleTime = 3.0f;
    private float idleTime = 0.0f;
    #endregion Patrol Variables

    private Animator animator;
    private CharacterController controller;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();

        isPatrol = true;
    }

    public override void OnEnter()
    {
        // ���¿� ���� �� �̵��� ���� �ʱ� ������
        animator?.SetBool(hashMove, false);
        animator?.SetFloat(hashMoveSpeed, 0);
        controller?.Move(Vector3.zero);

        #region Patrol OnEnter
        if (isPatrol)
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        #endregion Patrol OnEnter
    }

    public override void OnUpdate(float deltaTime)
    {
        // ����ؼ� �ֺ��� ���� �ִ����� �˻��ؾ��Ѵ�.
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // ���� �߰��ϰ� �� ���� ������ �� �ִ� ���̶�� ���� ���·� ��ȯ
            if (context.IsAvailableAttack)
                stateMachine.ChangeState<AttackState>();
            // �ƴ϶�� �̵� ���·� ��ȯ�Ѵ�.
            else
                stateMachine.ChangeState<MoveState>();
        }

        #region Patrol Update
        else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
        {
            stateMachine.ChangeState<MoveToWayPointState>();
        }
        #endregion Patrol Update
    }

    public override void OnExit()
    {
        
    }
}
