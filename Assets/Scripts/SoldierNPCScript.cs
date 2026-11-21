using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierNPCScript : MonoBehaviour
{

    private GameObject mainManager;
    private int currentMission;
    private GameObject inputManager;
    private bool speechIsTriggered = false;
    private GameObject player;
    public GameObject interactZone;
    public GameObject speechCanvas;
    public GameObject mainCamera;
    public GameObject mission2Speech;
    private bool mission2OpeningIsTriggered;
    public GameObject checkPoint;
    public GameObject enemy;

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

        if (currentMission == 1)
        {
            // NPC makes a speech at the start
            if (!speechIsTriggered) 
            {
                // trigger speech
                Invoke("DisplaySpeech", 2f);
                speechIsTriggered = true;
            }

            // if player close to NPC
            if (interactZone.GetComponent<InteractZone>().inZone) 
            {
                // check if player wants to interact
                if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
                {
                    // talk to player
                    gameObject.transform.Find("Mission1Dialogue").GetComponent<Canvas>().enabled = true;

                    inputManager.GetComponent<UserInput>().EnterDialogue();
                }
            }
        }

        if (currentMission == 2)
        {
            // trigger mission 2 opening
            if (!mission2OpeningIsTriggered)
            {
                // display speech bubble
                mission2Speech.GetComponent<Canvas>().enabled = true;

                // change camera target
                ChangeCameraTarget(checkPoint);

                mission2OpeningIsTriggered = true;

                inputManager.GetComponent<UserInput>().EnterDialogue();
            }

            // if player close to NPC
            if (interactZone.GetComponent<InteractZone>().inZone)
            {
                // check if player wants to interact
                if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
                {
                    transform.Find("Mission2Dialogue").GetComponent<Canvas>().enabled = true;

                    inputManager.GetComponent<UserInput>().EnterDialogue();
                }
            }
        }

        if (currentMission == 3)
        {
            // if player close to NPC
            if (interactZone.GetComponent<InteractZone>().inZone)
            {
                // check if player wants to interact
                if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
                {
                    transform.Find("Mission3Dialogue").GetComponent<Canvas>().enabled = true;

                    inputManager.GetComponent<UserInput>().EnterDialogue();
                }
            }
        }

        if (currentMission == 4)
        {
            // if player close to NPC
            if (interactZone.GetComponent<InteractZone>().inZone)
            {
                // check if player wants to interact
                if (Input.GetKeyDown(inputManager.GetComponent<UserInput>().interactKey))
                {
                    transform.Find("Mission4Dialogue").GetComponent<Canvas>().enabled = true;

                    inputManager.GetComponent<UserInput>().EnterDialogue();
                }
            }
        }
    }

    public void MissionComplete()
    {
        player.GetComponent<StoryManager>().missionCompleted = true;
    }

    void DisplaySpeech()
    {
        speechCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void ChangeCameraTarget(GameObject target)
    {
        mainCamera.GetComponent<cameraFollow>().ChangeTarget(target);
    }

    public void TargetPlayer()
    {
        mainCamera.GetComponent<cameraFollow>().ChangeTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    public void SpawnEnemy()
    {
        player.GetComponent<StoryManager>().spawnEnemy = true;
    }

    public void MakeOrcMove()
    {
        player.GetComponent<StoryManager>().MakeOrcMoves();
    }
}
