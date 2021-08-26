using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FieldOfView))]
public class EnemyController_Attackable : MonoBehaviour, IAttackable, IDamageable
{
    #region Variables
    // 어떠한 컨텍스트(Behaviour)가 소유하고 있는지 추가해준다.
    protected StateMachine<EnemyController_Attackable> stateMachine;
    public StateMachine<EnemyController_Attackable> StateMachine => stateMachine;

    [HideInInspector]
    public EnemyStat enemyStat;
    [HideInInspector]
    public PlayerStat playerStat;

    [HideInInspector]
    public FieldOfView fov;

    // 공격의 사정거리
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

    // 어떠한 공격을 선택할지 연산하기 위한 리스트
    [SerializeField]
    private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
    [SerializeField]
    private HitEffectSpawner hitSpawner;

    private LayerMask TargetMask => fov.targetMask;

    // 공격을 당했을 시의 위치를 가지는 변수
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
        // 초기 상태 선언
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
    // AttackBehaviour 를 초기화 해주는 함수
    private void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour == null)
                CurrentAttackBehaviour = behaviour;

            behaviour.targetMask = TargetMask;
        }
    }
    // 현재 어떠한 공격 행동을 결정할지 검사하는 함수
    private void CheckAttackBehaviour()
    {
        // CurrentAttackBehaviour 가 없거나 사용이 불가능하면
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
        {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    // 우선 순위가 높은 공격을 선택해 실행한다.
                    if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                    {
                        // 공격 행동을 교체
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
        // StateMachine 에 진입하기 전에 현재 어떠한 공격 행동을 사용할 수 있는지 연산
        CheckAttackBehaviour();

        // 소유자의 갱신 시간을 사용하기 위해 해당 함수에서 Time.deltaTime 을 사용
        stateMachine.Update(Time.deltaTime);

        //Debug.Log(stateMachine.CurrentState);
    }
    #endregion Unity Mathods

    #region Other Methods
    // 공격이 가능한지에 대한 프로퍼티
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

    // viewRadius 안에 타겟이 있는지 검색하기 위한 함수
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
    // AttackBehaviour 를 현재의 EnemyController 만 선택
    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    // 공격 애니메이션이 진행되는 중 한 프레임 구간에 대한 이벤트 구현
    public void OnExecuteAttack(int attackIndex)
    {
        // 타겟이 존재하지 않으면 공격 행동 자체가 실행될 수 없다.
        if (CurrentAttackBehaviour != null && Target != null && enemyStat.HP > 0)
        {
            // FieldOfView 의 타겟
            CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }
    #endregion IAttackable Interface

    #region IDamageable Interface
    // 캐릭터가 살아있는지 확인하기 위한 변수
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
