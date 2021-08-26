using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FieldOfView))]
public class EnemyController_Attackable : MonoBehaviour, IAttackable, IDamageable
{
    #region Variables
    // ��� ���ؽ�Ʈ(Behaviour)�� �����ϰ� �ִ��� �߰����ش�.
    protected StateMachine<EnemyController_Attackable> stateMachine;
    public StateMachine<EnemyController_Attackable> StateMachine => stateMachine;

    [HideInInspector]
    public EnemyStat enemyStat;
    [HideInInspector]
    public PlayerStat playerStat;

    [HideInInspector]
    public FieldOfView fov;

    // ������ �����Ÿ�
    public float attackRange;
    //public Transform Target => fov?.NearestTarget;
    public Transform Target
    {
        get { return fov?.NearestTarget; }
        set { }
    }

    public Transform[] wayPoints;
    [HideInInspector]
    public Transform targetWayPoint = null;
    private int wayPointIndex = 0;

    // Attackable
    public Transform projectileTransform;

    // ��� ������ �������� �����ϱ� ���� ����Ʈ
    [SerializeField]
    private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
    [SerializeField]
    private HitEffectSpawner hitSpawner;

    private LayerMask TargetMask => fov.targetMask;

    // ������ ������ ���� ��ġ�� ������ ����
    public Transform hitTransform;

    private Animator animator;

    private int hitTriggerHash = Animator.StringToHash("HitTrigger");

    [SerializeField]
    private CameraFacing_FloatingDamage floating;
    [HideInInspector]
    public Text damageText;

    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // �ʱ� ���� ����
        stateMachine = new StateMachine<EnemyController_Attackable>(this, new MoveToWayPointState());

        stateMachine.AddState(new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
        stateMachine.AddState(new DeadState());

        fov = GetComponent<FieldOfView>();

        // Attackable
        InitAttackBehaviour();

        animator = GetComponent<Animator>();

        playerStat = FindObjectOfType<PlayerStat>();
        enemyStat = GetComponent<EnemyStat>();

        floating = GetComponentInChildren<CameraFacing_FloatingDamage>();
    }

    #region AttackBehaviour Helper Methods
    // AttackBehaviour �� �ʱ�ȭ ���ִ� �Լ�
    private void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour == null)
                CurrentAttackBehaviour = behaviour;

            behaviour.targetMask = TargetMask;
        }
    }
    // ���� ��� ���� �ൿ�� �������� �˻��ϴ� �Լ�
    private void CheckAttackBehaviour()
    {
        // CurrentAttackBehaviour �� ���ų� ����� �Ұ����ϸ�
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
        {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    // �켱 ������ ���� ������ ������ �����Ѵ�.
                    if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                    {
                        // ���� �ൿ�� ��ü
                        CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }
    }
    #endregion AttackBehaviour helper Methods

    private void Update()
    {
        // Attackable
        // StateMachine �� �����ϱ� ���� ���� ��� ���� �ൿ�� ����� �� �ִ��� ����
        CheckAttackBehaviour();

        // �������� ���� �ð��� ����ϱ� ���� �ش� �Լ����� Time.deltaTime �� ���
        stateMachine.Update(Time.deltaTime);

        //Debug.Log(stateMachine.CurrentState);
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
    }

    public Transform FindNextWayPoint()
    {
        targetWayPoint = null;

        if (wayPoints.Length > 0)
            targetWayPoint = wayPoints[wayPointIndex];

        wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;

        return targetWayPoint;
    }
    #endregion Other Methods

    #region IAttackable Interface
    // AttackBehaviour �� ������ EnemyController �� ����
    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    // ���� �ִϸ��̼��� ����Ǵ� �� �� ������ ������ ���� �̺�Ʈ ����
    public void OnExecuteAttack(int attackIndex)
    {
        // Ÿ���� �������� ������ ���� �ൿ ��ü�� ����� �� ����.
        if (CurrentAttackBehaviour != null && Target != null && enemyStat.HP > 0)
        {
            // FieldOfView �� Ÿ��
            CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }
    #endregion IAttackable Interface

    #region IDamageable Interface
    // ĳ���Ͱ� ����ִ��� Ȯ���ϱ� ���� ����
    public bool IsAlive => (enemyStat.HP > 0);

    public void TakeDamage(float damage, GameObject hitEffectPrefabs)
    {
        if (!IsAlive)
            return;

        float originDamage = (playerStat.OffensivePower - enemyStat.DefensivePower * 0.5f) * 0.5f;
        int rnd = Random.Range(0, (int)originDamage);

        damage = Mathf.Floor(originDamage + rnd);

        floating.updateDamage(damageText, damage);
        hitSpawner.GetHit();

        if (damage <= 0)
            damage = 1;

        enemyStat.HP -= damage;

        if (IsAlive)
            animator?.SetTrigger(hitTriggerHash);
        else
            stateMachine.ChangeState<DeadState>();
    }
    #endregion IDamageable Interface
}
