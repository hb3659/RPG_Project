using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    #region Variables
    [Header("Sight Settings")]
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public int targetMask = 1 << 9;
    public int obstacleMask = 1 << 11;

    private List<Transform> visibleTargets = new List<Transform>();

    [SerializeField]
    private Transform nearestTarget = null;
    private float distanceToTarget = 0.0f;

    [Header("Find Settings")]
    public float delay = 0.2f;
    #endregion Variables

    #region Properties
    public List<Transform> VisibleTargets => visibleTargets;
    public Transform NearestTarget => nearestTarget;
    public float DistanceToTarget => distanceToTarget;
    #endregion Properties


    // Start is called before the first frame update
    public void OnEnable()
    {
        // delay 시간 마다 적을 검색
        StartCoroutine("FindTargetsWithDelay", delay);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        distanceToTarget = 0.0f;
        nearestTarget = null;
        visibleTargets.Clear();

        Collider[] targetsInViewRadius =
            Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            Transform target = targetsInViewRadius[i].transform;

            // 자신과 타겟의 방향 계산
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // 자신의 위치에서 타겟으로의 방향으로 거리만큼의 레이캐스트를 쏘는 중 장애물이 있는지 확인
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    // 이전에 계산된 거리가 현재 계산된 거리보다 더 크다면
                    if (nearestTarget == null || (distanceToTarget > dstToTarget))
                    {
                        nearestTarget = target;
                        distanceToTarget = dstToTarget;
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            // 로컬이라면 현재 오브젝트의 y 값
            // 글로벌이라면 글로벌의 y 값
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDisable()
    {
        nearestTarget = null;
        visibleTargets.Clear();
    }
}
