using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(3, 2, 2);

    // �ڽ� �������� �̿��Ͽ� ���ο� ���� ���� �ν��Ͽ� ����
    // ���ϴ� ���̾� ����ũ�� ����
    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, layerMask);
    }

    private void OnDrawGizmos()
    {
        // ���� ���� ������Ʈ�� ��ġ�� worldPosition ���� �������ִ� ��Ʈ������ ����� �߰�
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        // ���� (transform.position)
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
