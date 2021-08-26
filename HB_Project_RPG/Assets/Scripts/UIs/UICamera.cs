using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    Camera uiCam;
    ReSpawn Respawn;
    // Start is called before the first frame update
    void Start()
    {
        uiCam = GetComponent<Camera>();
        Respawn = FindObjectOfType<ReSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        //uiCam.transform.SetParent(Respawn.player.transform);
    }
}
