using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(3, 2, 2);

    // 박스 오버랩을 이용하여 내부에 들어온 적을 인식하여 리턴
    // 원하는 레이어 마스크를 검출
    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, layerMask);
    }

    private void OnDrawGizmos()
    {
        // 현재 게임 오브젝트의 위치를 worldPosition 으로 변경해주는 매트릭스를 기즈모에 추가
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        // 원점 (transform.position)
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
