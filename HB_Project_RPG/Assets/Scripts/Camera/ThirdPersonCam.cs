using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    private CinemachineFreeLook cam;
    public ReSpawn respawn;

    // Start is called before the first frame update
    private void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
        respawn = FindObjectOfType<ReSpawn>();
    }

    private void Update()
    {
        if (respawn != null)
        {
            cam.Follow = respawn.player.transform;
            cam.LookAt = respawn.player.transform;
        }
    }
}
