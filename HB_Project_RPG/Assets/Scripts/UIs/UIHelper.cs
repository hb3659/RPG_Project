using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    // ���� ��ġ (����Ƽ 3���� ��ǥ��) �� ui ���� ��ǥ (��ũ�� ��ǥ��) �� �����ϴ� �Լ�
    // ���� �� ������ : ui ���� ������ - ���� ��ġ : X 
    public static Vector2 WorldPosToMapPos(Vector3 worldPosition,       // 3���� ������Ʈ�� ��ġ
                                            float worldWidth,           // ���� ���� ������
                                            float worldDepth,           // ���� ���� ������
                                            float uiMapWidth,           // ui ���� ���� ������
                                            float uiMapHeight)          // ui ���� ���� ������
    {
        Vector2 result = Vector2.zero;
        result.x = (worldPosition.x * uiMapWidth) / worldWidth;
        result.y = (worldPosition.z * uiMapHeight) / worldDepth;
        return result;
    }

    // ui ���� 2���� ��ǥ�踦 3D �ʻ��� 3���� ��ǥ��� �����ϴ� �Լ�
    public static Vector3 MapPosToWorldPos(Vector3 uiPos,
                                            float worldWidth,           // ���� ���� ������
                                            float worldDepth,           // ���� ���� ������
                                            float uiMapWidth,           // ui ���� ���� ������
                                            float uiMapHeight)          // ui ���� ���� ������
    {
        Vector3 result = Vector3.zero;
        result.x = (uiPos.x * worldWidth) / uiMapWidth;
        result.z = (uiPos.y * worldDepth) / uiMapHeight;
        return result;
    }
}
