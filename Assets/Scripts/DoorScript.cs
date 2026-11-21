using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    GameObject mapChangeManager;

    private void Start()
    {
        mapChangeManager = GameObject.Find("MapChangeManager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if it is the player
        if (mapChangeManager == null || collision.tag != "Player")
        {
            return;
        }

        // UI indicate interaction available
        collision.transform.Find("InteractIndication").GetComponent<Canvas>().enabled = true;

        // set door in MapChangeManager
        mapChangeManager.GetComponent<MapChangeScript>().door = gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // check if it is the player
        if (mapChangeManager == null || collision.tag != "Player")
        {
            return;
        }

        // switch off indication
        collision.transform.Find("InteractIndication").GetComponent<Canvas>().enabled = false;

        // reset door in MapChangeManager
        mapChangeManager.GetComponent<MapChangeScript>().door = null;
    }

}
