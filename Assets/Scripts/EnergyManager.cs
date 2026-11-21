using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{

    public int maxEnergy = 100;
    int currentEnergy;
    public float regenerateInterval = 0.1f;
    float regenerateTimer;
    private Image energyBar;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;

        // find UI
        GameObject EnergyUI = GameObject.Find("Main Camera").transform.Find("UI").gameObject.transform.Find("Health").gameObject;
        energyBar = EnergyUI.transform.Find("EnergyBar").gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // if timer < 0 && energy not full
        if (regenerateTimer <= 0 && currentEnergy < maxEnergy)
        {
            // regenerate energy
            currentEnergy += 1;
            regenerateTimer = regenerateInterval;
        }

        // update timer
        if (regenerateTimer > 0)
        {
            regenerateTimer -= Time.deltaTime;
        }

        // update energy bar
        if (energyBar != null)
        {
            energyBar.fillAmount = (float)currentEnergy / (float)maxEnergy;
        }
    }

    public bool EnergyCheck(int cost)
    {
        // if not enough energy
        if (cost > currentEnergy)
        {
            return false;
        }

        // if enough energy
        currentEnergy -= cost;
        return true;
    }

}
