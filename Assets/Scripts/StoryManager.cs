using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StoryManager : MonoBehaviour
{
    /* plot:
    
    missions:
    1) make player familiarise with control
    2) kill invaders
    3) go to dungeon kill 5 slimes
    4) save hostile
    5) beat the boss

    */

    public int currentMission;
    public GameObject mainManager;
    public bool missionCompleted;
    public Animator animator;
    public GameObject inputManager;
    public GameObject mapChangeManager;
    public GameObject mainCamera;
    public GameObject orc;
    public GameObject armoredOrc;
    public GameObject player;
    GameObject OrcKing;

    // Start is called before the first frame update
    void Start()
    {
        // get main manager
        mainManager = GameObject.Find("MainManager");

        // initialise current mission
        currentMission = mainManager.GetComponent<Player>().currentMission;

        // get input manager
        inputManager = GameObject.Find("InputManager");

        // get map change manager
        mapChangeManager = GameObject.Find("MapChangeManager");

        // get camera
        mainCamera = GameObject.Find("Main Camera");

        // get player
        player = GameObject.FindGameObjectWithTag("Player");

        // get Orc King
        OrcKing = GameObject.FindGameObjectWithTag("OrcKing");

        // enable doors
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);

        // start mission
        switch (currentMission)
        {
            case (1):
                animator.SetTrigger("WakeUp");
                inputManager.GetComponent<UserInput>().EnterDialogue();
                StartCoroutine(Mission1());
                break;
            case (2):
                StartCoroutine(Mission2());
                break;
            case (3):
                StartCoroutine(Mission3());
                break;
            case (4):
                StartCoroutine(Mission4());
                break;
            case (5):
                StartCoroutine(Mission5());
                break;
        }
    }

    // mission 1
    IEnumerator Mission1()
    {
        while (!missionCompleted)
        {
            yield return new WaitForEndOfFrame();

            // code starts

            // nothing really happens
            // interation with NPC is dealt with in the corresponding scripts

            // code ends
        }

        // after complete
        currentMission = 2;
        missionCompleted = false;
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);
        StartCoroutine(Mission2());
    }


    // mission 2
    public bool spawnEnemy = false;
    bool enemySpawned = false;
    public GameObject orc1 = null;
    public GameObject orc2 = null;
    IEnumerator Mission2()
    {
        while (!missionCompleted)
        {
            yield return new WaitForEndOfFrame();

            // code starts

            if (spawnEnemy)
            {
                SpawnEnemies();
                mainCamera.GetComponent<cameraFollow>().ChangeTarget(GameObject.FindGameObjectWithTag("Orc"));
                spawnEnemy = false;
                enemySpawned = true;
            }

            if (enemySpawned)
            {
                // check if they still alive
                if (orc1.GetComponent<HPManager>().currentHP <= 0 && orc2.GetComponent<HPManager>().currentHP <= 0)
                {
                    missionCompleted = true;
                }
            }

            // code ends
        }

        // after complete
        currentMission = 3;
        missionCompleted = false;
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);
        StartCoroutine(Mission3());
    }

    public void SpawnEnemies()
    {
        orc1 = Instantiate(orc, new Vector3(-53.7f, -10.09f, 0), Quaternion.identity);
        orc2 = Instantiate(orc, new Vector3(-53.7f, -15.07f, 0), Quaternion.identity);
        orc1.GetComponent<EnemyBehavior>().enabled = false;
        orc2.GetComponent<EnemyBehavior>().enabled = false;
    }

    public void MakeOrcMoves()
    {
        orc1.GetComponent<EnemyBehavior>().enabled = true;
        orc2.GetComponent<EnemyBehavior>().enabled = true;
    }

    // mission 3
    IEnumerator Mission3()
    {
        while (!missionCompleted)
        {
            yield return new WaitForEndOfFrame();

            // code starts

            // if player levels up
            if (player.GetComponent<XPManager>().playerLevel >= 2)
            {
                missionCompleted = true;
            }

            // code ends
        }

        // after complete
        currentMission = 4;
        missionCompleted = false;
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);
        StartCoroutine(Mission4());
    }

    // mission 4
    bool mission4EnemiesSpawned = false;
    public GameObject mission4Spawner;
    public GameObject cage;
    bool mission4SpeechIsTriggered = false;
    IEnumerator Mission4()
    {
        while (!missionCompleted)
        {
            yield return new WaitForEndOfFrame();

            // code starts

            if (mapChangeManager.GetComponent<MapChangeScript>().currentArea == "Area1")
            {
                if (!mission4EnemiesSpawned)
                {
                    // start spawning enemies
                    for (int i = 0; i < 30; i += 3)
                    {
                        if (i < 20)
                        {
                            StartCoroutine(Mission4SpawnEnemy(orc, i));
                        }
                        else
                        {
                            StartCoroutine(Mission4SpawnEnemy(armoredOrc, i));
                        }
                    }
                    mission4EnemiesSpawned = true;
                }
            }

            // if cage is destroyed
            if (cage == null || cage.GetComponent<HPManager>().currentHP <= 0)
            {
                missionCompleted = true;
            }

            if (!mission4SpeechIsTriggered && mapChangeManager.GetComponent<MapChangeScript>().currentArea == "CastleArea")
            {
                // trigger speech
                GameObject.Find("Priest").transform.Find("Mission4Opening").GetComponent<Canvas>().enabled = true;

                inputManager.GetComponent<UserInput>().EnterDialogue();

                mission4SpeechIsTriggered = true;
            }

            // code ends
        }

        // after complete
        currentMission = 5;
        missionCompleted = false;
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);
        StartCoroutine(Mission5());
    }

    IEnumerator Mission4SpawnEnemy(GameObject enemy, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject newEnemy = Instantiate(enemy, mission4Spawner.transform.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyBehavior>().sightRange = 10000;
    }

    bool mission5SpeechTriggered = false;
    public GameObject orcKingSpeech;
    IEnumerator Mission5()
    {
        while (!missionCompleted)
        {
            yield return new WaitForEndOfFrame();

            // code starts

            if (!mission5SpeechTriggered && mapChangeManager.GetComponent<MapChangeScript>().currentArea == "CastleInDoor2")
            {
                // trigger speech
                orcKingSpeech.GetComponent<Canvas>().enabled = true;

                // change camera target
                mainCamera.GetComponent<cameraFollow>().ChangeTarget(GameObject.FindGameObjectWithTag("OrcKing"));

                inputManager.GetComponent<UserInput>().EnterDialogue();

                mission5SpeechTriggered = true;
            }

            if (OrcKing == null || OrcKing.GetComponent<HPManager>().currentHP < 0)
            {
                GameObject.Find("PhysicsManager").GetComponent<PhysicsScript>().ChangeGravityScale();
                missionCompleted = true;
            }

            // code ends
        }

        // after complete
        currentMission = 6; // story completed
        missionCompleted = false;
        mapChangeManager.GetComponent<DoorEnabler>().EnableDoors(currentMission);
    }
}
