using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AttackBehaviour ���� ����� �޾� ���� AttackBehaviour ���� ����
// �ش� ������Ʈ ��ü������ ���� �ൿ�� ���� �⺻���� ������ �������̽��� ������ �ְ�
// �������� �������� ������ �������� ���� �ൿ���� ���� (�ٰŸ�/���Ÿ�)
public abstract class AttackBehaviour : MonoBehaviour
{
    #region Variables
    // �����ڰ� �����Ϳ��� ��� ������Ʈ���� Ȯ���ϱ� ���� �뵵 (�ּ� ó��)
#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "AttackBehaviour";
#endif // UNITY_EDITOR

    // AttackBehaviour �� ����� �� ����Ǿ����� �� �ִϸ��̼� �ε���
    public int animationIndex;
    // ������ �켱����
    public int priority;
    public int damage = 10;
    // ���ݿ� ���� ���� �Ÿ�
    public float range = 2.5f;

    [SerializeField]
    protected float coolTime;
    // ��Ÿ�� ����
    protected float calcCoolTime = 0.0f;

    [HideInInspector]
    public GameObject effectPrefab;

    // ���� ���ݿ� ���� ���� ���̾� ����ũ
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

    // ��� ���� ������ ȣ������ �����ϴ� �Լ�
    // ���� ���� �� Ÿ���� �ʿ����� ���� �� �ִ�.
    // startPoint �� ���Ÿ� ���� �� �߻�ü�� �߻�Ǵ� ������ �ǹ�
    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}
