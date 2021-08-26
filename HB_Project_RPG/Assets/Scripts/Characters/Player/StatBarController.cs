using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{ 
    private PlayerStat playerStat;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image manaBar;

    // ������ PlayerStat ���� ����
    // �ش� ������Ʈ������ ������ �� ����.
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float currentHP;
    [SerializeField]
    private float MaxMana;
    [SerializeField]
    private float currentMana;

    // Start is called before the first frame update
    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        healthBar = transform.Find("HealthBar").gameObject.GetComponent<Image>();
        manaBar = transform.Find("ManaBar").gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        maxHP = playerStat.MaxHP;
        currentHP = playerStat.HP;
        MaxMana = playerStat.MaxMana;
        currentMana = playerStat.Mana;

        healthBar.fillAmount = currentHP / maxHP;
        manaBar.fillAmount = currentMana / MaxMana;
    }
}
