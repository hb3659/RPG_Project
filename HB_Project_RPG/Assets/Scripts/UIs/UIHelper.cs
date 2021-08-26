using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    // 월드 위치 (유니티 3차원 좌표계) 를 ui 상의 좌표 (스크린 좌표계) 로 변경하는 함수
    // 월드 맵 사이즈 : ui 맵의 사이즈 - 월드 위치 : X 
    public static Vector2 WorldPosToMapPos(Vector3 worldPosition,       // 3차원 오브젝트의 위치
                                            float worldWidth,           // 맵의 가로 사이즈
                                            float worldDepth,           // 맵의 깊이 사이즈
                                            float uiMapWidth,           // ui 맵의 가로 사이즈
                                            float uiMapHeight)          // ui 맵의 세로 사이즈
    {
        Vector2 result = Vector2.zero;
        result.x = (worldPosition.x * uiMapWidth) / worldWidth;
        result.y = (worldPosition.z * uiMapHeight) / worldDepth;
        return result;
    }

    // ui 맵의 2차원 좌표계를 3D 맵상의 3차원 좌표계로 변경하는 함수
    public static Vector3 MapPosToWorldPos(Vector3 uiPos,
                                            float worldWidth,           // 맵의 가로 사이즈
                                            float worldDepth,           // 맵의 깊이 사이즈
                                            float uiMapWidth,           // ui 맵의 가로 사이즈
                                            float uiMapHeight)          // ui 맵의 세로 사이즈
    {
        Vector3 result = Vector3.zero;
        result.x = (uiPos.x * worldWidth) / uiMapWidth;
        result.z = (uiPos.y * worldDepth) / uiMapHeight;
        return result;
    }
}
