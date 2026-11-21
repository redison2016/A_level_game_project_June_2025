using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestNPCScript : MonoBehaviour
{
    public GameObject mainManager;
    public int currentMission;
    public GameObject inputManager;
    public GameObject player;
    public GameObject mainCamera;
    public GameObject interactZone;

    // Start is called before the first frame update
    void Start()
    {
        // get main manager
        mainManager = GameObject.Find("MainManager");

        // initialise current mission
        currentMission = mainManager.GetComponent<Player>().currentMission;

        // get input manager
        inputManager = GameObject.Find("InputManager");

        // get player
        player = GameObject.FindGameObjectWithTag("Player");

        // get camera
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // update current mission
        currentMission = player.GetComponent<StoryManager>().currentMission;

        if (currentMission == 5)
        {
            gameObject.GetComponent<spriteOrderManager>().enabled = true;

            // if player close to NPC
            if (interactZone.GetComponent<InteractZone>().inZone)
            {
                // check if player wants to interact
                if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
                {
                    // talk to player
                    gameObject.transform.Find("Mission5Dialogue").GetComponent<Canvas>().enabled = true;

                    inputManager.GetComponent<UserInput>().EnterDialogue();
                }
            }
        }
    }
}
