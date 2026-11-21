using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // stores player data read from file
{
    public int EXP;
    public int health;
    public int gold;
    public Vector3 position;
    public bool[] checkPointVisited;
    public int difficulty;
    public float playerSkillLevel;
    public int currentMission;
    public float volume;
    public string[] keybinds;

    private void Awake()
    {
        LoadPlayer();
    }

    public void SavePlayer()
    {
        Debug.Log("player saved");
        SaveLoadSystem.SavePlayer(GameObject.FindGameObjectWithTag("Player"));
    }

    public void LoadPlayer()
    {
        Debug.Log("Player loaded");
        PlayerData data = SaveLoadSystem.LoadPlayer();

        EXP = data.EXP;
        health = data.health;
        gold = data.gold;

        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        checkPointVisited = data.checkPointVisited;

        difficulty = data.difficulty;
        playerSkillLevel = data.playerSkillLevel;

        currentMission = data.currentMission;

        volume = data.volume;

        keybinds = data.keybinds;
    }
}
