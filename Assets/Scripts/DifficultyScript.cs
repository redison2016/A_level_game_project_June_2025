using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DifficultyScript : MonoBehaviour
{

    public float playerSkillLevel = 1;
    private float skillMinCap = 0.9f;
    private float skillMaxCap = 1.1f;
    public int difficulty;
    /* 
    0 : easy
    1 : normal
    2 : hard 
    */
    public GameObject mainManager;
    public GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        // get main manager
        mainManager = GameObject.Find("MainManager");

        // default difficulty
        difficulty = mainManager.GetComponent<Player>().difficulty;

        // change UI
        settings = GameObject.Find("SettingsCanvas");
        TMP_Dropdown dropdown = settings.transform.Find("DifficultySettings").GetComponent<TMP_Dropdown>();
        dropdown.value = difficulty;

        // default skill level
        playerSkillLevel = mainManager.GetComponent<Player>().playerSkillLevel;
    }

    public void ChangeDifficulty(int val)
    {
        difficulty = val;
    }

    public float GetDifficultyScale()
    {
        switch (difficulty)
        {
            case (0):
                return 0.8f * playerSkillLevel;
            case (1):
                return 1f * playerSkillLevel;
            case (2):
                return 1.2f * playerSkillLevel;
        }
        return 1;
    }

    public void ChangeSkillLevel(bool increase)
    {
        if (increase)
        {
            playerSkillLevel += 0.01f;

            // make sure it doesnt exceed cap
            if (playerSkillLevel > skillMaxCap) 
            {
                playerSkillLevel = skillMaxCap;
            }
        }
        else
        {
            playerSkillLevel -= 0.01f;

            // make sure it doesnt go under cap
            if (playerSkillLevel < skillMinCap)
            {
                playerSkillLevel = skillMinCap;
            }
        }
    }
}
