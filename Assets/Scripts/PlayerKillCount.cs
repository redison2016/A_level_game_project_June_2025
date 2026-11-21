using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillCount : MonoBehaviour
{
    public int killCount;
    public int hitCount;
    public GameObject difficultyManager;

    private void Start()
    {
        // find difficulty manager
        difficultyManager = GameObject.Find("DifficultyManager");

        // initialise kill count
        killCount = 0;
    }

    public void Increment()
    {
        killCount += 1;
        if (killCount >= 10)
        {
            difficultyManager.GetComponent<DifficultyScript>().ChangeSkillLevel(true);
            killCount = 0;
        }
    }

    public void IncrementHitCount()
    {
        hitCount += 1;
        if (hitCount >= 10)
        {
            difficultyManager.GetComponent<DifficultyScript>().ChangeSkillLevel(false);
            hitCount = 0;
        }
    }
}
