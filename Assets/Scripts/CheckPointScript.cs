using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointScript : MonoBehaviour
{
    public bool inZone = false;
    public GameObject inputManager;
    public Canvas menu;
    public bool menuIsOn = false;
    public int checkPointRefNo;
    public bool checkPointVisited;
    private GameObject player;
    public GameObject buttons;

    private void Start()
    {
        // find inputManager
        inputManager = GameObject.Find("InputManager");

        // find player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (inZone)
        {
            // check if interact key is pressed
            if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
            {
                if (menuIsOn)
                {
                    // switch off menu
                    MenuOff();
                }
                else
                {
                    // switch on menu
                    MenuOn();
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if it is the player
        if (collision.tag != "Player")
        {
            return;
        }
        else
        {
            inZone = true;

            // UI indicate interaction available
            collision.transform.Find("InteractIndication").GetComponent<Canvas>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // check if it is the player
        if (collision.tag != "Player")
        {
            return;
        }
        else
        {
            inZone = false;

            // switch off indication
            collision.transform.Find("InteractIndication").GetComponent<Canvas>().enabled = false;
        }
    }

    public void Visited()
    {
        checkPointVisited = true;
    }

    public void FastTravel(int destinationRefNo)
    {
        Vector3 destinationPos = new Vector3();

        switch (destinationRefNo)
        {
            case 0:
                destinationPos = new Vector3(-41f, -7.5f, 0);
                break;
            case 1:
                destinationPos = new Vector3(-428.9f, 5.4f, 0);
                break;
        }
        
        player.transform.position = destinationPos;
    }

    public void MenuOff()
    {
        // close menu
        menu.GetComponent<Canvas>().enabled = false;
        menuIsOn = false;
        inputManager.GetComponent<UserInput>().CheckpointMenuIsOn = false;
    }

    public void MenuOn()
    {
        // open menu
        menu.GetComponent<Canvas>().enabled = true;
        menuIsOn = true;
        inputManager.GetComponent<UserInput>().CheckpointMenuIsOn = true;

        // disable locations that has not been visited
        bool[] checkPointVisited = player.GetComponent<CheckPointVisitedScript>().checkPointVisited;

        for (int i = 0; i < checkPointVisited.Length; i++)
        {
            buttons.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = checkPointVisited[i];
        }
    }
}
