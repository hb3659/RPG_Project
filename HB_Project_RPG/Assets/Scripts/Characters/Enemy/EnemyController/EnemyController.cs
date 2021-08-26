using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    // 어떠한 컨텍스트(Behaviour)가 소유하고 있는지 추가해준다.
    protected StateMachine<EnemyController> stateMachine;
    public StateMachine<EnemyController> StateMachine => StateMachine;

    // SearchEnemy 를 위한 변수
    // 검색할 대상의 레이어 마스크
    public LayerMask targetMask;
    // 어느 정도 거리에 있는 적을 검색할지 
    public float viewRadius;
    // 발견한 적의 Transform
    public Transform target;
    // 공격의 사정거리
    public float attackRange;

    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // 초기 상태 선언
        //stateMachine = new StateMachine<EnemyController>(this, new IdleState());
        //stateMachine.AddState(new MoveState());
        //stateMachine.AddState(new AttackState());
    }
    private void Update()
    {
        // 소유자의 갱신 시간을 사용하기 위해 해당 함수에서 Time.deltaTime 을 사용
        stateMachine.Update(Time.deltaTime);
        Debug.Log("Current State : " + stateMachine.CurrentState);
    }
    #endregion Unity Mathods

    #region Other Methods
    // 공격이 가능한지에 대한 프로퍼티
    public bool IsAvailableAttack
    {
        get
        {
            if (!target)
                return false;

            float distance = Vector3.Distance(transform.position, target.position);

            return (distance <= attackRange);
        }
    }
    // viewRadius 안에 타겟이 있는지 검색하기 위한 함수
    public Transform SearchEnemy()
    {
        target = null;

        // 현재 위치에서 viewRadius 만큼 떨어진 거리에 있는 타겟을 찾는다.
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // 충돌 검사를 통해 발견되었다면
        if (targetInViewRadius.Length > 0)
        {
            // 가장 첫번째 콜라이더의 transform 을 넣는다.
            target = targetInViewRadius[0].transform;
        }

        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion Other Methods
}
