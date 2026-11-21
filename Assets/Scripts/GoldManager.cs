using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{

    public int playerGold = 0;
    private Text goldUI;
    private GameObject mainManager;

    private void Start()
    {
        // find UI
        GameObject GoldUI = GameObject.Find("Main Camera").transform.Find("UI").gameObject.transform.Find("Gold").gameObject;
        goldUI = GoldUI.transform.Find("GoldAmount").gameObject.GetComponent<Text>();

        // get main Manager
        mainManager = GameObject.Find("MainManager");

        // Get gold value from save file
        playerGold = mainManager.GetComponent<Player>().gold;
    }

    private void Update()
    {
        if (goldUI != null)
        {
            goldUI.text = playerGold.ToString();
        }
    }

    public void GainGold(int amount)
    {
        playerGold += amount;
    }

    public bool LoseGold(int amount)
    {
        playerGold -= amount;

        if (playerGold < 0) // if not enough gold return false
        {
            playerGold += amount;
            return false;
        }

        return true; // otherwise return true
    }
}
