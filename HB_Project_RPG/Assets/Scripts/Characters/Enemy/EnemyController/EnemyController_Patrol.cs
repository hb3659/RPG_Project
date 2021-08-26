using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class EnemyController_Patrol : MonoBehaviour
{
    #region Variables
    // ��� ���ؽ�Ʈ(Behaviour)�� �����ϰ� �ִ��� �߰����ش�.
    protected StateMachine<EnemyController_Patrol> stateMachine;
    public StateMachine<EnemyController_Patrol> StateMachine => stateMachine;

    private FieldOfView fov;

    // SearchEnemy �� ���� ����
    // �˻��� ����� ���̾� ����ũ
    //public LayerMask targetMask;
    //// ��� ���� �Ÿ��� �ִ� ���� �˻����� 
    //public float viewRadius;
    //// �߰��� ���� Transform
    //public Transform target;

    // ������ �����Ÿ�
    public float attackRange;
    public Transform Target => fov?.NearestTarget;

    public Transform[] wayPoints;
    [HideInInspector]
    public Transform targetWayPoint = null;
    private int wayPointIndex = 0;

    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // �ʱ� ���� ����
        //stateMachine = new StateMachine<EnemyController_Patrol>(this, new MoveToWayPointState());

        //stateMachine.AddState(new IdleState());
        //stateMachine.AddState(new MoveState());
        //stateMachine.AddState(new AttackState());

        fov = GetComponent<FieldOfView>();
    }
    private void Update()
    {
        // �������� ���� �ð��� ����ϱ� ���� �ش� �Լ����� Time.deltaTime �� ���
        stateMachine.Update(Time.deltaTime);
        Debug.Log("Current State is " + stateMachine.CurrentState);
        //Debug.Log("IsAvailableAttack : " + IsAvailableAttack);
    }
    #endregion Unity Mathods

    #region Other Methods
    // ������ ���������� ���� ������Ƽ
    public bool IsAvailableAttack
    {
        get
        {
            if (!Target)
                return false;

            float distance = Vector3.Distance(transform.position, Target.position);

            return (distance <= attackRange);
        }
    }

    // viewRadius �ȿ� Ÿ���� �ִ��� �˻��ϱ� ���� �Լ�
    public Transform SearchEnemy()
    {
        
        return Target;

        //target = null;

        //// ���� ��ġ���� viewRadius ��ŭ ������ �Ÿ��� �ִ� Ÿ���� ã�´�.
        //Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //// �浹 �˻縦 ���� �߰ߵǾ��ٸ�
        //if (targetInViewRadius.Length > 0)
        //{
        //    // ���� ù��° �ݶ��̴��� transform �� �ִ´�.
        //    target = targetInViewRadius[0].transform;
        //}

        //return target;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, viewRadius);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

    public Transform FindNextWayPoint()
    {
        targetWayPoint = null;

        if (wayPoints.Length > 0)
            targetWayPoint = wayPoints[wayPointIndex];

        wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;

        return targetWayPoint;
    }
    #endregion Other Methods
}
