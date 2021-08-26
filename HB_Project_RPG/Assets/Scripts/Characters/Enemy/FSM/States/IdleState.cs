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
        // 상태에 진입 시 이동을 하지 않기 때문에
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
        // 계속해서 주변에 적이 있는지를 검색해야한다.
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            // 적을 발견하고 그 적이 공격할 수 있는 적이라면 공격 상태로 전환
            if (context.IsAvailableAttack)
                stateMachine.ChangeState<AttackState>();
            // 아니라면 이동 상태로 전환한다.
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
