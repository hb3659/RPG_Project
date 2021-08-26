using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private Transform target;

    private void Start()
    {
        camPosition = gameObject.GetComponent<Camera>().transform;
        target = GameObject.Find("Hero_PlayerObject").transform.GetChild(0);
    }

    private void Update()
    {
        float posX = target.position.x;
        float posZ = target.position.z;

        camPosition.position = new Vector3(posX, camPosition.position.y, posZ);
    }
}
