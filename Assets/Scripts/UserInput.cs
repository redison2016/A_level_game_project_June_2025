using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UserInput : MonoBehaviour
{

    public string moveUpKey = "w";
    public string moveDownKey = "s";
    public string moveRightKey = "d";
    public string moveLeftKey = "a";
    public string attack1Key = "j";
    public string attack2Key = "k";
    public string attack3Key = "l";
    public string healKey = "i";
    public string interactKey = "e";
    public string inventoryKey = "q";

    int velocityX = 0;
    int velocityY = 0;
    GameObject player;
    float jumpStrength;
    GameObject settingsMenu;
    private bool settingsIsOn;
    public bool changingKeybinds;
    public Canvas notice;
    public string[] keysAssigned;
    public bool CheckpointMenuIsOn = false;
    public bool inDialogue = false;
    public GameObject mainManager;
    public GameObject parentButtons;

    private void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player");

        // find settings menu
        settingsMenu = GameObject.Find("SettingsCanvas");

        // get main manager
        mainManager = GameObject.Find("MainManager");

        // initialise settings menu status
        settingsMenu.GetComponent<Canvas>().enabled = false;
        settingsIsOn = false;

        // assign saved keys
        keysAssigned = mainManager.GetComponent<Player>().keybinds;

        moveUpKey = keysAssigned[0];
        moveDownKey = keysAssigned[1];
        moveLeftKey = keysAssigned[2];
        moveRightKey = keysAssigned[3];
        interactKey = keysAssigned[4];
        attack1Key = keysAssigned[5];
        attack2Key = keysAssigned[6];
        attack3Key = keysAssigned[7];
        healKey = keysAssigned[8];
        inventoryKey = keysAssigned[9];

        // change UI
        for (int i = 0; i <= 9; i++)
        {
            parentButtons.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = keysAssigned[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // try find player
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        // check settings input
        SettingsInput();

        if (settingsIsOn)
        {
            return; // ignore other inputs
        }

        // check if in dialogue
        if (inDialogue)
        {
            return; // ignore inputs except settings
        }

        // check for input

        // check gravity to see which physics currently in
        if (GameObject.Find("PhysicsManager").GetComponent<PhysicsScript>().gravity == false)
        {
            TopDownMovementInput(player);
        }
        else
        {
            FrontMovementInput(player);
        }

        AttackInput(player);
        HealInput(player);
    }

    public void EnterDialogue()
    {
        inDialogue = true;
    }

    public void QuitDialogue()
    {
        inDialogue = false;
    }

    public bool ChangeKey(string input, int target) // returns whether change is successful
    {
        // detect key clash
        for (int i = 0; i < keysAssigned.Length; i++)
        {
            string keyAssigned = keysAssigned[i];

            if (keyAssigned == input)
            {
                if (i == target) // if its the input clash with old key
                {
                    return true; 
                }

                // if clash with different key
                return false;
            }
        }

        // change key
        switch (target)
        {
            case (0):
                moveUpKey = input;
                break;
            case (1):
                moveDownKey = input;
                break;
            case (2):
                moveLeftKey = input;
                break;
            case (3):
                moveRightKey = input;
                break;
            case (4):
                interactKey = input;
                break;
            case (5):
                attack1Key = input;
                break;
            case (6):
                attack2Key = input;
                break;
            case (7):
                attack3Key = input;
                break;
            case (8):
                healKey = input;
                break;
        }

        // change list
        keysAssigned[target] = input;

        return true;
    }

    void SettingsInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if notice is up
            if (notice.GetComponent<Canvas>().enabled == true)
            {
                // switch it off
                notice.GetComponent<Canvas>().enabled = false;
                changingKeybinds = false;
                return;
            }

            // else check if settings is up
            if (settingsIsOn)
            {
                // switch off settings menu
                settingsMenu.GetComponent<Canvas>().enabled = false;
                settingsIsOn = false;
            }
            else
            {
                // switch on settings menu
                settingsMenu.GetComponent<Canvas>().enabled = true;
                settingsIsOn = true;
            }
        }
    }

    public void CloseSettings()
    {
        // switch off settings menu
        settingsMenu.GetComponent<Canvas>().enabled = false;
        settingsIsOn = false;
    }

    void HealInput(GameObject player)
    {
        if (Input.GetKey(healKey))
        {
            // heal
            player.GetComponent<PlayerHPManager>().Heal();
        }
    }

    void TopDownMovementInput(GameObject player)
    {
        // set velocity
        if (Input.GetKey(moveUpKey)) // move up
        {
            velocityY += 1;
        }

        if (Input.GetKey(moveLeftKey)) // move left
        {
            velocityX -= 1;
        }

        if (Input.GetKey(moveDownKey)) // move down
        {
            velocityY -= 1;
        }

        if (Input.GetKey(moveRightKey)) // move right
        {
            velocityX += 1;
        }

        player.GetComponent<CharacterMovement>().Move(velocityX, velocityY);

        // reset velocity
        velocityX = 0;
        velocityY = 0;
    }

    void FrontMovementInput(GameObject player)
    {
        // no moving down in this physics
        // set velocity

        if (Input.GetKeyDown(moveUpKey)) // jump 
        {
            // Ground check
            GameObject GroundCheck = player.transform.Find("GroundCheck").gameObject;
            bool onGround = GroundCheck.GetComponent<GroundCheckScript>().onGround;
            if (onGround)
            {
                jumpStrength = player.GetComponent<CharacterMovement>().jumpStrength;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpStrength;
            }
        }

        if (Input.GetKey(moveLeftKey)) // move left
        {
            velocityX -= 1;
        }


        if (Input.GetKey(moveRightKey)) // move right
        {
            velocityX += 1;
        }

        player.GetComponent<CharacterMovement>().Move(velocityX, velocityY);

        // reset velocity
        velocityX = 0;
        velocityY = 0;
    }

    void AttackInput(GameObject player)
    {
        if (Input.GetKeyDown(attack1Key)) // attack 1
        {
            Attack(1, player);
        }

        if (Input.GetKeyDown(attack2Key)) // attack 2
        {
            Attack(2, player);
        }

        if (Input.GetKeyDown(attack3Key)) // attack 3
        {
            Attack(3, player);
        }
    }

    void Attack(int move, GameObject player)
    {
        switch (player.GetComponent<Character>().characterName)
        {
            case "Soldier":
                player.GetComponent<SoldierAttackPattern>().SoldierAttack(move);
                break;
            case "Knight":
                player.GetComponent<KnightAttackPattern>().KnightAttack(move);
                break;
        }

    }

}
