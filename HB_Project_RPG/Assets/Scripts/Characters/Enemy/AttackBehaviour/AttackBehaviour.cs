using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AttackBehaviour 에서 상속을 받아 하위 AttackBehaviour 들을 구현
// 해당 컴포넌트 자체에서는 공격 행동에 대한 기본적인 정보와 인터페이스만 가지고 있고
// 하위에서 정보들을 가지고 실질적인 공격 행동들을 구현 (근거리/원거리)
public abstract class AttackBehaviour : MonoBehaviour
{
    #region Variables
    // 개발자가 에디터에서 어떠한 컴포넌트인지 확인하기 위한 용도 (주석 처리)
#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "AttackBehaviour";
#endif // UNITY_EDITOR

    // AttackBehaviour 가 실행될 때 실행되어져야 할 애니메이션 인덱스
    public int animationIndex;
    // 공격의 우선순위
    public int priority;
    public int damage = 10;
    // 공격에 대한 사정 거리
    public float range = 2.5f;

    [SerializeField]
    protected float coolTime;
    // 쿨타임 계산용
    protected float calcCoolTime = 0.0f;

    [HideInInspector]
    public GameObject effectPrefab;

    // 현재 공격에 대한 적의 레이어 마스크
    public LayerMask targetMask;

    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;
    #endregion Variables

    private void Start()
    {
        calcCoolTime = coolTime;
    }

    private void Update()
    {
        if(calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;
        }
    }

    // 어떠한 공격 로직을 호출할지 결정하는 함수
    // 범위 공격 시 타겟이 필요하지 않을 수 있다.
    // startPoint 는 원거리 공격 시 발사체가 발사되는 지점을 의미
    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}
