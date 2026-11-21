using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeScript : MonoBehaviour
{

    public GameObject door;
    GameObject player;
    GameObject inputManager;
    private string interactKey;
    [SerializeField] public string currentArea;
    private GameObject physicsManager;
    public GameObject cutScene;

    private void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player");

        // find interact key
        inputManager = GameObject.Find("InputManager");
        interactKey = inputManager.GetComponent<UserInput>().interactKey;

        // find physics manager
        physicsManager = GameObject.Find("PhysicsManager");

        // default spawn area
        currentArea = "MainArea";

        // initialise area
        ChangeArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (door == null)
        {
            return;
        }

        if (player == null)
        {
            // try find player
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        
        if (Input.GetKeyDown(interactKey) == false)
        {
            return;
        }

        // play cut scene
        cutScene.SetActive(false);
        cutScene.SetActive(true);

        // teleport player to designated place
        // add doors here
        switch (door.name)
        {
            case "DungeonEntrance":
                Teleport(player, GameObject.Find("DungeonDoor"));
                currentArea = "Dungeon";
                break;
            case "DungeonDoor":
                Teleport(player, GameObject.Find("DungeonEntrance"));
                currentArea = "MainArea";
                break;
            case "MainAreaDoorLeft":
                Teleport(player, GameObject.Find("JunctionDoorRight"));
                currentArea = "Junction";
                break;
            case "JunctionDoorRight":
                Teleport(player, GameObject.Find("MainAreaDoorLeft"));
                currentArea = "MainArea";
                break;
            case "JunctionDoorLeft":
                Teleport(player, GameObject.Find("Area1DoorRight"));
                currentArea = "Area1";
                break;
            case "Area1DoorRight":
                Teleport(player, GameObject.Find("JunctionDoorLeft"));
                currentArea = "Junction";
                break;
            case "Area1DoorLeft":
                Teleport(player, GameObject.Find("CastleAreaDoorRight"));
                currentArea = "CastleArea";
                break;
            case "CastleAreaDoorRight":
                Teleport(player, GameObject.Find("Area1DoorLeft"));
                currentArea = "Area1";
                break;
            case "CastleEntrance":
                Teleport(player, GameObject.Find("CastleInDoor1DoorLeft"));
                currentArea = "CastleInDoor1";
                physicsManager.GetComponent<PhysicsScript>().ChangeGravityScale(); // change gravity
                break;
            case "CastleInDoor1DoorLeft":
                Teleport(player, GameObject.Find("CastleEntrance"));
                currentArea = "CastleArea";
                physicsManager.GetComponent<PhysicsScript>().ClearGravityScale(); // clear gravity
                break;
            case "CastleInDoor1DoorRight":
                Teleport(player, GameObject.Find("CastleInDoor2DoorLeft"));
                currentArea = "CastleInDoor2";
                break;
            case "CastleInDoor2DoorLeft":
                Teleport(player, GameObject.Find("CastleInDoor1DoorRight"));
                currentArea = "CastleInDoor1";
                break;
        }

        ChangeArea();

    }

    void Teleport(GameObject player, GameObject destination)
    {
        player.transform.position = destination.transform.position;
    }

    private void ChangeArea()
    {
        // call music change function
        GetComponent<MusicManager>().ChangeMusic(currentArea);
    }

}
