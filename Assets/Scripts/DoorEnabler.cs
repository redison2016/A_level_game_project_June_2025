using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnabler : MonoBehaviour
{
    public GameObject[] Doors;

    public void EnableDoors(int currentMission)
    {
        int doorsNeedEnabled = 0;

        // decide how many doors need enabled
        switch (currentMission)
        {
            case 1:
                doorsNeedEnabled = 0;
                break;
            case 2:
                doorsNeedEnabled = 0;
                break;
            case 3:
                doorsNeedEnabled = 1;
                break;
            case 4:
                doorsNeedEnabled = 3;
                break;
            case 5:
                doorsNeedEnabled = 4;
                break;
            case 6:
                doorsNeedEnabled = 4;
                break;
        }

        // enable the doors
        for (int i = 0; i < doorsNeedEnabled; i++)
        {
            Doors[i].GetComponent<DoorScript>().enabled = true;
        }
    }
}
