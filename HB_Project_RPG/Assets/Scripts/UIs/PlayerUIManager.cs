using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    #region Variabels
    public GameObject playerCanvas;
    [SerializeField]
    private Image statPanel;
    [SerializeField]
    private Image quickPanel;
    [SerializeField]
    private Image inventoryPanel;
    [SerializeField]
    private Image MenuPanel;

    private PlayerStat playerStat;
    private EnemyStat enemyStat;

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image manaBar;
    [SerializeField]
    private Image expertBar;

    private TabGroup tabGroup;
    private TabButton[] buttons;

    private float maxHP;
    private float currentHP;
    private float maxMana;
    private float currentMana;
    private float maxExpert;

    public Text healthText;
    public Text manaText;
    public Text offenseText;
    public Text defenseText;
    public Text currentGold;

    #endregion Variables

    private void Start()
    {
        tabGroup = inventoryPanel.GetComponentInChildren<TabGroup>();
        buttons = tabGroup.GetComponentsInChildren<TabButton>();

        playerStat = FindObjectOfType<PlayerStat>();
        enemyStat = FindObjectOfType<EnemyStat>();
    }

    private void Update()
    {
        UpdateBar();
        ShortCut();
        UpdateEquipStat();
    }

    private void UpdateBar()
    {
        maxHP = playerStat.MaxHP;
        maxMana = playerStat.MaxMana;
        maxExpert = (playerStat.Level - 1) * 40 + 100;

        currentHP = playerStat.HP;
        currentMana = playerStat.Mana;

        healthBar.fillAmount = currentHP / maxHP;
        manaBar.fillAmount = currentMana / maxMana;

        if (playerStat.Exp >= maxExpert)
        {
            playerStat.Exp -= maxExpert;
            playerStat.Level += 1;
        }
        expertBar.fillAmount = playerStat.Exp / maxExpert;
    }

    private void ShortCut()
    {
        if (!inventoryIsOpen)
        {
            if (!menuIsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    inventoryPanel.gameObject.SetActive(true);
                    tabGroup.OnTabSelected(buttons[0]);
                }
                else if (Input.GetKeyDown(KeyCode.I))
                {
                    inventoryPanel.gameObject.SetActive(true);
                    tabGroup.OnTabSelected(buttons[1]);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    inventoryPanel.gameObject.SetActive(true);
                    tabGroup.OnTabSelected(buttons[2]);
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    inventoryPanel.gameObject.SetActive(true);
                    tabGroup.OnTabSelected(buttons[3]);
                }
            }
        }
        else
        {
            if (!menuIsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (tabGroup.selectedTab == buttons[0])
                    {
                        inventoryPanel.gameObject.SetActive(false);
                        tabGroup.selectedTab = null;
                    }
                    else
                        tabGroup.OnTabSelected(buttons[0]);
                }
                else if (Input.GetKeyDown(KeyCode.I))
                {
                    if (tabGroup.selectedTab == buttons[1])
                    {
                        inventoryPanel.gameObject.SetActive(false);
                        tabGroup.selectedTab = null;
                    }
                    else
                        tabGroup.OnTabSelected(buttons[1]);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    if (tabGroup.selectedTab == buttons[2])
                    {
                        inventoryPanel.gameObject.SetActive(false);
                        tabGroup.selectedTab = null;
                    }
                    else
                        tabGroup.OnTabSelected(buttons[2]);
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    if (tabGroup.selectedTab == buttons[3])
                    {
                        inventoryPanel.gameObject.SetActive(false);
                        tabGroup.selectedTab = null;
                    }
                    else
                        tabGroup.OnTabSelected(buttons[3]);
                }
            }
        }

        if (!menuIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuPanel.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuPanel.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    private bool inventoryIsOpen
    {
        get
        {
            return inventoryPanel.gameObject.activeSelf;
        }
    }

    private bool menuIsOpen
    {
        get
        {
            return MenuPanel.gameObject.activeSelf;
        }
    }

    private void UpdateEquipStat()
    {
        healthText.text = playerStat.MaxHP.ToString();
        manaText.text = playerStat.MaxMana.ToString();
        offenseText.text = playerStat.OffensivePower.ToString();
        defenseText.text = playerStat.DefensivePower.ToString();
        currentGold.text = playerStat.Gold.ToString();
    }
}
