using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int EXP = 0;
    public int health = 100;
    public int gold = 0;
    public float[] position = { -2f, -13f, 0 };
    public bool[] checkPointVisited = { false, false, false };
    public int difficulty = 1;
    public float playerSkillLevel = 1;
    public int currentMission = 1;
    public float volume = 1;
    public string[] keybinds = { "w", "s", "a", "d", "e", "j", "k", "l", "i", "q" };

    public PlayerData (GameObject player)
    {
        if (player == null)
        {
            return;
        }
        EXP = player.GetComponent<XPManager>().playerXP;
        health = player.GetComponent<HPManager>().currentHP;
        gold = player.GetComponent<GoldManager>().playerGold;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        checkPointVisited = player.GetComponent<CheckPointVisitedScript>().checkPointVisited;

        GameObject difficultyManager = GameObject.Find("DifficultyManager");
        difficulty = difficultyManager.GetComponent<DifficultyScript>().difficulty;
        playerSkillLevel = difficultyManager.GetComponent<DifficultyScript>().playerSkillLevel;

        currentMission = player.GetComponent<StoryManager>().currentMission;

        GameObject mapChangeManager = GameObject.Find("MapChangeManager");
        volume = mapChangeManager.GetComponent<AudioSource>().volume;

        GameObject inputManager = GameObject.Find("InputManager");
        keybinds = inputManager.GetComponent<UserInput>().keysAssigned;
    }
}

