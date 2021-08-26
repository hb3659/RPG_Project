using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : State<PlayerController>
{
    private Animator animator;
    private PlayerController controller;

    protected int isAliveHash = Animator.StringToHash("IsAlive");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }
    public override void OnEnter()
    {
        animator?.SetBool(isAliveHash, false);
    }
    public override void OnUpdate(float deltaTime)
    {
        if (stateMachine.ElapsedTimeInState > 1.5f)
            GameObject.Destroy(context.gameObject);
    }
    public override void OnExit()
    {

    }
}
