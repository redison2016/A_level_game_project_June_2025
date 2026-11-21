using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public bool inZone = false;
    GameObject player;
    Canvas interactIndication;

    private void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player");

        // get interact indication
        interactIndication = player.transform.Find("InteractIndication").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inZone = true;
            interactIndication.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inZone = false;
            interactIndication.enabled = false;
        }
    }
}
