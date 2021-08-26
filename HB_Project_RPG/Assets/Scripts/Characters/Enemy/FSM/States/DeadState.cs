using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController_Attackable>
{
    PlayerController playerController;
    PointerSpawner pointSpawner;

    private Animator animator;
    protected int isAliveHash = Animator.StringToHash("IsAlive");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        playerController = context.GetComponent<PlayerController>();
    }
    public override void OnEnter()
    {
        animator?.SetBool(isAliveHash, false);
    }
    public override void OnUpdate(float deltaTime)
    {
        // 죽은 상태라면 2초 후 오브젝트 삭제
        if (stateMachine.ElapsedTimeInState > 2.0f)
        {
            ObjectPooler.Instance.ReturnObject(context.transform.parent.gameObject);
            stateMachine.ChangeState<IdleState>();

            context.enemyStat.HP = context.enemyStat.MaxHP;

            context.playerStat.Exp += context.enemyStat.Exp;
            context.playerStat.Gold += context.enemyStat.Gold;
        }
    }
    public override void OnExit()
    {

    }
}
