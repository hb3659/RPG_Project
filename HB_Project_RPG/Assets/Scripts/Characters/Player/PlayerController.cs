using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IAttackable, IDamageable
{
    #region Vairables
    protected StateMachine<PlayerController> stateMachine;
    public StateMachine<PlayerController> StateMachine => stateMachine;

    public float attackRange = 2f;

    private Camera cam;

    [HideInInspector]
    public LayerMask ground;
    [HideInInspector]
    public LayerMask interaction;

    //[HideInInspector]
    public Transform hitTransform;
    //[HideInInspector]
    public Vector3 hitPoint;

    // 이펙트의 발생 위치
    public Transform projectileTransform;
    public Transform hitEffectTransform;

    private Animator animator;
    private PlayerStat playerStat;
    private EnemyStat enemyStat;
    private EnemyController_Attackable enemyController;
    private HitEffectSpawner hitSpawner;

    [HideInInspector]
    public PointerSpawner pointSpawner;
    //[HideInInspector]
    public GameObject pointer;

    private CameraFacing_FloatingDamage floating;
    private Text damageText;

    [SerializeField]
    private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    private int hitTriggerHash = Animator.StringToHash("HitTrigger");

    #endregion Variables

    private void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        enemyStat = null;

        stateMachine = new StateMachine<PlayerController>(this, new PlayerIdleState());

        StateMachine.AddState(new PlayerMoveState());
        StateMachine.AddState(new PlayerAttackState());
        StateMachine.AddState(new PlayerDeadState());

        cam = Camera.main;
        animator = GetComponent<Animator>();

        hitSpawner = GetComponentInChildren<HitEffectSpawner>();

        floating = GetComponentInChildren<CameraFacing_FloatingDamage>();

        pointSpawner = GetComponentInChildren<PointerSpawner>();

        InitAttackBehaviour();
    }

    public bool IsAttackable
    {
        get
        {
            if (!hitTransform)
                return false;

            float distance = Vector3.Distance(transform.position, hitTransform.position);
            return (distance < attackRange);
        }
    }

    public bool IsPointerOver
    {
        get
        {
            // UI 클릭 막기위함
            return !EventSystem.current.IsPointerOverGameObject();
        }
    }

    public Vector3 ClickToMove()
    {
        if (Input.GetMouseButtonDown(1) && IsPointerOver)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                hitPoint = hit.point;

                if (!pointer)
                    pointer = pointSpawner.GetPointer(hitPoint);
                else
                {
                    pointSpawner.ReturnPointer(pointer);
                    pointer = pointSpawner.GetPointer(hitPoint);
                }

                return hitPoint;
            }
        }

        return transform.position;
    }

    public Transform clickToInteract()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOver)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, interaction))
            {
                hitTransform = hit.transform;
                enemyStat = hitTransform.gameObject.GetComponent<EnemyStat>();
            }
        }

        if (hitTransform)
        {
            if (!pointer)
                pointer = pointSpawner.GetPointer(hitTransform.position);
            else
            {
                pointSpawner.ReturnPointer(pointer);
                pointer = pointSpawner.GetPointer(hitTransform.position);
            }
        }

        return hitTransform;
    }

    private void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour == null)
                CurrentAttackBehaviour = behaviour;

            if (hitTransform && hitTransform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                behaviour.targetMask = hitTransform.gameObject.layer;
        }
    }

    private void CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
            CurrentAttackBehaviour = null;

        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (behaviour.IsAvailable)
            {
                // 우선순위가 높은 공격 행동을 실행
                if (CurrentAttackBehaviour == null ||
                    CurrentAttackBehaviour.priority < behaviour.priority)
                    CurrentAttackBehaviour = behaviour;
            }
        }
    }

    private void Update()
    {
        CheckAttackBehaviour();

        StateMachine.Update(Time.deltaTime);
    }

    #region IAttackable interface
    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null &&
            (hitTransform != null && hitTransform.gameObject.layer == LayerMask.NameToLayer("Enemy")))
        {
            CurrentAttackBehaviour.ExecuteAttack(hitTransform.gameObject, projectileTransform);

            if (!hitTransform.parent.gameObject.activeSelf || enemyStat.HP < 0)
            {
                hitTransform = null;
            }
        }
    }
    #endregion IAttackable interface

    #region IDamageable interface
    public bool IsAlive => playerStat.HP > 0;

    public void TakeDamage(float damage, GameObject hitEffectPrefabs)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, interaction);

        foreach (Collider collider in colliders)
        {
            enemyController = collider.gameObject.GetComponent<EnemyController_Attackable>();
        }

        if (!IsAlive)
            return;

        if (enemyController.Target)
            enemyStat = enemyController.enemyStat;
        else
            enemyStat = null;

        float originDamage = (enemyStat.OffensivePower - playerStat.DefensivePower * 0.5f) * 0.5f;
        int rnd = Random.Range(0, (int)originDamage);

        damage = Mathf.Floor(originDamage + rnd);

        floating.updateDamage(damageText, damage);
        hitSpawner.GetHit();

        if (damage <= 0)
            damage = 1;

        playerStat.HP -= damage;

        if (IsAlive)
            animator?.SetTrigger(hitTriggerHash);
        else
            StateMachine.ChangeState<PlayerDeadState>();
    }
    #endregion IDamageable interface
}
