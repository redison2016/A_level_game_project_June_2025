using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class XPManager : MonoBehaviour
{

    public int playerLevel = 1;
    public int playerXP = 0;
    public int XPRequiredToLevelUp = 10;
    public int XPSumOfAllPreviousLevels = 0;
    private Image XPBarImg;
    private Text levelTxt;
    private int PreXPValue = 0;
    private GameObject mainManager;

    private void Start()
    {
        // find UI
        GameObject XPUI = GameObject.Find("Main Camera").transform.Find("UI").gameObject.transform.Find("XP").gameObject;
        XPBarImg = XPUI.transform.Find("XPBar").gameObject.GetComponent<Image>();
        levelTxt = XPUI.transform.Find("Level").gameObject.GetComponent<Text>();

        // get main Manager
        mainManager = GameObject.Find("MainManager");

        // Get XP value from save file
        playerXP = mainManager.GetComponent<Player>().EXP;

        // update level
        playerLevel = (int)(Math.Log(1 + ((float)playerXP / 100f), 10) / Math.Log(1.1, 10)) + 1;
        XPRequiredToLevelUp = (int)(10 * Math.Pow(1.1, playerLevel - 1));

        // update UI
        XPSumOfAllPreviousLevels = (int)(10 * (Math.Pow(1.1, playerLevel - 1) - 1) / 0.1);
        XPBarImg.fillAmount = (float)(playerXP - XPSumOfAllPreviousLevels) / (float)XPRequiredToLevelUp;

        levelTxt.text = playerLevel.ToString();

        // update previous value
        PreXPValue = playerXP;
    }

    // Update is called once per frame
    void Update()
    {
        if (NeedUpdate()) // if XP value changed, then update
        {
            // update level
            playerLevel = (int)(Math.Log(1 + ((float)playerXP / 100f), 10) / Math.Log(1.1, 10)) + 1;
            XPRequiredToLevelUp = (int)(10 * Math.Pow(1.1, playerLevel - 1));

            // update UI
            XPSumOfAllPreviousLevels = (int)(10 * (Math.Pow(1.1, playerLevel - 1) - 1) / 0.1);
            XPBarImg.fillAmount = (float)(playerXP - XPSumOfAllPreviousLevels) / (float)XPRequiredToLevelUp;
        
            levelTxt.text = playerLevel.ToString();

            // update previous value
            PreXPValue = playerXP;
        }
    }

    public void GainXP(int amount)
    {
        playerXP += amount;
    }

    private bool NeedUpdate()
    {
        if (playerXP == PreXPValue)
        {
            return false;
        }
        return true;
    }
}
