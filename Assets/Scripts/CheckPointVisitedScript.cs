using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointVisitedScript : MonoBehaviour
{
    public bool[] checkPointVisited;
    private GameObject mainManager;

    private void Start()
    {
        mainManager = GameObject.Find("MainManager");
        checkPointVisited = mainManager.GetComponent<Player>().checkPointVisited;
    }

    public void Visited(int refNo)
    {
        checkPointVisited[refNo] = true;
    }
}
