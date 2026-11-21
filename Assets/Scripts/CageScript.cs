using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public GameObject mainManager;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("MainManager");

        if (mainManager.GetComponent<Player>().currentMission > 4)
        {
            Destroy(gameObject);
        }
    }
}
