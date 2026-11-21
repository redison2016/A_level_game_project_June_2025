using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : HPManager
{
    public float healCD;
    float currentCD;
    public int healAmount;
    private Image playerHealthBar;
    private GameObject mainManager;
    XPManager xpManager;
    int defaultHP;

    private void Start()
    {
        xpManager = GetComponent<XPManager>();
        defaultHP = maxHP;

        // get main Manager
        mainManager = GameObject.Find("MainManager");

        // find difficulty manager
        difficultyManager = GameObject.Find("DifficultyManager");

        currentHP = mainManager.GetComponent<Player>().health;

        // find UI
        GameObject HealthUI = GameObject.Find("Main Camera").transform.Find("UI").gameObject.transform.Find("Health").gameObject;
        playerHealthBar = HealthUI.transform.Find("HealthBar").gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        maxHP = (int)(defaultHP * (1f + (xpManager.playerLevel - 1) * 0.05f));

        // heal CD calculation
        if (currentCD >= 0) 
        {
            currentCD -= Time.deltaTime;
        }

        // update health bar 
        if (playerHealthBar != null)
        {
            playerHealthBar.fillAmount = (float)currentHP / (float)maxHP;
        }
    }

    public void Heal()
    {
        // check CD
        if (currentCD > 0)
        {
            return;
        }

        // heal
        currentHP += healAmount;

        // check if current HP exceeds max
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        // play heal animation
        GameObject EffectManager = transform.Find("HealEffectManager").gameObject;
        EffectManager.GetComponent<Animator>().SetTrigger("heal");

        // set CD
        currentCD = healCD;
    }
 
}
