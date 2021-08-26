using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBillBoard : MonoBehaviour
{
    private Transform cam;
    public EnemyController_Attackable controller;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponentInParent<EnemyController_Attackable>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);

        image.fillAmount = controller.enemyStat.HP / controller.enemyStat.MaxHP;
    }
}
