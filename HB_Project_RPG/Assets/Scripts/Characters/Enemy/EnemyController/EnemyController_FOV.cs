using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_FOV : MonoBehaviour
{
    #region Variables
    // ��� ���ؽ�Ʈ(Behaviour)�� �����ϰ� �ִ��� �߰����ش�.
    protected StateMachine<EnemyController_FOV> stateMachine;
    public StateMachine<EnemyController_FOV> StateMachine => stateMachine;

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

    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // �ʱ� ���� ����
        //stateMachine = new StateMachine<EnemyController_FOV>(this, new IdleState());
        //stateMachine.AddState(new MoveState());
        //stateMachine.AddState(new AttackState());

        fov = GetComponent<FieldOfView>();
    }
    private void Update()
    {
        // �������� ���� �ð��� ����ϱ� ���� �ش� �Լ����� Time.deltaTime �� ���
        stateMachine.Update(Time.deltaTime);
        Debug.Log("Current State : " + stateMachine.CurrentState);
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

            return (distance < attackRange);
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
    #endregion Other Methods
}
