using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFacing_HealthBar : CameraFacing
{
    public EnemyStat enemyStat;
    public Image healthBarImage;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        enemyStat = GetComponentInParent<EnemyStat>();
    }
    
    // Update is called once per frame
    protected override void LateUpdate()
    {
        base.LateUpdate();
        updateHealth();
    }

    public void updateHealth()
    {
        healthBarImage.fillAmount = enemyStat.HP / enemyStat.MaxHP;
    }
}
